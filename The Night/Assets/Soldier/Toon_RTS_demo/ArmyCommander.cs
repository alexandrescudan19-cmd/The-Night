/*using UnityEngine;
using System.Collections.Generic;

public class ArmyCommander : MonoBehaviour
{
    private SpawnerManager spawner;

    void Start()
    {
        spawner = GetComponent<SpawnerManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Click dreapta pentru a ataca
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    OrderAttack(hit.collider.transform);
                }
            }
        }
    }

    void OrderAttack(Transform enemy)
    {
        List<GameObject> soldiers = spawner.GetSpawnedSoldiers();

        if (soldiers.Count == 0) return;

        FormationManager1 formationManager = FindObjectOfType<FormationManager1>();
        formationManager?.ArrangeFormation(soldiers, enemy.position);

        foreach (GameObject soldier in soldiers)
        {
            soldier.GetComponent<SoldierController>().SetTarget(enemy);
        }
    }
}
*/