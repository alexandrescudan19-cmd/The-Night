using UnityEngine;
using Combat;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 15;
    public GameObject impactEffectPrefab;

    private Transform target;

    // Corecție pentru model rotit greșit
    public Vector3 modelRotationOffset = new Vector3(-90f, 0f, 0f); // ← ajustezi dacă vârful e în sus

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation * Quaternion.Euler(modelRotationOffset);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            IDamageable dmg = target.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }

            if (impactEffectPrefab != null)
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
