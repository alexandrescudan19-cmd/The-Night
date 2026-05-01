using UnityEngine;
using Combat;

public class BuildingHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 30;
    private int _currentHealth;
    
    public bool isPlayerBase = false;

    public int CurrentHealth
    {
        get { return _currentHealth; }
    }

    void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isPlayerBase && LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerLoses();
        }
        
        Destroy(gameObject);
    }
}