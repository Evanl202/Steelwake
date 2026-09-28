using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [Header("Ability Panels")]
    public GameObject smokePanel;
    public GameObject repairPanel;

    [Header("Smoke")]
    public TMP_Text smokeCooldownText;

    [Header("Repair")]
    public TMP_Text repairCooldownText;

    private ShipAbilities playerAbilities;

    private void Start()
    {
        Invoke(nameof(FindPlayer), 0.1f);
    }

    private void FindPlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerAbilities =
                player.GetComponent<ShipAbilities>();

            UpdateAbilityVisibility();
        }
    }

    private void Update()
    {
        if (playerAbilities == null)
            return;

        UpdateAbilityVisibility();
        UpdateSmokeUI();
        UpdateRepairUI();
    }

    private void UpdateAbilityVisibility()
    {
        smokePanel.SetActive(
            playerAbilities.hasSmoke
        );

        repairPanel.SetActive(
            playerAbilities.hasRepair
        );
    }

    private void UpdateSmokeUI()
    {
        if (!playerAbilities.hasSmoke)
            return;

        if (playerAbilities.IsInSmoke)
        {
            smokeCooldownText.text =
                "ACTIVE\n" +
                playerAbilities.GetSmokeDurationRemaining()
                    .ToString("F1") + "s";
        }
        else if (playerAbilities.GetSmokeCooldownRemaining() > 0f)
        {
            smokeCooldownText.text =
                "COOLDOWN\n" +
                playerAbilities.GetSmokeCooldownRemaining()
                    .ToString("F1") + "s";
        }
        else
        {
            smokeCooldownText.text =
                "READY\n[F]";
        }
    }

    private void UpdateRepairUI()
    {
        if (!playerAbilities.hasRepair)
            return;

        if (playerAbilities.GetRepairCooldownRemaining() > 0f)
        {
            repairCooldownText.text =
                "COOLDOWN\n" +
                playerAbilities.GetRepairCooldownRemaining()
                    .ToString("F1") + "s";
        }
        else
        {
            repairCooldownText.text =
                "READY\n[R]";
        }
    }
}