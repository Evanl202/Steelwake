using UnityEngine;

public class EnemyShell : MonoBehaviour
{
    [Header ("Shell settings")]
    public float speed = 20f;
    public float damage = 10f;
    public float maxDistance = 32f;

    private float distanceTravelled = 0f;

    [Header("Armor Penetration")]
    public bool isAP = false;
    public float penetration = 0f;

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
            if (isAP && penetration < player.armor)
            {
                Debug.Log("Enemy AP shell failed to penetrate armor.");

                player.ArmorBlockedFlash();
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