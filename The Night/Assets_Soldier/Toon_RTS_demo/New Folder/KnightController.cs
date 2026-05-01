using UnityEngine;
using System.Collections;

public class KnightController : MonoBehaviour
{
    public Transform target;  // Inamicul pe care îl atacă
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        float distanceToEnemy = Vector3.Distance(transform.position, target.position);

        if (distanceToEnemy > attackRange)
        {
            MoveTowardsEnemy();
        }
        else
        {
            AttackEnemy();
        }
    }

    void MoveTowardsEnemy()
    {
        if (isAttacking) return;

        animator.SetBool("merge", true);
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(target);
    }

    void AttackEnemy()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        isAttacking = true;
        animator.SetBool("merge", false);
        animator.SetTrigger("Attack");

        lastAttackTime = Time.time;

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f); // Timpul până când atacul este aplicat
        // Aici poți adăuga logica de a scădea viața inamicului
        isAttacking = false;
    }
}
