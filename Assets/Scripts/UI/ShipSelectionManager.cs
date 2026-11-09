using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShipSelectionManager : MonoBehaviour
{
    public GameObject[] shipPrefabs;

    public TMP_Text shipName;
    public TMP_Text shipStats;

    public Transform previewPoint;

    private GameObject previewShip;

    private int selectedShip = 0;

    [Header("Preview Controls")]
    public Camera previewCamera;

    public float rotationSpeed = 150f;

    public float zoomSpeed = 5f;
    public float minZoom = 25f;
    public float maxZoom = 60f;

    private float currentZoom = 45f;
    private bool isDragging = false;

    private void Start()
    {
        UpdateShipDisplay();
    }

    private void Update()
    {
        if (previewShip == null)
            return;

        // Start rotating when the left mouse button is pressed.
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = !IsPointerOverInteractiveUI();
        }

        // Stop rotating when the mouse button is released.
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Rotate the ship horizontally.
        if (isDragging && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");

            previewShip.transform.Rotate(
                Vector3.up,
                -mouseX * rotationSpeed * Time.deltaTime,
                Space.World
            );
        }

        // Zoom using the mouse wheel.
        if (previewCamera != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            currentZoom -= scroll * zoomSpeed * 100f * Time.deltaTime;

            currentZoom = Mathf.Clamp(
                currentZoom,
                minZoom,
                maxZoom
            );

            previewCamera.fieldOfView = currentZoom;
        }
    }

    private bool IsPointerOverInteractiveUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData =
            new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Selectable>() != null)
                return true;
        }

        return false;
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
        if (previewShip != null)
        {
            Destroy(previewShip);
        }

        previewShip = Instantiate(
            shipPrefabs[selectedShip],
            previewPoint.position,
            previewPoint.rotation
        );

        MonoBehaviour[] scripts =
            previewShip.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

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