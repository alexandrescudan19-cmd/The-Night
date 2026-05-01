using UnityEngine;
using UnityEngine.AI;
using Combat;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class ZombieAI : MonoBehaviour, IDamageable
{
    private enum AIState { Idle, Chasing, Attacking, Dead }
    private AIState currentState;

    [Header("Configurare Ținte")]
    [SerializeField] private string allyTag = "Ally";
    [SerializeField] private string buildingTag = "Building";
    [SerializeField] private string playerBaseTag = "PlayerBase";
    [SerializeField] private float detectionRange = 13f;

    [Header("Configurare Atac - Raze")]
    [SerializeField] private float allyAttackRange = 2f;
    [SerializeField] private float buildingAttackRange = 13f;
    [SerializeField] private float playerBaseAttackRange = 25f;

    [Header("Sunet")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip attackSound;

    private float attackInterval;
    private int attackDamage;
    private int maxHealth;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;
    private float timeOfLastAttack;
    private int currentHealth;

    private AudioSource audioSource;

    public void SetupStats(int health, int damage, float interval)
    {
        maxHealth = health;
        attackDamage = damage;
        attackInterval = interval;
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject audioHolder = new GameObject("ZombieAudioSource");
        audioHolder.transform.SetParent(transform);
        audioHolder.transform.localPosition = Vector3.zero;

        audioHolder.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSave;

        audioSource = audioHolder.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
    }

    private void Update()
    {
        if (currentState == AIState.Dead) return;
        HandleTargeting();
        switch (currentState)
        {
            case AIState.Idle: ExecuteIdleState(); break;
            case AIState.Chasing: ExecuteChaseState(); break;
            case AIState.Attacking: ExecuteAttackState(); break;
        }
        UpdateAnimator();
    }

    private void HandleTargeting()
    {
        if (currentTarget != null && !currentTarget.gameObject.activeInHierarchy) { currentTarget = null; }
        if (currentTarget == null)
        {
            Transform nearestAlly = FindNearest(allyTag, detectionRange);
            if (nearestAlly != null)
            {
                currentTarget = nearestAlly;
                agent.stoppingDistance = allyAttackRange;
            }
            else
            {
                Transform nearestBuilding = FindNearest(buildingTag, Mathf.Infinity);
                if (nearestBuilding != null)
                {
                    currentTarget = nearestBuilding;
                    agent.stoppingDistance = 2f;
                }
                else
                {
                    Transform playerBase = FindNearest(playerBaseTag, Mathf.Infinity);
                    if (playerBase != null)
                    {
                        currentTarget = playerBase;
                        agent.stoppingDistance = 2f;
                    }
                }
            }
        }
    }

    private float GetCurrentTargetAttackRange()
    {
        if (currentTarget == null) return 0f;
        if (currentTarget.CompareTag(playerBaseTag)) return playerBaseAttackRange;
        if (currentTarget.CompareTag(buildingTag)) return buildingAttackRange;
        if (currentTarget.CompareTag(allyTag)) return allyAttackRange;
        return 0f;
    }

    private void ExecuteIdleState()
    {
        if (agent.enabled && agent.isOnNavMesh) { agent.isStopped = true; }
        StopWalkSound();

        if (currentTarget != null) { currentState = AIState.Chasing; }
        else
        {
            bool targetsExist = FindNearest(allyTag, Mathf.Infinity) != null ||
                                FindNearest(buildingTag, Mathf.Infinity) != null ||
                                FindNearest(playerBaseTag, Mathf.Infinity) != null;
            if (!targetsExist) { Die(); }
        }
    }

    private void ExecuteChaseState()
    {
        if (currentTarget == null) { currentState = AIState.Idle; return; }
        agent.isStopped = false;
        agent.updateRotation = true;
        PlayWalkSound();

        if (currentTarget.CompareTag(buildingTag) || currentTarget.CompareTag(playerBaseTag))
        {
            agent.SetDestination(currentTarget.GetComponent<Collider>().ClosestPoint(transform.position));
        }
        else { agent.SetDestination(currentTarget.position); }

        float requiredAttackRange = GetCurrentTargetAttackRange();
        if (Vector3.Distance(transform.position, currentTarget.position) <= requiredAttackRange)
        {
            currentState = AIState.Attacking;
        }
    }

    private void ExecuteAttackState()
    {
        if (currentTarget == null) { currentState = AIState.Idle; return; }

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetFloat("Speed", 0f);
        StopWalkSound();

        FaceTarget(currentTarget.position);

        float requiredAttackRange = GetCurrentTargetAttackRange();
        if (Vector3.Distance(transform.position, currentTarget.position) > requiredAttackRange + 0.1f)
        {
            currentState = AIState.Chasing;
            return;
        }

        if (Time.time > timeOfLastAttack + attackInterval)
        {
            timeOfLastAttack = Time.time;
            animator.SetTrigger("Attack");
            Hit();
            PlayAttackSound();
        }
    }

    public void Hit()
    {
        if (currentTarget == null) return;
        IDamageable damageable = currentTarget.GetComponent<IDamageable>();
        if (damageable != null) { damageable.TakeDamage(attackDamage); }
    }

    private void UpdateAnimator()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            float speed = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

            if (speed > 0.1f) PlayWalkSound();
            else StopWalkSound();
        }
    }

    private Transform FindNearest(string tag, float range) { GameObject[] objects = GameObject.FindGameObjectsWithTag(tag); Transform bestTarget = null; float closestDistanceSqr = range * range; foreach (var obj in objects) { if (!obj.activeInHierarchy) continue; float dSqr = (transform.position - obj.transform.position).sqrMagnitude; if (dSqr < closestDistanceSqr) { closestDistanceSqr = dSqr; bestTarget = obj.transform; } } return bestTarget; }

    private void FaceTarget(Vector3 destination) { Vector3 lookPos = destination - transform.position; lookPos.y = 0; if (lookPos == Vector3.zero) return; Quaternion rotation = Quaternion.LookRotation(lookPos); transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f); }

    public void TakeDamage(int amount)
    {
        if (currentState == AIState.Dead) return;
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (currentState == AIState.Dead) return;
        currentState = AIState.Dead;
        if (agent.enabled) { agent.enabled = false; }
        if (GetComponent<Collider>().enabled) { GetComponent<Collider>().enabled = false; }
        animator.SetBool("isDead", true);
        StopWalkSound();
        Destroy(gameObject, 5f);
        this.enabled = false;
    }

    public void StopAI()
    {
        currentState = AIState.Dead;
        if (agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        StopWalkSound();
        this.enabled = false;
    }

    private void PlayWalkSound()
    {
        if (walkSound != null && audioSource != null && (!audioSource.isPlaying || audioSource.clip != walkSound))
        {
            audioSource.clip = walkSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopWalkSound()
    {
        if (audioSource != null && audioSource.clip == walkSound && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}
