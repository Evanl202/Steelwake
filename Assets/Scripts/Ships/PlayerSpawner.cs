using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] shipPrefabs;
    public Transform spawnPoint;

    private void Start()
    {
        int selectedShip = PlayerPrefs.GetInt("SelectedShip", 0);

        if (selectedShip < 0 || selectedShip >= shipPrefabs.Length)
        {
            selectedShip = 0;
        }

        Instantiate(
            shipPrefabs[selectedShip],
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}