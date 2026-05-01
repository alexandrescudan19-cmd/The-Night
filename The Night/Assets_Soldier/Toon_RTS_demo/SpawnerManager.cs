using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    [Header("Soldier Prefabs")]
    public List<GameObject> soldierPrefabs;

    [Header("Spawn Settings")]
    public int soldiersToSpawn = 10;
    public float spawnDelay = 0.1f;
    public Transform spawnPoint;

    private List<GameObject> spawnedSoldiers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnArmy());
    }

    IEnumerator SpawnArmy()
    {
        for (int i = 0; i < soldiersToSpawn; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(spawnPoint.position);
            GameObject soldierPrefab = soldierPrefabs[Random.Range(0, soldierPrefabs.Count)];
            GameObject soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
            spawnedSoldiers.Add(soldier);
            yield return new WaitForSeconds(spawnDelay);
        }

        // După spawn, aranjează formația cu targetPosition corect
        FindObjectOfType<FormationManager1>()?.ArrangeFormation(spawnedSoldiers, spawnPoint.position);
    }

    Vector3 GetSpawnPosition(Vector3 basePosition)
    {
        float x = Random.Range(basePosition.x - 2f, basePosition.x + 2f);
        float z = Random.Range(basePosition.z - 2f, basePosition.z + 2f);
        float y = Terrain.activeTerrain ? Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) : basePosition.y;
        return new Vector3(x, y, z);
    }

    internal List<GameObject> GetSpawnedSoldiers()
    {
        return spawnedSoldiers; // Implementat corect pentru a returna lista soldaților
    }
}
