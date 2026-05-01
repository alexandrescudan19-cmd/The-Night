using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public int[] zombiesPerWave;
        public float spawnRate;
        public int zombieHealth;
        public int zombieDamage;
        public float zombieAttackInterval;
    }

    [Header("Level Setup")]
    public int currentLevelIndex = 0;
    public LevelData[] levels;
    public List<ZombieSpawner> zombieSpawners;

    [Header("Game End UI")]
    public GameObject pauseMenuUI;
    public GameObject winnerUI;
    public GameObject loserUI;
    public GameObject pausedTextObject; // 👈 referință la textul "The game is paused"

    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject pauseCamera;

    private int currentWaveIndex = 0;
    private int _zombiesAlive = 0;
    private bool _levelEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartLevel(currentLevelIndex);
    }

    public void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.Log("Felicitări! Ai terminat toate nivelele!");
            return;
        }

        currentLevelIndex = levelIndex;
        currentWaveIndex = 0;
        _zombiesAlive = 0;
        _levelEnded = false;

        StopAllCoroutines();
        StartCoroutine(RunLevelCoroutine());
    }

    private IEnumerator RunLevelCoroutine()
    {
        LevelData currentLevelData = levels[currentLevelIndex];
        Debug.Log($"🧟 Începe Nivelul: {currentLevelData.levelName}");

        while (currentWaveIndex < currentLevelData.zombiesPerWave.Length)
        {
            if (currentWaveIndex > 0)
            {
                Debug.Log("⏳ Următorul val începe în 10 secunde...");
                yield return new WaitForSeconds(10f);
            }

            int zombiesToSpawn = currentLevelData.zombiesPerWave[currentWaveIndex];
            Debug.Log($"⚔️ Valul {currentWaveIndex + 1} începe! Se vor spawna {zombiesToSpawn} zombii.");

            for (int i = 0; i < zombiesToSpawn; i++)
            {
                if (zombieSpawners.Count > 0)
                {
                    ZombieSpawner randomSpawner = zombieSpawners[Random.Range(0, zombieSpawners.Count)];
                    randomSpawner.SpawnZombie(currentLevelData);
                    _zombiesAlive++;
                }

                yield return new WaitForSeconds(currentLevelData.spawnRate);
            }

            currentWaveIndex++;
        }

        Debug.Log("✅ Toate valurile au fost spawnate. Așteptăm ca jucătorul să elimine toți zombii...");
        yield return new WaitUntil(() => _zombiesAlive <= 0);

        if (!_levelEnded)
        {
            _levelEnded = true;
            OnPlayerWins();
        }
    }

    public void RegisterZombie()
    {
        _zombiesAlive++;
    }

    public void UnregisterZombie()
    {
        _zombiesAlive--;

        if (_zombiesAlive <= 0 &&
            currentWaveIndex >= levels[currentLevelIndex].zombiesPerWave.Length &&
            !_levelEnded)
        {
            _levelEnded = true;
            OnPlayerWins();
        }
    }

    public void OnPlayerWins()
    {
        if (_levelEnded) return;
        _levelEnded = true;

        StopAllCoroutines();
        Debug.Log("🏆 JUCĂTORUL A CÂȘTIGAT!");

        DisableAllZombies();
        ShowEndUI(true);
    }

    public void OnPlayerLoses()
    {
        if (_levelEnded) return;
        _levelEnded = true;

        StopAllCoroutines();
        Debug.Log("💀 JUCĂTORUL A PIERDUT!");

        DisableAllZombies();
        ShowEndUI(false);
    }

    private void DisableAllZombies()
    {
        ZombieAI[] allZombies = FindObjectsOfType<ZombieAI>();
        foreach (ZombieAI zombie in allZombies)
        {
            zombie.StopAI();
        }

        Debug.Log($"🧟 {allZombies.Length} zombii au fost opriți.");
    }

    private void ShowEndUI(bool win)
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (winnerUI != null) winnerUI.SetActive(win);
        if (loserUI != null) loserUI.SetActive(!win);

        // 🔁 camera switch
        if (mainCamera != null) mainCamera.SetActive(false);
        if (pauseCamera != null) pauseCamera.SetActive(true);

        // 🔕 dezactivează textul "game is paused"
        if (pausedTextObject != null) pausedTextObject.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
