using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [Header ("Panel")]
    public GameObject levelUpPanel;
    
    [Header ("Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;

    [Header ("Button Text")]
    public TMP_Text button1Name;
    public TMP_Text button1Description;

    public TMP_Text button2Name;
    public TMP_Text button2Description;

    public TMP_Text button3Name;
    public TMP_Text button3Description;

    private UpgradeData[] upgrades =
    {
        new UpgradeData(
            "SPEED",
            "Increase maximum ship speed by 10%".
            0
        ),

        new UpgradeData(
            "DAMAGE",
            "Increase shell damage by 10%".
            1
        ),

        new UpgradeData(
            "RELOAD",
            "Reduce reload time by 10%".
            2
        ),

        new UpgradeData(
            "HEALTH",
            "Increase maximum health by 20".
            3
        ),
    };

    private UpgradeData choice1;
    private UpgradeData choice2;
    private UpgradeData choice3;

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

    private void GenerateChoices()
    {
        choice1 = upgrades[Random.Range(0, upgrades.Length)];

        do
        {
            choice2 = upgrades[Random.Range(0, upgrades.Length)];
        }
        while (choice2 == choice1);

        do
        {
            choice3 = upgrades[Random.Range(0, upgrades.Length)];
        }
        while (choice3 == choice1 || choice3 == choice2);

        DisplayUpgrade(
            choice1,
            button1Name,
            button1Description
        );

        DisplayUpgrade(
            choice2,
            button2Name,
            button2Description
        );

        DisplayUpgrade(
            choice3,
            button3Name,
            button3Description
        );
    }

    private void DisplayUpgrade(
        UpgradeData upgrade,
        TMP_Text nameText,
        TMP_Text descriptionText
    )
    {
        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;
    }

    public void SelectButton1()
    {
        ApplyUpgrade(choice1);
    }

    public void SelectButton2()
    {
        ApplyUpgrade(choice2);
    }

    public void SelectButton3()
    {
        ApplyUpgrade(choice3);
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        Debug.Log("Selected upgrade: " + upgrade.upgradeName);

        if (UpgradeManager.Instance == null)
        {
            Debug.LogWarning("UpgradeManager not found.");
            HideLevelUp();
            return;
        }

        switch (upgrade.upgradeType)
        {
            case 0:
                UpgradeManager.Instance.UpgradeSpeed();
                break;

            case 1:
                UpgradeManager.Instance.UpgradeDamage();
                break;

            case 2:
                UpgradeManager.Instance.UpgradeReload();
                break;

            case 3:
                UpgradeManager.Instance.UpgradeHealth();
                break;
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

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;
    public int upgradeType;

    public UpgradeData(
        string name,
        string descriptionText,
        int type
    )
    {
        upgradeName = name;
        description = descriptionText;
        upgradeType = type;
    }
}