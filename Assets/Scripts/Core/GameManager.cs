using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header ("Game State")]
    public bool gameRunning = true;

    private void Awake()\
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

    public void GameOver()
    {
        if (!gameRunning)
            return
        
        gameRunning = false;
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}