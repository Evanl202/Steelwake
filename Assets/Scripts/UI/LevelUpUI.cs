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

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.UpgradeSpeed();
        }

        HideLevelUp();
    }

    public void SelectDamage()
    {
        Debug.Log("Damage Upgrade Selected");

        // if (UpgradeManager.Instance != null)
        // {
        //     UpgradeManager.Instance.UpgradeSpeed();
        // }

        HideLevelUp();
    }

    public void SelectHealth()
    {
        Debug.Log("Health Upgrade Selected");

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.UpgradeSpeed();
        }

        HideLevelUp();
    }

    public void SelectReload()
    {
        Debug.Log("Reload Upgrade Selected");

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.UpgradeSpeed();
        }

        HideLevelUp();
    }

    public void HideLevelUp()
    {
        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;

        Debug.Log("Level up panel closed");
    }
}