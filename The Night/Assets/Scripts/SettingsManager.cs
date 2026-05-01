using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("AUDIO")]
    public Slider musicVolumeSlider;
    public MusicPlayer musicPlayer;

    [Header("HUD & Tooltips")]
    public Toggle hudToggle;
    public Toggle tooltipToggle;
    public GameObject hudRoot;
    public GameObject tooltipRoot;

    [Header("CONTROLS")]
    public Slider mouseSensitivitySlider;
    public Toggle invertMouseToggle;

    [Header("VIDEO")]
    public Toggle vSyncToggle;
    public Toggle motionBlurToggle;
    public Behaviour motionBlurComponent;

    private void Start()
    {
        LoadSettings();

        // AUDIO
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // HUD & Tooltips
        if (hudToggle != null)
            hudToggle.onValueChanged.AddListener(SetHUDVisible);
        if (tooltipToggle != null)
            tooltipToggle.onValueChanged.AddListener(SetTooltipsVisible);

        // CONTROLS
        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        if (invertMouseToggle != null)
            invertMouseToggle.onValueChanged.AddListener(SetInvertMouse);

        // VIDEO
        if (vSyncToggle != null)
            vSyncToggle.onValueChanged.AddListener(SetVSync);
        if (motionBlurToggle != null)
            motionBlurToggle.onValueChanged.AddListener(SetMotionBlur);
    }

    private void LoadSettings()
    {
        // AUDIO
        float volume = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = volume;
        if (musicPlayer != null)
            musicPlayer.SetVolume(volume);

        // HUD & Tooltips
        bool hud = PlayerPrefs.GetInt("showHUD", 1) == 1;
        bool tooltips = PlayerPrefs.GetInt("showTooltips", 1) == 1;
        if (hudToggle != null)
            hudToggle.isOn = hud;
        if (tooltipToggle != null)
            tooltipToggle.isOn = tooltips;
        if (hudRoot != null)
            hudRoot.SetActive(hud);
        if (tooltipRoot != null)
            tooltipRoot.SetActive(tooltips);

        // CONTROLS
        float sensitivity = PlayerPrefs.GetFloat("mouseSens", 1f);
        bool invert = PlayerPrefs.GetInt("invertMouse", 0) == 1;
        if (mouseSensitivitySlider != null)
            mouseSensitivitySlider.value = sensitivity;
        if (invertMouseToggle != null)
            invertMouseToggle.isOn = invert;

        // VIDEO
        bool vSync = PlayerPrefs.GetInt("vSync", 1) == 1;
        bool blur = PlayerPrefs.GetInt("motionBlur", 0) == 1;
        if (vSyncToggle != null)
            vSyncToggle.isOn = vSync;
        if (motionBlurToggle != null)
            motionBlurToggle.isOn = blur;

        QualitySettings.vSyncCount = vSync ? 1 : 0;
        if (motionBlurComponent != null)
            motionBlurComponent.enabled = blur;
    }

    // AUDIO
    public void SetMusicVolume(float value)
    {
        if (musicPlayer != null)
            musicPlayer.SetVolume(value);

        PlayerPrefs.SetFloat("musicVolume", value);
    }

    // HUD
    public void SetHUDVisible(bool value)
    {
        if (hudRoot != null)
            hudRoot.SetActive(value);
        PlayerPrefs.SetInt("showHUD", value ? 1 : 0);
    }

    public void SetTooltipsVisible(bool value)
    {
        if (tooltipRoot != null)
            tooltipRoot.SetActive(value);
        PlayerPrefs.SetInt("showTooltips", value ? 1 : 0);
    }

    // CONTROLS
    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("mouseSens", value);
    }

    public void SetInvertMouse(bool value)
    {
        PlayerPrefs.SetInt("invertMouse", value ? 1 : 0);
    }

    // VIDEO
    public void SetVSync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
        PlayerPrefs.SetInt("vSync", value ? 1 : 0);
    }

    public void SetMotionBlur(bool value)
    {
        if (motionBlurComponent != null)
            motionBlurComponent.enabled = value;
        PlayerPrefs.SetInt("motionBlur", value ? 1 : 0);
    }
}
