using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [Header ("Torpedo settings")]
    public float speed = 20f;
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
        Debug.Log("Torpedo hit: " + other.gameObject.name);

        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (enemy != null)
        {
            Debug.Log("Torpedo dealing " + damage + " damage.");
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}