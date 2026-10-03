using UnityEngine;

public class Shell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 30f;
    public float damage = 25f;
    public float maxDistance = 30f;

    private float distanceTravelled = 0f;

    [Header("Armor Penetration")]
    public bool isAP = false;
    public float penetration = 0f;

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
        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (enemy != null)
        {
            if (isAP && penetration < enemy.armor)
            {
                Debug.Log("AP shell failed to penetrate armor.");
                Destroy(gameObject);
                return;
            }

            Debug.Log("Shell dealing " + damage + " damage.");
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}