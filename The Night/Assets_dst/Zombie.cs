using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 2f;
    public int health = 100;
    public float attackInterval = 1.5f;
    public float stopDistance = 1.5f;

    private Transform target;
    private Animator animator;

    private float attackTimer = 0f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("PlayerBase")?.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null || animator == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            // Merge spre player
            transform.position += direction * speed * Time.deltaTime;
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);

            // Opțional: zombie-ul se rotește spre player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            // E în range de atac
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);

            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                Attack();
                attackTimer = 0f;
            }

            // Oprire completă (dacă are Rigidbody)
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = Vector3.zero;
        }
    }

    void Attack()
    {
        if (target == null) return;

        PlayerBase player = target.GetComponent<PlayerBase>();
        if (player != null)
        {
            player.TakeDamage(10);
        }

        // Poți activa asta dacă vrei ca zombie-ul să moară după atac:
        // Die();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        this.enabled = false;
        Destroy(gameObject, 2f); // Distrugem zombie-ul după animația de moarte
    }
}
