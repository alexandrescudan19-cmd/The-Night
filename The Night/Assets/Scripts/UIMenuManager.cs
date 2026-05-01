using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject textDefault;    // Obiectul "Text" (The game is paused)
    public GameObject tutorialPanel;  // Obiectul "Tutorial"

    public void ShowTutorial()
    {
        if (textDefault != null) textDefault.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        if (textDefault != null) textDefault.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }
}
