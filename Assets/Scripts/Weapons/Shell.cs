using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 30f;
    public float damage = 25f;
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
        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (isAP && penetration < enemy.armor)
        {
            Debug.Log("AP shell failed to penetrate armor.");
            Destroy(gameObject);
            return;
        }
        
        if (enemy != null)
        {
            Debug.Log("Shell dealing " + damage + " damage.");
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}