using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private string optionsSceneName = "OptionMenu";

    public bool IsPaused { get; private set; }
    private bool isOptionsSceneLoaded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (IsPaused) return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayPauseMenuMusic();

        if (!isOptionsSceneLoaded)
        {
            SceneManager.LoadScene(optionsSceneName, LoadSceneMode.Additive);
            isOptionsSceneLoaded = true;
        }
    }
    public void ResumeGame()
    {
        if (!IsPaused) return;

        IsPaused = false;
        Time.timeScale = 1f;

        if (MusicManager.Instance != null)
            MusicManager.Instance.ResumeZoneMusic();

        if (isOptionsSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(optionsSceneName);
            isOptionsSceneLoaded = false;
        }
    }
}