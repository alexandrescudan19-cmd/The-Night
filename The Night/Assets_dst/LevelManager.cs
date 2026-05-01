using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1;
    public ZombieSpawner zombieSpawner;

    void Start()
    {
        StartLevel(currentLevel);
    }

    public void StartLevel(int level)
    {
        currentLevel = level;
        zombieSpawner.StartSpawning(level);
    }

    public void NextLevel()
    {
        currentLevel++;
        StartLevel(currentLevel);
    }
}
