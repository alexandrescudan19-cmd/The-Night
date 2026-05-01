/*using UnityEngine;
using System.Collections.Generic;
using System;

public class FormationManager : MonoBehaviour
{
    public float spacing = 2f;

    public void ArrangeFormation(List<GameObject> soldiers, Vector3 basePosition, FormationType formationType)
    {
        int rows = Mathf.CeilToInt(Mathf.Sqrt(soldiers.Count));
        int cols = Mathf.CeilToInt(soldiers.Count / (float)rows);

        for (int i = 0; i < soldiers.Count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            Vector3 formationOffset = GetFormationOffset(formationType, row, col);
            Vector3 formationPosition = basePosition + formationOffset;
            formationPosition.y = Terrain.activeTerrain.SampleHeight(formationPosition);

            soldiers[i].transform.position = formationPosition;
        }
    }

    private Vector3 GetFormationOffset(FormationType formationType, int row, int col)
    {
        switch (formationType)
        {
            case FormationType.Line:
                return new Vector3(col * spacing, 0, row * spacing * 0.5f);
            case FormationType.Phalanx:
                return new Vector3(col * spacing * 0.5f, 0, row * spacing);
            case FormationType.Wedge:
                return new Vector3(col * spacing - row * spacing * 0.5f, 0, row * spacing);
            case FormationType.Circle:
                float angle = (float)row / 10 * Mathf.PI * 2;
                return new Vector3(Mathf.Cos(angle) * spacing * row, 0, Mathf.Sin(angle) * spacing * row);
            case FormationType.Square:
                return new Vector3(col * spacing, 0, row * spacing);
            case FormationType.Column:
                return new Vector3(0, 0, row * spacing);
            default:
                return new Vector3(col * spacing, 0, row * spacing);
        }
    }

    internal class FormationType
    {
        public static explicit operator FormationType(global::FormationType v)
        {
            throw new NotImplementedException();
        }
    }
}
*/