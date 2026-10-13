using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [Header ("Experience")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header ("Level Up UI")]
    public LevelUpUI levelUpUI;

    private int pendingLevelUps = 0;
    private bool isProcessingLevelUps = false;

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

    public void AddXP(int amount)
    {
        currentXP += amount;

        Debug.Log(
            "Gained: " + amount +
            "XP. Total XP: " + currentXP +
            "/" + xpToNextLevel
        );

        CheckLevel();
    }

    public void CheckLevel()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;

            xpToNextLevel = Mathf.RoundToInt(
                xpToNextLevel * 1.5f
            );

            pendingLevelUps++;

            Debug.Log(
                "LEVEL UP! Current Level: " + currentLevel
            );
        }

        ProcessNextLevelUp();
    }

    public void ProcessNextLevelUp()
    {
        if (isProcessingLevelUps || pendingLevelUps <= 0)
            return;

        if (levelUpUI == null)
        {
            Debug.LogWarning("ExperienceManager has no LevelUpUI assigned");
            return;
        }

        isProcessingLevelUps = true;
        levelUpUI.ShowLevelUp();
    }

    public void CompleteLevelUpChoice()
    {
        pendingLevelUps--;
        isProcessingLevelUps = false;

        if (pendingLevelUps > 0)
        {
            ProcessNextLevelUp();
        }
        else
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.gameRunning)
            {
                Time.timeScale = 1f;
            }
        }
    }
}