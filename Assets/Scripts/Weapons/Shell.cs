using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 30f;
    public float damage = 25f;
    public float maxDistance = 30f;

    private float distanceTravelled = 0f;

    [Header ("Armor Penetration")]
    public bool isAP = false;
    public float penetration = 0f;

    [Header ("Critical Hit")]
    [Range(0f, 1f)]
    public float criticalChance = 0f;

    [Min(1f)]
    public float criticalMultiplier  = 2f;

    [Header ("Fire Settings")]
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
        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (enemy != null)
        {
            // AP pen check
            if (isAP && penetration < enemy.armor)
            {
                Debug.Log("AP shell failed to penetrate armor.");

                if (CombatFeedbackManager.Instance != null)
                {
                    CombatFeedbackManager.Instance.ArmorBlockedFlash(
                        enemy.gameObject
                    );
                }
                
                Destroy(gameObject);
                return;
            }

            float finalDamage = damage;
            bool isCritical = false;

            if (isAP && Random.value < criticalChance)
            {
                finalDamage *= criticalMultiplier;
                isCritical = true;

                Debug.Log("AP CRITICAL HIT! Damage: " + finalDamage);
            }

            // HE shells are reduced by regular armor.
            if (!isAP)
            {
                float damageMultiplier = Mathf.Max(
                    0.33f,
                    1f - enemy.armor / 200f
                );

                finalDamage *= damageMultiplier;
            }

            Debug.Log("Shell dealing " + finalDamage + " damage.");
            
            enemy.TakeDamage(finalDamage);
            
            if (isCritical && CombatFeedbackManager.Instance != null)
            {
                CombatFeedbackManager.Instance.ShowCriticalHit(
                    finalDamage,
                    enemy.transform.position
                );
            }

            if (!isAP)
            {
                ShipFire fire = enemy.GetComponent<ShipFire>();

                if (fire != null && fire.TryStartFire(false, ignitionChance))
                {
                    Debug.Log("Enemy ship caught fire!");
                }
            }

            Destroy(gameObject);
        }

    }
}