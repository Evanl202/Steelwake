using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [Header ("Torpedo settings")]
    public float speed = 20f;
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

        EnemyShip enemy = other.GetComponent<EnemyShip>();

        if (enemy != null)
        {
            Debug.Log("Torpedo dealing " + damage + " damage.");
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}