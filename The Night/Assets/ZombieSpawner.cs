using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;

    public void SpawnZombie(LevelManager.LevelData levelData)
    {
        if (zombiePrefab != null)
        {
            GameObject newZombieGO = Instantiate(zombiePrefab, transform.position, transform.rotation);
            ZombieAI zombieScript = newZombieGO.GetComponent<ZombieAI>();

            if (zombieScript != null)
            {
                zombieScript.SetupStats(
                    levelData.zombieHealth, 
                    levelData.zombieDamage, 
                    levelData.zombieAttackInterval
                );
            }
        }
        else
        {
            Debug.LogWarning("Prefab-ul de zombi nu este setat pe spawner-ul: " + gameObject.name);
        }
    }
}