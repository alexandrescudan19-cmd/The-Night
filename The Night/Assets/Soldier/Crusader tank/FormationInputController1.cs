using System.Collections.Generic;
using UnityEngine;
using MyGameNamespace;

public class MultiFormationInputController : MonoBehaviour
{
    private FormationManager formationManager;
    private SpawnerManager[] spawners;

    void Start()
    {
        formationManager = FindObjectOfType<FormationManager>();
        spawners = FindObjectsOfType<SpawnerManager>();
    }

    void Update()
    {
        // Grup 0
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ApplyFormationToGroup(FormationType.Rectangle, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ApplyFormationToGroup(FormationType.Circle, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ApplyFormationToGroup(FormationType.Triangle, 0);

        // Grup 1
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ApplyFormationToGroup(FormationType.Rectangle, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ApplyFormationToGroup(FormationType.Circle, 1);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ApplyFormationToGroup(FormationType.Triangle, 1);
    }

    void ApplyFormationToGroup(FormationType type, int groupID)
    {
        formationManager.currentFormation = type;

        List<GameObject> groupSoldiers = new List<GameObject>();

        foreach (var spawner in spawners)
        {
            var allSoldiers = spawner.GetSpawnedSoldiers();
            if (allSoldiers == null || allSoldiers.Count == 0)
                continue;

            foreach (var s in allSoldiers)
            {
                var ctrl = s.GetComponent<SoldierController>();
                if (ctrl != null && ctrl.groupID == groupID)
                {
                    groupSoldiers.Add(s);
                }
            }
        }

        if (groupSoldiers.Count > 0)
        {
            Vector3 center = CalculateCenter(groupSoldiers);
            formationManager.ArrangeFormation(groupSoldiers, center);
        }
    }

    Vector3 CalculateCenter(List<GameObject> soldiers)
    {
        Vector3 sum = Vector3.zero;
        foreach (var s in soldiers)
            sum += s.transform.position;
        return sum / soldiers.Count;
    }
}
