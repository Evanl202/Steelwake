using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Text levelText;
    public TMP_Text xpText;

    private void Update()
    {
        if (ExperienceManager.Instance == null)
            return;

        UpdateXPBar();
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