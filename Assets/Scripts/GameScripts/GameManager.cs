using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public Button reiniciarButton;
    public Button menuButton;

    private bool gameOverActivo = false;
    // Start is called before the first frame update

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
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if(reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(TryAgain);
        }

        if(menuButton != null)
        {
            menuButton.onClick.AddListener(GoToMenu);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        if (gameOverActivo) return;

        gameOverActivo = true;

        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
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
