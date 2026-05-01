/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmySpawner_1 : MonoBehaviour
{
    [Header("Soldier Prefabs")]
    public List<GameObject> soldierPrefabs;

    [Header("Spawn Settings")]
    public int soldiersToSpawn = 10;
    public float spacing = 2f;
    public float spawnDelay = 0.1f;

    private List<GameObject> spawnedSoldiers = new List<GameObject>(); // Lista cu soldații spawnați

    void Start()
    {
        StartCoroutine(SpawnArmy());
    }

    IEnumerator SpawnArmy()
    {
        for (int i = 0; i < soldiersToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject soldierPrefab = soldierPrefabs[Random.Range(0, soldierPrefabs.Count)];
            GameObject soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
            spawnedSoldiers.Add(soldier);
            yield return new WaitForSeconds(spawnDelay);
        }

        // După ce s-au spawnat, așează-i în formație
        ArrangeFormation();
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(transform.position.x - 5, transform.position.x + 5);
        float z = Random.Range(transform.position.z - 5, transform.position.z + 5);
        float y = Terrain.activeTerrain ? Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) : 0;
        return new Vector3(x, y, z);
    }

    void ArrangeFormation()
    {
        int rows = Mathf.CeilToInt(Mathf.Sqrt(spawnedSoldiers.Count));
        int cols = Mathf.CeilToInt(spawnedSoldiers.Count / (float)rows);

        for (int i = 0; i < spawnedSoldiers.Count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            Vector3 formationPosition = transform.position + new Vector3(col * spacing, 0, row * spacing);
            formationPosition.y = Terrain.activeTerrain ? Terrain.activeTerrain.SampleHeight(formationPosition) : 0;

            spawnedSoldiers[i].transform.position = formationPosition;
        }
    }
}
*/
/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmySpawner_1 : MonoBehaviour
{
    [Header("Soldier Prefabs")]
    public List<GameObject> soldierPrefabs;

    [Header("Spawn Settings")]
    public int soldiersPerGroup = 10;
    public float spawnDelay = 0.1f;
    public float spacing = 2f; // Spațiere mai clară între soldați

    private Dictionary<string, List<GameObject>> armyGroups = new Dictionary<string, List<GameObject>>();
    private FormationManager formationManager;

    void Start()
    {
        formationManager = FindObjectOfType<FormationManager>();
        if (formationManager == null)
        {
            Debug.LogError("FormationManager nu a fost găsit în scenă! Asigură-te că este atașat la un GameObject.");
            return;
        }

        // Generăm diferite formații pe teren
        SpawnFormation("LineFormation", new Vector3(0, 0, 0), FormationType.Line);
        SpawnFormation("PhalanxFormation", new Vector3(10, 0, 10), FormationType.Phalanx);
        SpawnFormation("WedgeFormation", new Vector3(-10, 0, -10), FormationType.Wedge);
        SpawnFormation("CircleFormation", new Vector3(15, 0, -15), FormationType.Circle);
        SpawnFormation("SquareFormation", new Vector3(-15, 0, 15), FormationType.Square);
        SpawnFormation("ColumnFormation", new Vector3(5, 0, 5), FormationType.Column);
    }

    void SpawnFormation(string formationName, Vector3 position, FormationType formationType)
    {
        StartCoroutine(SpawnArmy(formationName, position, formationType));
    }

    IEnumerator SpawnArmy(string formationName, Vector3 startPosition, FormationType formationType)
    {
        List<GameObject> soldiers = new List<GameObject>();

        for (int i = 0; i < soldiersPerGroup; i++)
        {
            Vector3 spawnPosition = GetRandomPosition(startPosition);
            GameObject soldierPrefab = soldierPrefabs[Random.Range(0, soldierPrefabs.Count)];
            GameObject soldier = Instantiate(soldierPrefab, spawnPosition, Quaternion.identity);
            soldiers.Add(soldier);
            yield return new WaitForSeconds(spawnDelay);
        }

        armyGroups[formationName] = soldiers;

        // Verificăm dacă formationManager are metoda ArrangeFormation
        if (formationManager != null)
        {
            formationManager.ArrangeFormation(soldiers, startPosition, (FormationManager.FormationType)formationType);
        }
        else
        {
            Debug.LogError("FormationManager nu este setat corect!");
        }
    }

    Vector3 GetRandomPosition(Vector3 basePosition)
    {
        float x = Random.Range(basePosition.x - 3, basePosition.x + 3);
        float z = Random.Range(basePosition.z - 3, basePosition.z + 3);

        float y = (Terrain.activeTerrain != null) ? Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) : 0;
        return new Vector3(x, y, z);
    }

    public void ChangeFormation(string formationName, FormationType newFormation)
    {
        if (armyGroups.ContainsKey(formationName))
        {
            List<GameObject> soldiers = armyGroups[formationName];
            if (soldiers.Count == 0)
            {
                Debug.LogWarning("Nu există soldați în această formație!");
                return;
            }
            Vector3 basePosition = soldiers[0].transform.position;
            formationManager.ArrangeFormation(soldiers, basePosition, (FormationManager.FormationType)newFormation);
        }
        else
        {
            Debug.LogWarning("Formația nu există în dicționarul armyGroups!");
        }
    }
}

public enum FormationType
{
    Line,
    Phalanx,
    Wedge,
    Circle,
    Square,
    Column
}
*/