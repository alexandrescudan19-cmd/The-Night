using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuWorld : MonoBehaviour
{
    [Header("UI & Camera References")]
    public GameObject pauseMenuUI;     // Canv_Main
    public GameObject optionsMenuUI;   // Canv_Options

    public GameObject mainCamObj;      // camera main ca GameObject
    public GameObject pauseCamObj;     // camera pause ca GameObject
    public GameObject optionsCamObj;   // camera options ca GameObject

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        SetOnlyCameraActive(mainCamObj);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);

        SetOnlyCameraActive(pauseCamObj);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Pause menu opened");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);

        SetOnlyCameraActive(mainCamObj);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Game resumed");
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);

        SetOnlyCameraActive(optionsCamObj);

        Debug.Log("Options menu opened");
    }

    public void BackToPause()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);

        SetOnlyCameraActive(pauseCamObj);

        Debug.Log("Returned to pause menu");
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void SetOnlyCameraActive(GameObject activeCam)
    {
        mainCamObj.SetActive(false);
        pauseCamObj.SetActive(false);
        optionsCamObj.SetActive(false);

        activeCam.SetActive(true);
    }
}
