using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyGameNamespace;

public class SpawnerManager : MonoBehaviour
{
    public enum DifficultyLevel { Easy, Medium, Hard }

    [Header("Soldier Prefabs")]
    public List<GameObject> soldierPrefabs;

    [Header("Spawn Settings")]
    public DifficultyLevel difficulty = DifficultyLevel.Medium;
    public int easySoldiers = 5;
    public int mediumSoldiers = 10;
    public int hardSoldiers = 20;
    public float spawnDelay = 0.1f;
    public Transform spawnPoint;

    [Header("Retreat Points")]
    public Transform retreat1;
    public Transform retreat2;

    public int groupID = 0;

    private List<GameObject> spawnedSoldiers = new List<GameObject>();
    private int soldiersToSpawn;

    void Start()
    {
        soldiersToSpawn = GetSoldierCountByDifficulty();
        StartCoroutine(SpawnArmy());
    }

    int GetSoldierCountByDifficulty()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                return easySoldiers;
            case DifficultyLevel.Medium:
                return mediumSoldiers;
            case DifficultyLevel.Hard:
                return hardSoldiers;
            default:
                return 10;
        }
    }

    IEnumerator SpawnArmy()
    {
        for (int i = 0; i < soldiersToSpawn; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(spawnPoint.position);
            GameObject prefab = soldierPrefabs[Random.Range(0, soldierPrefabs.Count)];
            GameObject soldier = Instantiate(prefab, spawnPosition, Quaternion.identity);

            var controller = soldier.GetComponent<SoldierController>();
            if (controller != null)
            {
                controller.groupID = groupID;

                if (retreat1 != null && retreat2 != null)
                    controller.retreatPath = new Transform[] { retreat1, retreat2 };
                else
                    Debug.LogWarning($"{soldier.name}: Retreat points not set!");
            }

            spawnedSoldiers.Add(soldier);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    Vector3 GetSpawnPosition(Vector3 basePosition)
    {
        float x = Random.Range(basePosition.x - 2f, basePosition.x + 2f);
        float z = Random.Range(basePosition.z - 2f, basePosition.z + 2f);
        Vector3 rayStart = new Vector3(x, basePosition.y + 50f, z);

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f))
        {
            return new Vector3(x, hit.point.y, z);
        }

        return new Vector3(x, basePosition.y, z);
    }

    public List<GameObject> GetSpawnedSoldiers()
    {
        return spawnedSoldiers;
    }
}
