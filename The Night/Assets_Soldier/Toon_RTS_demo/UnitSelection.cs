/*using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public bool isSelected = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        isSelected = !isSelected;
        rend.material.color = isSelected ? Color.green : originalColor;
    }
}
*/