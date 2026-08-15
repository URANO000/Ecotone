using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "Bioma Bosque";
    [SerializeField] private string optionsMenuSceneName = "OptionMenu";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;

    private void Start()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayTrack(mainMenuMusic);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(optionsMenuSceneName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}