using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 20f;
    public float damage = 10f;
    public float maxDistance = 32f;

    private float distanceTravelled = 0f;

    [Header("Armor Penetration")]
    public bool isAP = false;
    public float penetration = 0f;

    [Header("Fire Settings")]
    [Range(0f, 1f)]
    public float ignitionChance = 0.20f;

    private void Update()
    {
        float movement = speed * Time.deltaTime;

        transform.position += transform.forward * movement;

        distanceTravelled += movement;

        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerShip player = other.GetComponent<PlayerShip>();

        if (player != null)
        {
            // AP pen check
            if (isAP && penetration < player.armor)
            {
                Debug.Log("Enemy AP shell failed to penetrate armor.");
                
                if (CombatFeedbackManager.Instance != null)
                {
                    CombatFeedbackManager.Instance.ArmorBlockedFlash(
                        player.gameObject
                    );
                }

                Destroy(gameObject);
                return;
            }
            
            float finalDamage = damage;

            // Enemy HE shells are reduced by the player's armor.
            if (!isAP)
            {
                float damageMultiplier = Mathf.Max(
                    0.33f,
                    1f - player.armor / 200f
                );

                finalDamage *= damageMultiplier;
            }

            Debug.Log("Enemy shell hit player");
            Debug.Log("Enemy shell dealing " + finalDamage + " damage.");

            player.TakeDamage(finalDamage);
            
            if (!isAP)
            {
                ShipFire fire = player.GetComponent<ShipFire>();

                if (fire != null && fire.TryStartFire(false, ignitionChance))
                {
                    Debug.Log("Player ship caught fire!");
                }
            }

            Destroy(gameObject);
        }
    }
}