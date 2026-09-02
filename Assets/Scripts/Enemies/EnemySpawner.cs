using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header ("Enemy")]
    public GameObject enemyPrefab;

    [Header ("Player")]
    public Transform player;

    [Header ("Spawning")]
    public float spawnDistance = 30f;
    public float spawnInterval = 3f;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();

            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
        {
            Debug.LogWarning("EnemySpawner missing reference");
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnPosition = player.position + new Vector3(randomDirection.x, 0f, randomDirection.y) * spawnDistance;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

}