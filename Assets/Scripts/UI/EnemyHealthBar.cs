
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public EnemyShip enemy;
    public Camera mainCamera;

    [Header("Position")]
    public Vector3 screenOffset = new Vector3(0f, 35f, 0f);

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(EnemyShip target, Camera cameraToUse)
    {
        enemy = target;
        mainCamera = cameraToUse;

        if (mainCamera == null)
            mainCamera = Camera.main;

        // Position immediately so it does not flash at the Canvas center.
        UpdateHealthBarPosition();
    }

    private void LateUpdate()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = enemy.maxHealth;
            healthSlider.value = enemy.currentHealth;
        }

        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        if (enemy == null || mainCamera == null)
            return;

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(enemy.transform.position);

        // Hide the bar when the enemy is behind the camera.
        bool visible = screenPosition.z > 0f;
        gameObject.SetActive(visible);

        if (!visible)
            return;

        if (rectTransform != null)
        {
            rectTransform.position = screenPosition + screenOffset;
        }
        else
        {
            transform.position = screenPosition + screenOffset;
        }
    }
}