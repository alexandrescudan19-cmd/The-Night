using UnityEngine;
using Combat;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class TankPlayerController : MonoBehaviour, IDamageable
{
    [Header("Mișcare & Combat")]
    public float moveSpeed = 5f;
    public float attackRange = 1.5f;
    public int attackDamage = 25;
    public float attackCooldown = 1f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask groundLayer;

    [Header("Viață")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;     // ← Prefab de tip HealthBar3D
    private HealthBar3D healthBar;

    private Rigidbody rb;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.isKinematic = true;
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBar = hb.GetComponent<HealthBar3D>();
            healthBar.SetTarget(transform);
            healthBar.SetHealth(1f);
        }
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");

        Vector3 moveDirection = (transform.forward * moveInput + transform.right * strafeInput).normalized;
        moveDirection *= moveSpeed * Time.fixedDeltaTime;

        Vector3 nextPosition = rb.position + moveDirection;

        if (Physics.Raycast(nextPosition + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            nextPosition.y = hit.point.y;
        }

        rb.MovePosition(nextPosition);

        if (animator != null)
        {
            bool isMoving = moveInput != 0 || strafeInput != 0;
            animator.SetBool("merge", isMoving);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        animator?.SetTrigger("Attack");

        Invoke(nameof(DealDamage), 0.3f); // Delay ca să se sincronizeze cu animația
    }

    void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                IDamageable dmg = col.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(attackDamage);
                    Debug.Log($"Lovit: {col.name}");
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float percent = (float)currentHealth / maxHealth;
            healthBar.SetHealth(percent);
        }
    }

    void Die()
    {
        Debug.Log("Player a murit.");
        this.enabled = false;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
