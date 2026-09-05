using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 30f;
    public float damage = 25f;
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
        Debug.Log("Shell hit: " + other.gameObject.name);

        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (enemy != null)
        {
            Debug.Log("Shell dealing " + damage + " damage.");
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}