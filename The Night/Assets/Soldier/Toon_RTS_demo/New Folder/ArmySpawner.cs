using UnityEngine;
using System.Collections;

public class ArmySpawner1 : MonoBehaviour
{
    public GameObject soldierPrefab;
    public int rows1 = 2;
    public int cols1 = 2;
    public float spacing1 = 2f;
    public float spawnDelay = 0.1f; // Delay între instanțieri

    void Start()
    {
        StartCoroutine(SpawnArmy());
    }

    IEnumerator SpawnArmy()
    {
        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols1; j++)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position + new Vector3(j * spacing1, 0, i * spacing1));
                Vector3 position = new Vector3(transform.position.x + j * spacing1, terrainHeight + 1f, transform.position.z + i * spacing1);

                Debug.Log($"Instantiating soldier at: {position}");
                Instantiate(soldierPrefab, position, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay); // Așteaptă înainte de următorul spawn
            }
        }
    }

}
