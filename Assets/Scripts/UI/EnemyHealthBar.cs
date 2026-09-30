using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public EnemyShip enemy;
    public Camera mainCamera;

    public Vector3 screenOffset = new Vector3(0f, 35f, 0f);

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        if (healthSlider == null || mainCamera == null)
            return;

        healthSlider.maxValue = enemy.maxHealth;
        healthSlider.value = enemy.currentHealth;

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(enemy.transform.position);

        transform.position =
            screenPosition + screenOffset;
    }
}