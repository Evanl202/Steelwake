using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [Header ("Experience")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

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
        if(currentXP >= xpToNextLevel)
        {
            LevelUp();

        }
    }

    public void LevelUp()
    {
        currentXP -= xpToNextLevel;

        currentXP++;

        xpToNextLevel = Mathf.RoundToInt(
            xpToNextLevel * 1.5f
        );

        Debug.Log(
            "LEVEL UP! Current Level: " + currentLevel
        );
    }
}