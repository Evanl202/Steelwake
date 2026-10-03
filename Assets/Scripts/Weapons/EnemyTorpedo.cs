using UnityEngine;

public class EnemyTorpedo : MonoBehaviour
{
    [Header ("Torpedo settings")]
    public float speed = 15f;
    public float damage = 50f;
    public float maxDistance = 35f;

    private float distanceTravelled = 0f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

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
            Debug.Log("Enemy torpedo hit player");
            Debug.Log("Enemy torpedo dealing " + damage + " damage.");
            
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}