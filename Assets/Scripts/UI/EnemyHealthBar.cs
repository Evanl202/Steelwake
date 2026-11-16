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
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
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

        // Hide the health bar when the enemy is behind the camera.
        bool visible = screenPosition.z > 0f;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

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