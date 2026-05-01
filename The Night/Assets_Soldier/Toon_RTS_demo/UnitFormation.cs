/*using UnityEngine;

public enum FormationType { Attack, Defense, Line }

public class UnitFormation : MonoBehaviour
{
    public FormationType currentFormation;

    public void SetFormation(FormationType type)
    {
        currentFormation = type;
        Vector3 positionOffset = Vector3.zero;

        switch (type)
        {
            case FormationType.Attack:
                positionOffset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
                break;
            case FormationType.Defense:
                positionOffset = new Vector3(0, 0, Random.Range(-3, 3));
                break;
            case FormationType.Line:
                positionOffset = new Vector3(transform.position.x, 0, transform.position.z + 5);
                break;
        }

        transform.position += positionOffset;
    }
}
*/