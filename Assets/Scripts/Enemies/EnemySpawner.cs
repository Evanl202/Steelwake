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
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();

            spawnTimer = GetCurrentSpawnInterval();
        }

        if (waveAnnouncementUI != null)
        {
            waveAnnouncementUI.ShowWave(currentWave);
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

}