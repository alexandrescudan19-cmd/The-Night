using UnityEngine;

public class TowerShooter : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float range = 15f;
    public float fireRate = 1f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        GameObject target = FindClosestEnemy();

        if (target != null && fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            ShootAt(target.transform);
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float closestDist = range;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    void ShootAt(Transform target)
    {
        if (arrowPrefab == null || shootPoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        ArrowProjectile ap = arrow.GetComponent<ArrowProjectile>();
        if (ap != null)
        {
            ap.SetTarget(target);
        }
    }
}
