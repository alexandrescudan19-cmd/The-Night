using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace MyGameNamespace
{
    public class SoldierController : MonoBehaviour
    {
        public int maxHP = 100;
        public int attackDamage = 10;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public float moveSpeed = 3f;
        public Slider healthBar;

        private int currentHP;
        private Animator animator;
        private Transform target;
        private bool isAttacking = false;
        private Rigidbody rb;
        private string teamTag;

        void Start()
        {
            currentHP = maxHP;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();

            if (healthBar != null)
            {
                healthBar.maxValue = maxHP;
                healthBar.value = currentHP;
            }

            if (gameObject.CompareTag("Ally"))
                teamTag = "Ally";
            else if (gameObject.CompareTag("Enemy"))
                teamTag = "Enemy";

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.useGravity = true; // Asigură-te că gravitația e activată

            FindNewTarget();
        }

        void FixedUpdate()
        {
            Debug.Log("Velocity: " + rb.velocity);

            if (target == null || target.GetComponent<SoldierController>().IsDead())
            {
                FindNewTarget();
            }

            if (target != null && !isAttacking)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > attackRange)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;

                    rb.MovePosition(newPosition); // Înlătură Raycast-ul care putea cauza sărituri

                    animator.SetBool("merge", true);
                }
                else
                {
                    animator.SetBool("merge", false);
                    rb.velocity = Vector3.zero;
                    StartCoroutine(Attack());
                }
            }
        }

        IEnumerator Attack()
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);

            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                SoldierController enemySoldier = target.GetComponent<SoldierController>();
                if (enemySoldier != null && !enemySoldier.IsDead() && enemySoldier.teamTag != teamTag)
                {
                    Debug.Log(gameObject.name + " lovește " + enemySoldier.gameObject.name);
                    enemySoldier.TakeDamage(attackDamage);
                }
            }
            yield return new WaitForSeconds(attackCooldown - 0.5f);
            isAttacking = false;
        }

        public void TakeDamage(int damage)
        {
            currentHP -= damage;
            if (healthBar != null)
                healthBar.value = currentHP;

            if (currentHP <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            animator.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
            target = null;

            foreach (SoldierController soldier in FindObjectsOfType<SoldierController>())
            {
                if (soldier.target == this.transform)
                {
                    soldier.FindNewTarget();
                }
            }

            Destroy(gameObject, 3f);
        }

        void FindNewTarget()
        {
            string enemyTagToFind = (teamTag == "Ally") ? "Enemy" : "Ally";
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTagToFind);

            float shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                SoldierController enemySoldier = enemy.GetComponent<SoldierController>();
                if (enemySoldier != null && !enemySoldier.IsDead())
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    Debug.Log(gameObject.name + " vede inamicul " + enemy.name + " la distanța de: " + distance);

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemy.transform;
                    }
                }
            }
            target = nearestEnemy;

            if (target != null)
            {
                Debug.Log(gameObject.name + " a ales ca target " + target.name);
            }
            else
            {
                Debug.Log(gameObject.name + " nu a găsit niciun inamic.");
            }
        }

        public bool IsDead()
        {
            return currentHP <= 0;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}