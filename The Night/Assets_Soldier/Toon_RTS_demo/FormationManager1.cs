using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationManager1 : MonoBehaviour
{
    public enum FormationType { Line, Square, Wedge }
    public FormationType currentFormation = FormationType.Line;
    public float spacing = 2.0f;

    public void ArrangeFormation(List<GameObject> soldiers, Vector3 targetPosition)
    {
        if (soldiers == null || soldiers.Count == 0) return;

        switch (currentFormation)
        {
            case FormationType.Line:
                ArrangeInLine(soldiers, targetPosition);
                break;
            case FormationType.Square:
                ArrangeInSquare(soldiers, targetPosition);
                break;
            case FormationType.Wedge:
                ArrangeInWedge(soldiers, targetPosition);
                break;
        }
    }

    void ArrangeInLine(List<GameObject> soldiers, Vector3 targetPos)
    {
        for (int i = 0; i < soldiers.Count; i++)
        {
            Vector3 newPos = targetPos + new Vector3(i * spacing, 0, 0);
            soldiers[i].GetComponent<NavMeshAgent>().SetDestination(AdjustToTerrain(newPos));
        }
    }

    void ArrangeInSquare(List<GameObject> soldiers, Vector3 targetPos)
    {
        int side = Mathf.CeilToInt(Mathf.Sqrt(soldiers.Count));
        for (int i = 0; i < soldiers.Count; i++)
        {
            int row = i / side;
            int col = i % side;
            Vector3 newPos = targetPos + new Vector3(col * spacing, 0, row * spacing);
            soldiers[i].GetComponent<NavMeshAgent>().SetDestination(AdjustToTerrain(newPos));
        }
    }

    void ArrangeInWedge(List<GameObject> soldiers, Vector3 targetPos)
    {
        for (int i = 0; i < soldiers.Count; i++)
        {
            int row = i / 2;
            int col = (i % 2 == 0) ? -row : row;
            Vector3 newPos = targetPos + new Vector3(col * spacing, 0, row * spacing);
            soldiers[i].GetComponent<NavMeshAgent>().SetDestination(AdjustToTerrain(newPos));
        }
    }

    Vector3 AdjustToTerrain(Vector3 position)
    {
        if (Terrain.activeTerrain != null)
        {
            position.y = Terrain.activeTerrain.SampleHeight(position);
        }
        return position;
    }

    internal void ArrangeFormation(List<GameObject> spawnedSoldiers)
    {
        throw new NotImplementedException();
    }
}
