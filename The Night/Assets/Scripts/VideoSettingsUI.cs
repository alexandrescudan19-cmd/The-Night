using UnityEngine;
using UnityEngine.UI;

public class VideoSettingsUI : MonoBehaviour
{
    public Toggle vSyncToggle;
    public Toggle motionBlurToggle;

    public Behaviour motionBlurComponent; // scriptul pe camera

    void Start()
    {
        bool vsync = PlayerPrefs.GetInt("vSync", 1) == 1;
        bool blur = PlayerPrefs.GetInt("motionBlur", 0) == 1;

        QualitySettings.vSyncCount = vsync ? 1 : 0;
        motionBlurComponent.enabled = blur;

        vSyncToggle.isOn = vsync;
        motionBlurToggle.isOn = blur;

        vSyncToggle.onValueChanged.AddListener(SetVSync);
        motionBlurToggle.onValueChanged.AddListener(SetMotionBlur);
    }

    public void SetVSync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
        PlayerPrefs.SetInt("vSync", value ? 1 : 0);
    }

    public void SetMotionBlur(bool value)
    {
        motionBlurComponent.enabled = value;
        PlayerPrefs.SetInt("motionBlur", value ? 1 : 0);
    }
}

