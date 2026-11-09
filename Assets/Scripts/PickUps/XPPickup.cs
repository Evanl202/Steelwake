using UnityEngine;

public class XPPickup : MonoBehaviour
{
    public int xpAmount = 10;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;
        
        PlayerShip player = other.GetComponent<PlayerShip>();

        if (player != null)
        {
            CollectXP();
        }
    }

    public void CollectXP(bool showNotification = true)
    {
        if (collected)
            return;
        
        collected = true;

        int gainedXP = xpAmount;
        
        if (ExperienceManager.Instance != null)
        {
            int gainedXP = ExperienceManager.Instance.AddXP(xpAmount);
        }
        
        if (GameAudioSFX.Instance != null)
        {
            GameAudioSFX.Instance.PlayXPPickup();
        }

        if (showNotification && PickupNotificationUI.Instance != null)
        {
            PickupNotificationUI.Instance.ShowNotification(
                "XP +" + gainedXP,
                "Experience gained"
            );
        }

        Debug.Log("Collected XP: " + xpAmount);
        Destroy(gameObject);
    }

    public static void CollectAllXP()
    {
        XPPickup[] pickups = Object.FindObjectsByType<XPPickup>(
            FindObjectsInactive.Exclude
        );

        Debug.Log($"XP Collector found {pickups.Length} XP pickups.");

        foreach (XPPickup pickup in pickups)
        {
            if (pickup != null)
            {
                pickup.CollectXP(false);
            }
        }
    }
}