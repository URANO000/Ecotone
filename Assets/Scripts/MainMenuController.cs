using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string firstLevelSceneName = "Bioma Bosque";

    [Header("Panels")]
    [SerializeField] private GameObject mainButtonsPanel;
    [SerializeField] private GameObject optionsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void OpenOptions()
    {
        mainButtonsPanel.SetActive(false);

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        mainButtonsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}