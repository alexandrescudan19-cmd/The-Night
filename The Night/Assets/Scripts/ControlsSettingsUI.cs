using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingsUI : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public Toggle invertMouseToggle;

    void Start()
    {
        float sens = PlayerPrefs.GetFloat("mouseSens", 1f);
        bool invert = PlayerPrefs.GetInt("invertMouse", 0) == 1;

        mouseSensitivitySlider.value = sens;
        invertMouseToggle.isOn = invert;

        mouseSensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        invertMouseToggle.onValueChanged.AddListener(SetInvertMouse);
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("mouseSens", value);
        // Aplică în input controller-ul tău
    }

    public void SetInvertMouse(bool value)
    {
        PlayerPrefs.SetInt("invertMouse", value ? 1 : 0);
        // Aplică în controller-ul tău
    }
}
