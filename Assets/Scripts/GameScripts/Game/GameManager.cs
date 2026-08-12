using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button reiniciarButton;
    public Button menuButton;

    [Header("Win UI")]
    public GameObject winPanel;
    public Button winReiniciarButton;
    public Button winMenuButton;

    [Header("Gameplay HUD")]
    public GameObject healthBarCanvas;

    private bool gameOverActivo = false;
    private bool winActivo = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(true);
        }

        if (reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(TryAgain);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(GoToMenu);
        }

        if (winReiniciarButton != null)
        {
            winReiniciarButton.onClick.AddListener(TryAgain);
        }

        if (winMenuButton != null)
        {
            winMenuButton.onClick.AddListener(GoToMenu);
        }
    }

    public void GameOver()
    {
        if (gameOverActivo) return;

        gameOverActivo = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(false);
        }

        Time.timeScale = 0f;
    }

    public void WinLevel()
    {
        if (winActivo) return;

        winActivo = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(false);
        }

        Time.timeScale = 0f;
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}