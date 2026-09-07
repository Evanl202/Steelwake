using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header ("Game State")]
    public bool gameRunning = true;

    [Header ("UI")]
    public GameObject gameOverPanel;

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

    private void Start()
    {
        //If restarted makes game run
        Time.timeScale = 1f;
        gameRunning = true;

        //Hide Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (!gameRunning)
            return;
        
        gameRunning = false;
        Debug.Log("Game Over");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}