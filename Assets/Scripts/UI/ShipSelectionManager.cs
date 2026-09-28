using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShipSelectionManager : MonoBehaviour
{
    public GameObject[] shipPrefabs;

    public TMP_Text shipName;
    public TMP_Text shipStats;

    private int selectedShip = 0;

    private void Start()
    {
        UpdateShipDisplay();
    }

    public void NextShip()
    {
        selectedShip++;

        if (selectedShip >= shipPrefabs.Length)
            selectedShip = 0;

        UpdateShipDisplay();
    }

    public void PreviousShip()
    {
        selectedShip--;

        if (selectedShip < 0)
            selectedShip = shipPrefabs.Length - 1;

        UpdateShipDisplay();
    }

    public void SelectShip()
    {
        PlayerPrefs.SetInt("SelectedShip", selectedShip);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Main");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateShipDisplay()
    {
        if (selectedShip == 0)
        {
            shipName.text = "DESTROYER";
            shipStats.text =
                "HP: Medium\n" +
                "Speed: Fast\n" +
                "Armor: Low\n\n" +
                "Ability: Smoke";
        }
        else if (selectedShip == 1)
        {
            shipName.text = "CRUISER";
            shipStats.text =
                "HP: Medium-High\n" +
                "Speed: Medium\n" +
                "Armor: Medium\n\n" +
                "Abilities: Smoke + Repair";
        }
        else if (selectedShip == 2)
        {
            shipName.text = "BATTLESHIP";
            shipStats.text =
                "HP: High\n" +
                "Speed: Slow\n" +
                "Armor: High\n\n" +
                "Ability: Repair";
        }
    }
}