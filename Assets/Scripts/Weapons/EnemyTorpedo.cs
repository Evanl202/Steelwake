using UnityEngine;

public class EnemyTorpedo : MonoBehaviour
{
    [Header ("Torpedo settings")]
    public float speed = 15f;
    public float damage = 50f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy torpedo hit: " + other.gameObject.name);

        PlayerShip player = other.GetComponent<PlayerShip>();

        if (player != null)
        {
            Debug.Log("Enemy torpedo dealing " + damage + " damage.");
            
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}