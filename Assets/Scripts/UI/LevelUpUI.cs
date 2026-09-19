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
        // SPEED
        new UpgradeData(
            "SPEED +FLAT",
            "Increase maximum ship speed by 1",
            0
        ),

        new UpgradeData(
            "SPEED +%",
            "Increase maximum ship speed by 10%",
            1
        ),

        // UNIVERSAL DAMAGE
        new UpgradeData(
            "DAMAGE +FLAT",
            "Increase HE, AP, and torpedo damage by 10",
            2
        ),

        new UpgradeData(
            "DAMAGE +%",
            "Increase HE, AP, and torpedo damage by 10%",
            3
        ),

        // UNIVERSAL RELOAD
        new UpgradeData(
            "RELOAD +FLAT",
            "Reduce main battery and torpedo reload time by 0.05 seconds",
            4
        ),

        new UpgradeData(
            "RELOAD +%",
            "Reduce main battery and torpedo reload time by 5%",
            5
        ),

        // HEALTH
        new UpgradeData(
            "HEALTH +FLAT",
            "Increase maximum health by 20",
            6
        ),

        new UpgradeData(
            "HEALTH +%",
            "Increase maximum health by 10%",
            7
        ),

        // HE DAMAGE
        new UpgradeData(
            "HE DAMAGE +FLAT",
            "Increase HE shell damage by 10",
            8
        ),

        new UpgradeData(
            "HE DAMAGE +%",
            "Increase HE shell damage by 10%",
            9
        ),

        // AP DAMAGE
        new UpgradeData(
            "AP DAMAGE +FLAT",
            "Increase AP shell damage by 10",
            10
        ),

        new UpgradeData(
            "AP DAMAGE +%",
            "Increase AP shell damage by 10%",
            11
        ),

        // TORPEDO DAMAGE
        new UpgradeData(
            "TORPEDO DAMAGE +FLAT",
            "Increase torpedo damage by 10",
            12
        ),

        new UpgradeData(
            "TORPEDO DAMAGE +%",
            "Increase torpedo damage by 10%",
            13
        ),

        // AP PENETRATION
        new UpgradeData(
            "AP PENETRATION +FLAT",
            "Increase AP penetration by 5",
            14
        ),

        new UpgradeData(
            "AP PENETRATION +%",
            "Increase AP penetration by 10%",
            15
        ),

        // ARMOR
        new UpgradeData(
            "ARMOR +FLAT",
            "Increase armor by 5",
            16
        ),

        new UpgradeData(
            "ARMOR +%",
            "Increase armor by 10%",
            17
        ),

        // MAIN BATTERY RELOAD
        new UpgradeData(
            "MAIN RELOAD +FLAT",
            "Reduce main battery reload time by 0.05 seconds",
            18
        ),

        new UpgradeData(
            "MAIN RELOAD +%",
            "Reduce main battery reload time by 5%",
            19
        ),

        // TORPEDO RELOAD
        new UpgradeData(
            "TORPEDO RELOAD +FLAT",
            "Reduce torpedo reload time by 0.05 seconds",
            20
        ),

        new UpgradeData(
            "TORPEDO RELOAD +%",
            "Reduce torpedo reload time by 5%",
            21
        )
    };

    private UpgradeData choice1;
    private UpgradeData choice2;
    private UpgradeData choice3;

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowLevelUp()
    {
        GenerateChoices();

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
            // SPEED
            case 0:
                UpgradeManager.Instance.UpgradeSpeedFlat();
                break;

            case 1:
                UpgradeManager.Instance.UpgradeSpeedPercent();
                break;

            // UNIVERSAL DAMAGE
            case 2:
                UpgradeManager.Instance.UpgradeDamageFlat();
                break;

            case 3:
                UpgradeManager.Instance.UpgradeDamagePercent();
                break;

            // UNIVERSAL RELOAD
            case 4:
                UpgradeManager.Instance.UpgradeReloadFlat();
                break;

            case 5:
                UpgradeManager.Instance.UpgradeReloadPercent();
                break;

            // HEALTH
            case 6:
                UpgradeManager.Instance.UpgradeHealthFlat();
                break;

            case 7:
                UpgradeManager.Instance.UpgradeHealthPercent();
                break;

            // HE DAMAGE
            case 8:
                UpgradeManager.Instance.UpgradeHEDamageFlat();
                break;

            case 9:
                UpgradeManager.Instance.UpgradeHEDamagePercent();
                break;

            // AP DAMAGE
            case 10:
                UpgradeManager.Instance.UpgradeAPDamageFlat();
                break;

            case 11:
                UpgradeManager.Instance.UpgradeAPDamagePercent();
                break;

            // TORPEDO DAMAGE
            case 12:
                UpgradeManager.Instance.UpgradeTorpedoDamageFlat();
                break;

            case 13:
                UpgradeManager.Instance.UpgradeTorpedoDamagePercent();
                break;

            // AP PENETRATION
            case 14:
                UpgradeManager.Instance.UpgradeAPPenetrationFlat();
                break;

            case 15:
                UpgradeManager.Instance.UpgradeAPPenetrationPercent();
                break;

            // ARMOR
            case 16:
                UpgradeManager.Instance.UpgradeArmorFlat();
                break;

            case 17:
                UpgradeManager.Instance.UpgradeArmorPercent();
                break;

            // MAIN BATTERY RELOAD
            case 18:
                UpgradeManager.Instance.UpgradeWeaponReloadFlat();
                break;

            case 19:
                UpgradeManager.Instance.UpgradeWeaponReloadPercent();
                break;

            // TORPEDO RELOAD
            case 20:
                UpgradeManager.Instance.UpgradeTorpedoReloadFlat();
                break;

            case 21:
                UpgradeManager.Instance.UpgradeTorpedoReloadPercent();
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