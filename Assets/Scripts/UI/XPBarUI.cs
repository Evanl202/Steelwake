using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Text levelText;
    public TMP_Text xpText;

    private int lastCurrentXP = -1;
    private int lastXPToNextLevel = -1;
    private int lastCurrentLevel = -1;

    private void Update()
    {
        if (ExperienceManager.Instance == null)
            return;

        ExperienceManager manager = ExperienceManager.Instance;

        if (manager.currentXP == lastCurrentXP &&
            manager.xpToNextLevel == lastXPToNextLevel &&
            manager.currentLevel == lastCurrentLevel)
        {
            return;
        }

        UpdateXPBar();

        lastCurrentXP = manager.currentXP;
        lastXPToNextLevel = manager.xpToNextLevel;
        lastCurrentLevel = manager.currentLevel;
    }

    private void UpdateXPBar()
    {
        ExperienceManager manager = ExperienceManager.Instance;

        //Update bar
        xpSlider.maxValue = manager.xpToNextLevel;
        xpSlider.value = manager.currentXP;

        //Update level
        levelText.text = "LEVEL " + manager.currentLevel;

        //Update xp numbers
        xpText.text = manager.currentXP + " / " + manager.xpToNextLevel + " XP";

    }
}