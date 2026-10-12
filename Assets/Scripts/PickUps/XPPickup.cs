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
        XPPickup[] pickups = FindObjectsByType<XPPickup>(
            FindObjectsSortMode.None
        );

        foreach (XPPickup pickup in pickups)
        {
            if (pickup != null)
            {
                pickup.CollectXP();
            }
        }

        Debug.Log("XP Collector collected " + pickups.Length + " XP pickups.");
    }
}