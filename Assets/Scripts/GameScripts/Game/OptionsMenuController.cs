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
    [Tooltip("Solo se usa si el menú se abrió desde el Main Menu real, no desde la pausa in-game.")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private const string FullscreenKey = "Fullscreen";
    private bool savedFullscreenAtOpen;

    private void Start()
    {
        LoadSavedSettings();

        if (effectsSlider != null && SFXManager.Instance != null)
            effectsSlider.onValueChanged.AddListener(SFXManager.Instance.PreviewSFXVolume);

        if (musicSlider != null && MusicManager.Instance != null)
            musicSlider.onValueChanged.AddListener(MusicManager.Instance.PreviewMusicVolume);
    }

    private void LoadSavedSettings()
    {
        if (musicSlider != null)
            musicSlider.value = MusicManager.Instance != null ? MusicManager.Instance.GetSavedMusicVolume() : 1f;

        if (effectsSlider != null)
            effectsSlider.value = SFXManager.Instance != null ? SFXManager.Instance.GetSavedSFXVolume() : 1f;

        savedFullscreenAtOpen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreenAtOpen;
    }

    public void SaveSettings()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.CommitSFXVolume();
        if (MusicManager.Instance != null) MusicManager.Instance.CommitMusicVolume();

        bool fullscreen = fullscreenToggle != null && fullscreenToggle.isOn;
        PlayerPrefs.SetInt(FullscreenKey, fullscreen ? 1 : 0);
        PlayerPrefs.Save();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = fullscreen;

        CloseMenu();
    }

    public void CancelSettings()
    {
        if (SFXManager.Instance != null && effectsSlider != null)
            effectsSlider.value = SFXManager.Instance.RevertSFXVolume();

        if (MusicManager.Instance != null && musicSlider != null)
            musicSlider.value = MusicManager.Instance.RevertMusicVolume();

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreenAtOpen;

        CloseMenu();
    }
    private void CloseMenu()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
        {
            PauseManager.Instance.ResumeGame();
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}