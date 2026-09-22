using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header ("Enemy")]
    public GameObject frigatePrefab;
    public GameObject destroyerPrefab;
    public GameObject cruiserPrefab;
    public GameObject battleshipPrefab;

    [Header ("Player")]
    public Transform player;

    [Header ("Spawning")]
    public float spawnDistance = 45f;
    public float spawnInterval = 4f;

    private float spawnTimer;
    private float waveTimer;

    [Header("Waves")]
    public int currentWave = 1;
    public float waveDuration = 60f;

    public float spawnIntervalReductionPerWave = 0.25f;
    public float minimumSpawnInterval = 0.5f;

    public int startingEnemyLimit = 5;
    public int extraEnemiesPerWave = 2;
    public int maximumEnemyLimit = 25;

    [Header("Wave Scaling")]
    public float enemyHealthIncreasePerWave = 0.10f;
    public float enemyDamageIncreasePerWave = 0.10f;
    public float enemySpeedIncreasePerWave = 0.05f;

    public float maximumEnemyHealthMultiplier = 2f;
    public float maximumEnemyDamageMultiplier = 2f;
    public float maximumEnemySpeedMultiplier = 1.5f;

    public WaveAnnouncementUI waveAnnouncementUI;

    [Header ("Elite Enemies")]
    [Range(0f, 1f)]
    public float eliteChance = 0.10f;

    private void Start()
    {
        spawnTimer = spawnInterval;
        waveTimer = waveDuration;

        if (waveAnnouncementUI != null)
        {
            waveAnnouncementUI.ShowWave(currentWave);
        }
    }

    private int GetEnemyLimit()
    {
        int limit =
            startingEnemyLimit +
            (currentWave - 1) * extraEnemiesPerWave;

        return Mathf.Min(limit, maximumEnemyLimit);
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
        {
            return;
        }
        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0f)
        {
            currentWave++;
            waveTimer = waveDuration;
            spawnTimer = GetCurrentSpawnInterval();

            Debug.Log("Wave " + currentWave + " started!");

            if (waveAnnouncementUI != null)
            {
                waveAnnouncementUI.ShowWave(currentWave);
            }
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();

            spawnTimer = GetCurrentSpawnInterval();
        }
    }

    private void SpawnEnemy()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length >= GetEnemyLimit())
        {
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("EnemySpawner missing player reference");
            return;
        }

        GameObject enemyPrefab = GetEnemyPrefab();

        if (enemyPrefab == null)
        {
            Debug.LogWarning("No enemy prefab available for current phase");
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnPosition =
            player.position + 
            new Vector3(randomDirection.x, 0f, randomDirection.y) * spawnDistance;

        Vector3 directionToPlayer =
            player.position - spawnPosition;

        directionToPlayer.y = 0f;

        Quaternion spawnRotation =
            Quaternion.LookRotation(directionToPlayer);

        GameObject spawnedEnemy = Instantiate (
            enemyPrefab,
            spawnPosition, 
            spawnRotation
        );

        EnemyShip enemyShip = spawnedEnemy.GetComponent<EnemyShip>();

        if (enemyShip != null)
        {
            enemyShip.isElite = Random.value < eliteChance;
        
            float healthMultiplier;
            float damageMultiplier;
            float speedMultiplier;

            GetEnemyMultipliers(
                out healthMultiplier,
                out damageMultiplier,
                out speedMultiplier
            );

            enemyShip.maxHealth *= healthMultiplier;

            enemyShip.maxSpeed *= speedMultiplier;
            enemyShip.acceleration *= speedMultiplier;
            enemyShip.deceleration *= speedMultiplier;
            enemyShip.reverseSpeed *= speedMultiplier;

            EnemyWeapon enemyWeapon =
                spawnedEnemy.GetComponentInChildren<EnemyWeapon>();

            if (enemyWeapon != null)
            {
                enemyWeapon.gunDamage *= damageMultiplier;
                enemyWeapon.torpedoDamage *= damageMultiplier;
            }
        }
    }

    private GameObject GetEnemyPrefab()
    {
        float roll = Random.value;

        switch (currentWave)
        {
            // Wave 1: Frigates only
            case 1:
                return frigatePrefab;

            // Wave 2: Frigates + Destroyers
            case 2:
                return roll < 0.7f
                    ? frigatePrefab
                    : destroyerPrefab;

            // Wave 3: Destroyers + Frigates
            case 3:
                return roll < 0.6f
                    ? destroyerPrefab
                    : frigatePrefab;

            // Wave 4: Destroyers + Cruisers
            case 4:
                return roll < 0.6f
                    ? destroyerPrefab
                    : cruiserPrefab;

            // Wave 5+: Destroyers + Cruisers + Battleships
            default:
                if (roll < 0.45f)
                    return destroyerPrefab;

                if (roll < 0.80f)
                    return cruiserPrefab;

                return battleshipPrefab;
        }
    }

    private float GetCurrentSpawnInterval()
    {
        float interval =
            spawnInterval -
            (currentWave - 1) * spawnIntervalReductionPerWave;

        return Mathf.Max(interval, minimumSpawnInterval);
    }

    private void GetEnemyMultipliers(
        out float healthMultiplier,
        out float damageMultiplier,
        out float speedMultiplier)
    {
        healthMultiplier = Mathf.Min(
            1f + (currentWave - 1) * enemyHealthIncreasePerWave,
            maximumEnemyHealthMultiplier
        );

        damageMultiplier = Mathf.Min(
            1f + (currentWave - 1) * enemyDamageIncreasePerWave,
            maximumEnemyDamageMultiplier
        );

        speedMultiplier = Mathf.Min(
            1f + (currentWave - 1) * enemySpeedIncreasePerWave,
            maximumEnemySpeedMultiplier
        );
    }

}