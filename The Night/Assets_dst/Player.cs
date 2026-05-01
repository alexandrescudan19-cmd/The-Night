using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player hit! Health: " + health);
        if (health <= 0)
        {
            Debug.Log("Player dead!");
        }
    }
}
