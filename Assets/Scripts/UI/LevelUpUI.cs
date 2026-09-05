using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel();

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

    public void HideLevelUp()
    {
        levelUpPanel.SetActive(false);

        Time.timeScale = 1f;

        Debug.Log("Level up panel closed");
    }
}