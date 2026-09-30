using UnityEngine;

public class CombatFeedbackManager  : MonoBehaviour
{
    public static CombatFeedbackManager  Instance;

    [Header ("Damage Numbers")]
    public GameObject damageNumberPrefab;

    [Header ("Enemy Health Bars")]
    public GameObject enemyHealthBarPrefab;

    [Header ("UI")]
    public Canvas canvas;
    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void ShowDamage(float damage, Vector3 worldPosition)
    {
        if (damageNumberPrefab == null || canvas == null)
        {
            return;
        }

        GameObject damageObject =
            Instantiate(damageNumberPrefab, canvas.transform);

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(worldPosition);

        damageObject.transform.position = screenPosition;

        DamageNumber damageNumber =
            damageObject.GetComponent<DamageNumber>();

        if (damageNumber != null)
        {
            damageNumber.SetDamage(damage);
        }
    }

    public void CreateEnemyHealthBar(EnemyShip enemy)
    {
        if (enemyHealthBarPrefab == null || canvas == null)
        {
            return;
        }

        GameObject healthBarObject =
            Instantiate(
                enemyHealthBarPrefab,
                canvas.transform
            );

        EnemyHealthBarUI healthBar =
            healthBarObject.GetComponent<EnemyHealthBarUI>();

        if (healthBar != null)
        {
            healthBar.enemy = enemy;
            healthBar.mainCamera = mainCamera;
        }
    }
}