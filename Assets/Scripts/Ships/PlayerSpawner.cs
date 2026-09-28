using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] shipPrefabs;
    public Transform spawnPoint;

    public CameraFollow cameraFollow;
    public EnemySpawner enemySpawner;

    private void Start()
    {
        int selectedShip = PlayerPrefs.GetInt("SelectedShip", 0);

        if (selectedShip < 0 || selectedShip >= shipPrefabs.Length)
        {
            selectedShip = 0;
        }

        GameObject spawnedPlayer = Instantiate(
            shipPrefabs[selectedShip],
            spawnPoint.position,
            spawnPoint.rotation
        );

        if (cameraFollow != null)
        {
            cameraFollow.target = spawnedPlayer.transform;
        }

        if (enemySpawner != null)
        {
            enemySpawner.player = spawnedPlayer.transform;
        }
    }
}