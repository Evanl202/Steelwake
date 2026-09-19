using UnityEngine;

public enum AIBehavior
{
    Aggressive,
    Flanker,
    Balanced,
    Defensive
}

public class EnemyShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 50f;

    private float currentHealth;

    [Header ("Armor")]
    public float armor = 0f;

    [Header ("Elite")]
    public bool isElite = false;
    public GameObject eliteGlow;

    [Header ("Experience")]
    public int experienceReward = 10;
    public GameObject xpPickupPrefab;
    
    [Header ("Movement")]
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 4f;
    public float reverseSpeed = 5f;
    public float turnSpeed = 60f;

    [Header ("AI Behavior")]
    public AIBehavior aiBehavior = AIBehavior.Balanced;

    [Header ("AI Distance")]
    public float preferredDistance = 25f;
    public float minimumDistance = 10f;
    public float orbitSpeed = 1f;

    [Header("AI Weapon Awareness")]
    private EnemyWeapon enemyWeapon;
    public float weaponRangeBuffer = 0.9f;

    [Header ("Spacing")]
    public float enemySpacing = 8f;
    public float spacingStrength = 2f;

    protected Transform player;

    private float orbitDirection;
    private float currentSpeed = 0f;

    private void Start()
    {
        if (isElite)
        {
            maxHealth *= 1.5f;
            armor *= 1.25f;

            maxSpeed *= 1.15f;
            acceleration *= 1.15f;
            deceleration *= 1.15f;
            reverseSpeed *= 1.15f;
            turnSpeed *= 1.15f;

            experienceReward *= 2;
            
            if (eliteGlow != null)
            {
                eliteGlow.SetActive(true);
            }
        }

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

        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
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
        float combatDistance = GetCombatDistance();

        if (toPlayer.sqrMagnitude < 0.1f)
            return;

        Vector3 directionToPlayer = toPlayer.normalized;

        //Surround and Orbit
        Vector3 orbitDirectionVector = new Vector3(
            -directionToPlayer.z, 0f, directionToPlayer.x);

        orbitDirectionVector *= orbitDirection;

        Vector3 movementDirection = Vector3.zero;

        switch (aiBehavior)
        {
            case AIBehavior.Aggressive:

                // Stay close and constantly pressure the player
                if (distance > combatDistance)
                {
                    movementDirection =
                        directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else
                {
                    movementDirection =
                        directionToPlayer * 0.5f +
                        orbitDirectionVector * orbitSpeed * 1.5f;
                }

                break;


            case AIBehavior.Flanker:

                // Try to circle around the player
                if (distance > combatDistance)
                {
                    movementDirection =
                        directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else if (distance < minimumDistance)
                {
                    movementDirection =
                        -directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else
                {
                    movementDirection =
                        orbitDirectionVector * orbitSpeed * 1.5f;
                }

                break;


            case AIBehavior.Balanced:

                // Normal current behavior
                if (distance > combatDistance)
                {
                    movementDirection =
                        directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else if (distance < minimumDistance)
                {
                    movementDirection =
                        -directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else
                {
                    movementDirection =
                        orbitDirectionVector * orbitSpeed;
                }

                break;


            case AIBehavior.Defensive:

                // Stay farther away and avoid closing in
                if (distance < minimumDistance)
                {
                    movementDirection =
                        -directionToPlayer +
                        orbitDirectionVector * orbitSpeed;
                }
                else if (distance > combatDistance)
                {
                    movementDirection =
                        directionToPlayer * 0.5f +
                        orbitDirectionVector * orbitSpeed * 0.5f;
                }
                else
                {
                    movementDirection =
                        orbitDirectionVector * orbitSpeed * 0.5f;
                }

                break;
        }

        //Spacing
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject)
                continue;
            
            Vector3 awayFromEnemy = transform.position - enemy.transform.position;

            awayFromEnemy.y = 0f;

            float enemyDistance = awayFromEnemy.magnitude;

            if (enemyDistance < enemySpacing && enemyDistance > 0.01f)
            {
                float strength = 1f - (enemyDistance / enemySpacing);

                movementDirection +=
                    awayFromEnemy.normalized * strength * spacingStrength;
            }
        }

        movementDirection.y = 0f;

        if (movementDirection.sqrMagnitude > 0.01f)
        {
            movementDirection.Normalize();

            //Turn toward movement direction
            Quaternion targetRotation =
                Quaternion.LookRotation(movementDirection);
            
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );

            //Determine direction
            float directionDot = 
                Vector3.Dot(transform.forward, movementDirection);

            //Accel
            if (directionDot > 0f)
            {
                currentSpeed += acceleration * Time.deltaTime;

                currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            }
            //Reverse
            else
            {
                currentSpeed -= acceleration * Time.deltaTime;

                currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, maxSpeed);
            }
        }
        //Slow down if no movement choice
        else
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }

        //Move
        transform.position += 
            transform.forward * currentSpeed * Time.deltaTime;
        
    }

    private float GetCombatDistance()
    {
        if (enemyWeapon == null)
            return preferredDistance;

        if (enemyWeapon.gunFiringRange <= 0f)
            return preferredDistance;

        return enemyWeapon.gunFiringRange * weaponRangeBuffer;
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
                    "Enemyship XP Pickup prefab has no XPPickup component"
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "Enemyship has no XP Prefab assigned"
            );
        }

        Destroy(gameObject);
    }
}