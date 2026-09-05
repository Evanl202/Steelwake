using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Test levelText;
    public TMP_Test xpText;

    private void Update()
    {
        if (ExperienceManager != null)
            return;

        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        ExperienceManager manager = ExperienceManager.Instace;

        //Update bar
        xpSlider.maxValue = manager.xpToNextLevel;
        xpSlider.value = manager.currentXP;

        //Update level
        levelText.text = "LEVEL " + manager.currentLevel;

        //Update xp numbers
        xpText.text = manager.currentXP " / " + manager.xpToNextLevel " XP";

    }
}