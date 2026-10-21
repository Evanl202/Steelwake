using UnityEngine;

public class EnemyTorpedo : MonoBehaviour
{
    [Header ("Torpedo settings")]
    public float speed = 15f;
    public float damage = 50f;
    public float maxDistance = 35f;
    
    private float distanceTravelled = 0f;

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
            float protection = Mathf.Clamp(
                player.torpedoProtection,
                0f,
                0.5f
            );

            float finalDamage = damage * (1f - protection);

            Debug.Log(
                "Enemy torpedo dealing " + finalDamage +
                " damage after " + (protection * 100f) +
                "% protection."
            );

            player.TakeDamage(finalDamage);
            Destroy(gameObject);
        }
    }
}