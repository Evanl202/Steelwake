using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;

    [Header("Armor Penetration")]
    public bool isAP = false;
    public float penetration = 0f;

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
            if (isAP && penetration < player.armor)
            {
                Debug.Log("Enemy AP shell failed to penetrate armor.");
                Destroy(gameObject);
                return;
            }
            
            Debug.Log("Enemy shell hit player");
            Debug.Log("Enemy shell dealing " + damage + " damage.");
            
            player.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
}