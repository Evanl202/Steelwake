using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 50f;

    private float currentHealth;

    [Header ("Experience")]
    public int experienceReward = 10;
    public GameObject xpPickupPrefab;
    
    [Header ("Movement")]
    public float moveSpeed = 3f;
    public float preferredDistance = 25f;
    public float minimumDistance = 10f;
    public float orbitSpeed = 1f;

    protected Transform player;

    private float orbitDirection;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Enemy can't find Player");
        }

        //clockwise or counterclockwise
        orbitDirection = Random.value < 0.5f ? -1f : 1f;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }
            
        MoveAroundPlayer();
    }

    private void MoveAroundPlayer()
    {
        if (player == null)
            return;
        
        Vector3 toPlayer = player.position - transform.position;

        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (toPlayer.sqrMagnitude < 0.1f)
            return;

        Vector3 directionToPlayer = toPlayer.normalized;

        //Surround and Orbit
        Vector3 orbitDirectionVector = new Vector3(
            -directionToPlayer.z, 0f, directionToPlayer.x);

        orbitDirectionVector *= orbitDirection;

        Vector3 movementDirection;

        if (distance > preferredDistance)
        {
            //Too far: Move toward player while orbit
            movementDirection = directionToPlayer + orbitDirectionVector * orbitSpeed;
        }
        else if (distance < minimumDistance)
        {
            //Too close: Move away while orbiting
            movementDirection = -directionToPlayer + orbitDirectionVector * orbitSpeed;
        }

        else
        {
            //Maintain distance
            movementDirection = orbitDirectionVector * orbitSpeed;
        }

        movementDirection.y = 0f;

        if (movementDirection.sqrMagnitude > 0.01f)
        {
            movementDirection.Normalize();

            transform.position +=
                movementDirection * moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Destroyed");

        if (xpPickupPrefab != null)
        {
            Debug.Log("Enemey dropped XP");
            GameObject pickup = Instantiate(
                xpPickupPrefab,
                transform.position,
                Quaternion.identity
            );

            XPPickup xp = pickup.GetComponent<XPPickup>();

            if (xp != null)
            {
                xp.xpAmount = experienceReward;
            }
        else
        {
            Debug.LogWarning(
                "Enemyship has no XP Prefab assigned"
            );
        }
            
        }
        Destroy(gameObject);
    }
}