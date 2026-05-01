using UnityEngine;
using UnityEngine.UI;

public class HUDSettingsUI : MonoBehaviour
{
    public Toggle hudToggle;
    public Toggle tooltipToggle;

    public GameObject hudRoot;
    public GameObject tooltipRoot;

    void Start()
    {
        // Load saved state
        bool showHUD = PlayerPrefs.GetInt("showHUD", 1) == 1;
        bool showTooltips = PlayerPrefs.GetInt("showTooltips", 1) == 1;

        hudRoot.SetActive(showHUD);
        tooltipRoot.SetActive(showTooltips);

        hudToggle.isOn = showHUD;
        tooltipToggle.isOn = showTooltips;

        hudToggle.onValueChanged.AddListener(ToggleHUD);
        tooltipToggle.onValueChanged.AddListener(ToggleTooltips);
    }

    public void ToggleHUD(bool value)
    {
        hudRoot.SetActive(value);
        PlayerPrefs.SetInt("showHUD", value ? 1 : 0);
    }

    public void ToggleTooltips(bool value)
    {
        tooltipRoot.SetActive(value);
        PlayerPrefs.SetInt("showTooltips", value ? 1 : 0);
    }
}
