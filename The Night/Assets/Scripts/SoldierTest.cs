using UnityEngine;

public class SoldierTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("PAS apasat"); // doar pentru test în consolă
            GetComponent<SoldierSound>().PlayWalkSound();
        }
    }
}
