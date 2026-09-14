using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject frigatePrefab;
    public GameObject destroyerPrefab;
    public GameObject cruiserPrefab;
    public GameObject battleshipPrefab;

    [Header("Player")]
    public Transform player;

    [Header("Spawning")]
    public float spawnDistance = 30f;
    public float spawnInterval = 3f;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
        {
            return;
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

        Vector3 spawnPosition = player.position +
            new Vector3(randomDirection.x, 0f, randomDirection.y) *
            spawnDistance;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private GameObject GetEnemyPrefab()
    {
        GameTimer timer = FindAnyObjectByType<GameTimer>();

        if (timer == null)
        {
            return frigatePrefab;
        }

        float time = timer.elapsedTime;

        // 0:00 - 0:20
        if (time < 20f)
        {
            return frigatePrefab;
        }

        // 0:20 - 0:30
        if (time < 30f)
        {
            return Random.value < 0.7f ? frigatePrefab : destroyerPrefab;
        }

        // 0:30 - 0:40
        if (time < 40f)
        {
            float roll = Random.value;

            if (roll < 0.5f)
                return frigatePrefab;

            if (roll < 0.8f)
                return destroyerPrefab;

            return cruiserPrefab;
        }

        // 0:40+
        float lateRoll = Random.value;

        if (lateRoll < 0.45f)
            return destroyerPrefab;

        if (lateRoll < 0.8f)
            return cruiserPrefab;

        return battleshipPrefab;
    }
}