using UnityEngine;

public class DamageSystem1 : MonoBehaviour
{
    public float damageAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount);
            Debug.Log(other.gameObject.name + " a primit " + damageAmount + " damage!");
        }
    }
}
