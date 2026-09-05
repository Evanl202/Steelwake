using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel;

    private void start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUp()
    {
        levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Level up panel opened");
    }

    public void SelectSpeed()
    {
        Debug.Log("Speed Upgrade Selected");
        HideLevelUp();
    }

    public void SelectAttack()
    {
        Debug.Log("Attack Upgrade Selected");
        HideLevelUp();
    }

    public void SelectHealth()
    {
        Debug.Log("Health Upgrade Selected");
        HideLevelUp();
    }

    public void HideLevelUp()
    {
        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;

        Debug.Log("Level up panel closed");
    }
}