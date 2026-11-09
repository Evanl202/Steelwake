
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        Repair,
        DamageBoost,
        ReloadBoost,
        XPCollector,
        Invulnerability
    }

    [Header("Pickup Settings")]
    public PickupType pickupType = PickupType.Repair;

    [Range(0f, 1f)]
    public float repairPercent = 0.30f;

    private bool collected;
    
    private void ShowPickupNotification()
    {
        if (PickupNotificationUI.Instance == null)
            return;

        string title = "";
        string description = "";

        switch (pickupType)
        {
            case PickupType.Repair:
                title = "REPAIR KIT";
                description =
                    $"Restores {repairPercent * 100f:0}% of maximum health";
                break;

            case PickupType.DamageBoost:
                title = "DAMAGE BOOST";
                description =
                    "Increases damage by 50% for 10 seconds";
                break;

            case PickupType.ReloadBoost:
                title = "RELOAD BOOST";
                description =
                    "Reduces reload time by 40% for 10 seconds";
                break;

            case PickupType.XPCollector:
                title = "XP COLLECTOR";
                description =
                    "Collects all XP pickups on the map";
                break;

            case PickupType.Invulnerability:
                title = "INVULNERABILITY";
                description =
                    "Prevents damage for 5 seconds";
                break;
        }

        PickupNotificationUI.Instance.ShowNotification(
            title,
            description
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerShip player = other.GetComponentInParent<PlayerShip>();

        if (player == null)
            return;

        switch (pickupType)
        {
            case PickupType.Repair:
                player.RepairPercent(repairPercent);
                break;

            case PickupType.XPCollector:
                Debug.Log("XP Collector activated!");
                XPPickup.CollectAllXP();
                break;

            case PickupType.DamageBoost:
            case PickupType.ReloadBoost:
            case PickupType.Invulnerability:
                PickupEffects effects =
                    player.GetComponent<PickupEffects>();

                if (effects == null)
                {
                    Debug.LogWarning(
                        "Player is missing PickupEffects."
                    );
                    return;
                }

                switch (pickupType)
                {
                    case PickupType.DamageBoost:
                        effects.ActivateDamageBoost();
                        break;

                    case PickupType.ReloadBoost:
                        effects.ActivateReloadBoost();
                        break;

                    case PickupType.Invulnerability:
                        effects.ActivateInvulnerability();
                        break;
                }
                break;
        }

        collected = true;

        if (GameAudioSFX.Instance != null)
        {
            GameAudioSFX.Instance.PlayPickup();
        }

        ShowPickupNotification();

        Debug.Log("Collected pickup: " + pickupType);
        Destroy(gameObject);
    }
}