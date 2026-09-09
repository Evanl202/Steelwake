using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 20f;
    public float damage = 10f;
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
        PlayerShip player = other.GetComponent<PlayerShip>();

        if (player != null)
        {
            Debug.Log("Enemy shell hit player");
            Debug.Log("Enemy shell dealing " + damage + " damage.");
            
            player.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
}