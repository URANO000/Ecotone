using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Scene Navigation")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private const string MusicVolumeKey = "MusicVolume";
    private const string EffectsVolumeKey = "EffectsVolume";
    private const string FullscreenKey = "Fullscreen";

    private void Start()
    {
        LoadSavedSettings();
    }

    private void LoadSavedSettings()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        float savedEffectsVolume = PlayerPrefs.GetFloat(EffectsVolumeKey, 0.5f);
        bool savedFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;

        if (musicSlider != null)
            musicSlider.value = savedMusicVolume;

        if (effectsSlider != null)
            effectsSlider.value = savedEffectsVolume;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreen;
    }

    public void SaveSettings()
    {
        float musicVolume = musicSlider != null ? musicSlider.value : 0.5f;
        float effectsVolume = effectsSlider != null ? effectsSlider.value : 0.5f;
        bool fullscreen = fullscreenToggle != null && fullscreenToggle.isOn;

        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(EffectsVolumeKey, effectsVolume);
        PlayerPrefs.SetInt(FullscreenKey, fullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = fullscreen;

        ReturnToMainMenu();
    }

    public void CancelSettings()
    {
        ReturnToMainMenu();
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}