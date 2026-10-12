
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        Repair,
        DamageBoost,
        AttackSpeedBoost,
        ReloadBoost,
        XPCollector,
        Invulnerability
    }

    [Header("Pickup Settings")]
    public PickupType pickupType = PickupType.Repair;

    [Range(0f, 1f)]
    public float repairPercent = 0.30f;

    private bool collected;

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

            case PickupType.DamageBoost:
            case PickupType.AttackSpeedBoost:
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

                    case PickupType.AttackSpeedBoost:
                        effects.ActivateAttackSpeedBoost();
                        break;

                    case PickupType.ReloadBoost:
                        effects.ActivateReloadBoost();
                        break;

                    case PickupType.Invulnerability:
                        effects.ActivateInvulnerability();
                        break;

                    case PickupType.XPCollector:
                        XPPickup.CollectAllXP();
                        break;
                }
                break;
        }

        collected = true;
        Debug.Log("Collected pickup: " + pickupType);
        Destroy(gameObject);
    }
}