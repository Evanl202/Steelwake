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
    [Header ("Ship Class")]
    public ShipClass shipClass = ShipClass.Destroyer;

    [Header ("Health")]
    public float maxHealth = 50f;

    public float currentHealth;

    [Header ("Armor")]
    public float armor = 0f;

    [Header ("Torpedo Protection")]
    [Range(0f, 0.5f)]
    public float torpedoProtection = 0.05f;

    [Header ("Elite")]
    public bool isElite = false;
    public Color eliteColor = Color.black;

    [Header ("Experience")]
    public int experienceReward = 10;
    public GameObject xpPickupPrefab;

    [Header ("Regular Pickup Drop")]
    public GameObject[] pickupPrefabs;

    [Range (0f, 1f)]
    public float pickupDropChance = 0.05f;
    
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
    public float maximumDistance = 60f;
    public float returnTurnSpeedMultiplier = 2f;

    [Header("AI Weapon Awareness")]
    private EnemyWeapon enemyWeapon;
    public float weaponRangeBuffer = 0.9f;

    [Header ("Spacing")]
    public float enemySpacing = 8f;
    public float spacingStrength = 1f;

    protected Transform player;

    private float orbitDirection;
    private float currentSpeed = 0f;
    public float CurrentSpeed => currentSpeed;

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

            if (CombatFeedbackManager.Instance != null)
            {
                CombatFeedbackManager.Instance.ApplyEliteVisual(
                    gameObject,
                    eliteColor
                );
            }
        }

        currentHealth = maxHealth;

        if (CombatFeedbackManager.Instance != null)
        {
            CombatFeedbackManager.Instance.CreateEnemyHealthBar(this);
        }

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

        if (toPlayer.sqrMagnitude < 0.1f)
            return;

        float distance = toPlayer.magnitude;
        float combatDistance = GetCombatDistance();

        Vector3 directionToPlayer = toPlayer.normalized;

        // Direction tangent to the player
        Vector3 orbitDirectionVector = new Vector3(
            -directionToPlayer.z,
            0f,
            directionToPlayer.x
        );

        orbitDirectionVector *= orbitDirection;

        Vector3 movementDirection;

        if (distance > maximumDistance)
        {
            movementDirection = directionToPlayer;
        }
        else
        {
            // Stay aware of the enemy's weapon firing arc
            bool canFireGun =
                enemyWeapon != null &&
                enemyWeapon.CanCurrentlyFireGun();

            if (!canFireGun && enemyWeapon != null)
            {
                float gunCorrection =
                    enemyWeapon.GetGunPositioningAngle();

                float correctionDirection =
                    Mathf.Sign(gunCorrection);

                Vector3 repositionDirection =
                    orbitDirectionVector * correctionDirection;

                movementDirection =
                    repositionDirection +
                    directionToPlayer * 0.25f;
            }
            else
            {
                switch (aiBehavior)
                {
                    case AIBehavior.Aggressive:

                        if (distance > combatDistance)
                        {
                            movementDirection =
                                directionToPlayer +
                                orbitDirectionVector;
                        }
                        else
                        {
                            movementDirection =
                                orbitDirectionVector;
                        }

                        break;


                    case AIBehavior.Flanker:

                        if (distance > combatDistance)
                        {
                            movementDirection =
                                directionToPlayer +
                                orbitDirectionVector;
                        }
                        else if (distance < minimumDistance)
                        {
                            movementDirection =
                                directionToPlayer +
                                orbitDirectionVector;
                        }
                        else
                        {
                            movementDirection =
                                orbitDirectionVector;
                        }

                        break;


                    case AIBehavior.Balanced:

                        if (distance < minimumDistance)
                        {
                            // Too close: move away while continuing to orbit.
                            movementDirection =
                                -directionToPlayer + orbitDirectionVector;
                        }
                        else if (distance > combatDistance)
                        {
                            // Too far: approach while continuing to orbit.
                            movementDirection =
                                directionToPlayer + orbitDirectionVector;
                        }
                        else
                        {
                            // At a suitable distance: circle the player.
                            movementDirection = orbitDirectionVector;
                        }

                        break;


                    case AIBehavior.Defensive:

                        if (distance < minimumDistance)
                        {
                            movementDirection =
                                -directionToPlayer +
                                orbitDirectionVector;
                        }
                        else if (distance > combatDistance)
                        {
                            movementDirection =
                                directionToPlayer +
                                orbitDirectionVector;
                        }
                        else
                        {
                            movementDirection =
                                orbitDirectionVector;
                        }

                        break;


                    default:

                        movementDirection =
                            orbitDirectionVector;

                        break;
                }
            }
        }

        // Enemy spacing
        Vector3 spacingDirection = Vector3.zero;

        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject)
                continue;

            Vector3 awayFromEnemy =
                transform.position - enemy.transform.position;

            awayFromEnemy.y = 0f;

            float enemyDistance =
                awayFromEnemy.magnitude;

            if (enemyDistance < enemySpacing &&
                enemyDistance > 0.01f)
            {
                float strength =
                    1f - (enemyDistance / enemySpacing);

                spacingDirection +=
                    awayFromEnemy.normalized * strength;
            }
        }

        if (spacingDirection.sqrMagnitude > 0.01f)
        {
            spacingDirection.Normalize();

            // Only allow spacing to affect the orbit direction.
            // It cannot push the ship farther away from the player.
            Vector3 spacingOrbitDirection =
                Vector3.ProjectOnPlane(
                    spacingDirection,
                    directionToPlayer
                );

            if (spacingOrbitDirection.sqrMagnitude > 0.01f)
            {
                movementDirection +=
                    spacingOrbitDirection.normalized * 0.35f;
            }
        }

        movementDirection.y = 0f;

        if (movementDirection.sqrMagnitude > 0.01f)
        {
            movementDirection.Normalize();
        }

        if (movementDirection.sqrMagnitude > 0.01f)
        {
            movementDirection.Normalize();

            // Continuously turn toward the desired sailing direction
            Quaternion targetRotation =
                Quaternion.LookRotation(movementDirection);

            float currentTurnSpeed =
                turnSpeed;

            // Turn back ship when too far
            if (distance > maximumDistance)
            {
                currentTurnSpeed *=
                    returnTurnSpeedMultiplier;
            }

            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    currentTurnSpeed * Time.deltaTime
                );
        }

        
        float targetSpeed = maxSpeed;

        // Slow down near  combat distance to reduce overshooting
        if (aiBehavior == AIBehavior.Balanced)
        {
            float distanceError = Mathf.Abs(distance - combatDistance);

            // Slow down within 20 units of combat distance
            float speedFactor = Mathf.Clamp01(distanceError / 20f);

            // Never drop below 35% speed
            targetSpeed = Mathf.Lerp(maxSpeed * 0.35f, maxSpeed, speedFactor);
        }

        float speedChange = targetSpeed > currentSpeed
            ? acceleration
            : deceleration;

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            speedChange * Time.deltaTime
        );

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

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

        if (GameAudioSFX.Instance != null)
        {
            GameAudioSFX.Instance.PlayEnemyHit();
        }

        if (CombatFeedbackManager.Instance != null)
        {
            CombatFeedbackManager.Instance.HitFlash(gameObject);

            CombatFeedbackManager.Instance.ShowDamage(
                damage,
                transform.position
            );
        }
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

        if (pickupPrefabs != null &&
            pickupPrefabs.Length > 0 &&
            Random.value < pickupDropChance)
        {
            GameObject selectedPickup = pickupPrefabs[
                Random.Range(0, pickupPrefabs.Length)
            ];

            if (selectedPickup != null)
            {
                Instantiate(
                    selectedPickup,
                    transform.position,
                    Quaternion.identity
                );

                Debug.Log(
                    "Enemy dropped pickup: " + selectedPickup.name
                );
            }
        }

        if (GameAudioSFX.Instance != null)
        {
            GameAudioSFX.Instance.PlayEnemyDestroyed();
        }
        
        Destroy(gameObject);
    }
}