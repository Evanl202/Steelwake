
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        Repair
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
        }

        collected = true;

        Debug.Log("Collected pickup: " + pickupType);

        Destroy(gameObject);
    }
}
