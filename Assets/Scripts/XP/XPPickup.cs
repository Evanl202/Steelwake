using UnityEgine;

public class XPPickup : MonoBehaviour
{
    public int xpAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        PlayerShip player = other.GetComponent<PlayerShip>();

        if (player != null)
        {
            if (ExperienceManager.Instance != null)
            {
                ExperienceManager.Instance.AddXP(xpAmount);
            }
            Debug.Log("Collected XP: " + xpAmount);
            Destroy(gameObject);
        }
    }
}