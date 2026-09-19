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
    public float spawnInterval = 3f;

    private float spawnTimer;
    private float waveTimer;

    [Header("Waves")]
    public int currentWave = 1;
    public float waveDuration = 60f;

    [Header ("Elite Enemies")]
    [Range(0f, 1f)]
    public float eliteChance = 0.10f;

    private void Start()
    {
        spawnTimer = spawnInterval;
        waveTimer = waveDuration;
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

            Debug.Log("Wave " + currentWave + " started!");
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();

            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
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

        GameObject spawnedEnemy = Instantiate (
            enemyPrefab,
            spawnPosition, 
            Quaternion.identity
        );

        EnemyShip enemyShip = spawnedEnemy.GetComponent<EnemyShip>();

        if (enemyShip != null)
        {
            enemyShip.isElite = Random.value < eliteChance;
        }
    }

    private GameObject GetEnemyPrefab()
    {
        GameTimer timer = FindAnyObjectByType<GameTimer>();

        if (timer == null)
        {
            return frigatePrefab;
        }

        float time = timer.elapsedTime;

        //0:00 - 2:00 120f
        if (time < 10f)
        {
            return frigatePrefab;
        }

        //2:00 - 5:00 300f
        if (time < 30)
        {
            return Random.value < 0.7f ? frigatePrefab : destroyerPrefab;
        }

        //8:00 - 12:00 720f
        if (time < 40)
        {
            float roll = Random.value;

            if (roll < 0.5f)
                return frigatePrefab;

            if (roll < 0.8f)
                return destroyerPrefab;

            return cruiserPrefab;
        }
        //12:00+
        float lateRoll = Random.value;

        if (lateRoll < 0.45f)
            return destroyerPrefab;

        if (lateRoll < 0.8f)
            return cruiserPrefab;
                
        return battleshipPrefab;
        
    }
}