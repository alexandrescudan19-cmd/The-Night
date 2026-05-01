/*using UnityEngine;
using System.Collections.Generic;

public class FormationController : MonoBehaviour
{
    public List<UnitFormation> selectedUnits = new List<UnitFormation>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Formație de atac
            SetFormation(FormationType.Attack);
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Formație de apărare
            SetFormation(FormationType.Defense);
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Formație de linii
            SetFormation(FormationType.Line);
    }

    void SetFormation(FormationType type)
    {
        foreach (var unit in selectedUnits)
        {
            unit.SetFormation(type);
        }
    }
}
*/