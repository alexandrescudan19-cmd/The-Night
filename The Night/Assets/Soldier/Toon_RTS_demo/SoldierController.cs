using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Combat;

namespace MyGameNamespace
{
    public class SoldierController : MonoBehaviour, IDamageable
    {
        public int maxHP = 100;
        public int attackDamage = 10;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public float moveSpeed = 3f;
        public float rotateSpeed = 5f;
        public Slider healthBar;

        public Transform retreatTarget; // Nou

        public int groupID;

        int currentHP;
        Animator animator;
        Transform target;
        Rigidbody rb;
        string teamTag;
        bool isRetreating;
        float attackTimer = 0f;

        bool isAttacking;
        public bool canAttack = false;

        NavMeshAgent agent;
        Transform followTarget;

        public AudioClip walkSound;
        public AudioClip attackSound;
        AudioSource audioSource;
        public Transform[] retreatPath;


        public Animator Animator => animator;

        void Start()
        {
            currentHP = maxHP;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();
            GameObject audioHolder = new GameObject("SoldierAudioSource");
            audioHolder.transform.SetParent(transform);
            audioHolder.transform.localPosition = Vector3.zero;

            // Ascunde-l complet din Inspector și ierarhie
            audioHolder.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;

            audioSource = audioHolder.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // sunet 3D, dacă vrei


            if (healthBar != null)
            {
                healthBar.maxValue = maxHP;
                healthBar.value = currentHP;
            }

            teamTag = gameObject.CompareTag("Ally") ? "Ally" : "Enemy";

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }

            if (agent != null)
            {
                agent.speed = moveSpeed;
                agent.angularSpeed = rotateSpeed * 100f;
                agent.stoppingDistance = attackRange * 0.9f;
            }
            if (retreatTarget == null)
            {
                GameObject go = GameObject.Find("RetreatPoint");
                if (go != null) retreatTarget = go.transform;
            }
            StickToGround();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canAttack = !canAttack;
                followTarget = null;

                if (canAttack)
                {
                    FindNewTarget();
                }
                else
                {
                    target = null;
                    animator.SetBool("merge", false);
                    StopWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && teamTag == "Ally")
            {
                StartRetreat();
            }

            if (isRetreating && retreatTarget != null)
            {
                float distance = Vector3.Distance(transform.position, retreatTarget.position);

                if (distance > 0.5f)
                {
                    animator.SetBool("merge", true);
                    PlayWalkSound();
                    if (agent != null && agent.isOnNavMesh)
                        agent.SetDestination(retreatTarget.position);
                }
                else
                {
                    animator.SetBool("merge", false);
                    StopWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                    isRetreating = false;
                    canAttack = false;
                }

                return;
            }

            if (followTarget != null && !isRetreating)
            {
                float distance = Vector3.Distance(transform.position, followTarget.position);
                if (distance > 2f)
                {
                    animator.SetBool("merge", true);
                    PlayWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.SetDestination(followTarget.position);
                }
                else
                {
                    animator.SetBool("merge", false);
                    StopWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                }
                return;
            }

            if (!canAttack)
            {
                animator.SetBool("merge", false);
                StopWalkSound();
                if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                return;
            }

            if (TargetInvalid())
                FindNewTarget();

            if (target == null)
            {
                animator.SetBool("merge", false);
                StopWalkSound();
                if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Vector3 dir = (target.position - transform.position).normalized;
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);

            if (!isAttacking)
            {
                if (distanceToTarget > attackRange)
                {
                    animator.SetBool("merge", true);
                    PlayWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.SetDestination(target.position);
                }
                else
                {
                    animator.SetBool("merge", false);
                    StopWalkSound();
                    if (agent != null && agent.isOnNavMesh) agent.ResetPath();
                    Face(flatDir);

                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackCooldown)
                    {
                        Attack();
                        attackTimer = 0f;
                    }
                }
            }
            else
            {
                Face(flatDir);
            }
        }

        void Face(Vector3 flatDir)
        {
            if (flatDir.sqrMagnitude < 0.001f) return;

            Quaternion targetRot = Quaternion.LookRotation(flatDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        void Attack()
        {
            if (target == null) return;

            animator.SetTrigger("Attack");

            if (attackSound != null && audioSource != null)
                audioSource.PlayOneShot(attackSound);

            IDamageable dmg = target.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(attackDamage);
        }

        public void TakeDamage(int dmg)
        {
            currentHP -= dmg;
            if (healthBar != null) healthBar.value = currentHP;
            if (currentHP <= 0) Die();
        }

        bool IsDead() => currentHP <= 0;

        void Die()
        {
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
            target = null;
            if (agent != null) agent.enabled = false;

            foreach (SoldierController s in FindObjectsOfType<SoldierController>())
                if (s.target == transform) s.FindNewTarget();

            Destroy(gameObject, 3f);
        }

        bool TargetInvalid()
        {
            if (target == null) return true;

            IDamageable dmg = target.GetComponent<IDamageable>();
            if (dmg == null) return true;

            SoldierController sc = target.GetComponent<SoldierController>();
            if (sc != null && sc.IsDead()) return true;

            return false;
        }

        public void FindNewTarget()
        {
            string enemyTag = (teamTag == "Ally") ? "Enemy" : "Ally";
            GameObject[] candidates = GameObject.FindGameObjectsWithTag(enemyTag);

            float bestDist = Mathf.Infinity;
            Transform best = null;

            foreach (GameObject go in candidates)
            {
                IDamageable dmg = go.GetComponent<IDamageable>();
                if (dmg == null) continue;

                SoldierController sc = go.GetComponent<SoldierController>();
                if (sc != null && sc.IsDead()) continue;

                float d = Vector3.Distance(transform.position, go.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = go.transform;
                }
            }
            target = best;
        }

        void StartRetreat()
        {
            if (retreatTarget == null)
            {
                Debug.LogWarning($"{name}: Retreat target NOT set!");
                return;
            }

            isRetreating = true;
            target = null;
            followTarget = null;
            animator.SetBool("merge", true);
            PlayWalkSound();

            if (agent != null && agent.isOnNavMesh)
                agent.SetDestination(retreatTarget.position);
        }

        void StickToGround()
        {
            RaycastHit hit;
            Vector3 rayStart = transform.position + Vector3.up * 2f;
            Vector3 rayDir = Vector3.down;

            if (Physics.Raycast(rayStart, rayDir, out hit, 10f, LayerMask.GetMask("Ground")))
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y;
                transform.position = pos;
            }
        }

        public void FollowMe(Transform leader)
        {
            followTarget = leader;
            target = null;
            canAttack = false;
            isRetreating = false;
            animator.SetBool("merge", true);
            PlayWalkSound();
            if (agent != null && agent.isOnNavMesh) agent.SetDestination(leader.position);
        }

        public void StopFollowing()
        {
            followTarget = null;
            animator.SetBool("merge", false);
            StopWalkSound();
            if (agent != null && agent.isOnNavMesh) agent.ResetPath();
        }

        void PlayWalkSound()
        {
            if (walkSound != null && audioSource != null && (!audioSource.isPlaying || audioSource.clip != walkSound))
            {
                audioSource.clip = walkSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        void StopWalkSound()
        {
            if (audioSource != null && audioSource.clip == walkSound && audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
