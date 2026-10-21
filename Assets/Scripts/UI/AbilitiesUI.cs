using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [Header("Ability Panels")]
    public GameObject smokePanel;
    public GameObject repairPanel;
    public GameObject repairPartyPanel;

    [Header("Smoke")]
    public TMP_Text smokeCooldownText;

    [Header("Repair")]
    public TMP_Text repairCooldownText;

    private ShipAbilities playerAbilities;

    [Header("Repair Party")]
    public TMP_Text repairPartyCooldownText;

    [Header("Fire Timer")]
    public GameObject firePanel;
    public TMP_Text fireTimerText;

    private ShipFire playerFire;

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

            playerFire = player.GetComponent<ShipFire>();
            
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
        UpdateRepairPartyUI();
        UpdateFireUI();
    }

    private void UpdateAbilityVisibility()
    {
        smokePanel.SetActive(
            playerAbilities.hasSmoke
        );

        repairPanel.SetActive(
            playerAbilities.hasRepair
        );

        repairPartyPanel.SetActive(
            playerAbilities.hasRepairParty
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
                "READY\n[E]";
        }
    }

    private void UpdateRepairPartyUI()
    {
        if (!playerAbilities.hasRepairParty)
            return;

        if (playerAbilities.GetRepairPartyCooldownRemaining() > 0f)
        {
            repairPartyCooldownText.text =
                "COOLDOWN\n" +
                playerAbilities.GetRepairPartyCooldownRemaining()
                    .ToString("F1") + "s";
        }
        else
        {
            repairPartyCooldownText.text = "READY\n[C]";
        }
    }

    private void UpdateFireUI()
    {
        bool burning = playerFire != null && playerFire.IsBurning;

        if (firePanel != null)
            firePanel.SetActive(burning);

        if (burning && fireTimerText != null)
        {
            fireTimerText.text =
                "ON FIRE: " +
                playerFire.FireTimeRemaining.ToString("F0") + "s";
        }
    }

}