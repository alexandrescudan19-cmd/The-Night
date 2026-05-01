using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int zombiesPerWave = 5;

    private int currentLevel = 1;

    public void StartSpawning(int level)
    {
        currentLevel = level;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < zombiesPerWave + currentLevel * 2; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval / Mathf.Clamp(currentLevel, 1, 10));
        }
    }

    void SpawnZombie()
    {
        int prefabIndex = Random.Range(0, zombiePrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(zombiePrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }
}
