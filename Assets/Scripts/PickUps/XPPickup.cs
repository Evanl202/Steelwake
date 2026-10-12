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

    public void CollectXP()
    {
        if (collected)
            return;
        
        collected = true;

        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.AddXP(xpAmount);
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
                pickup.CollectXP();
            }
        }
    }
}