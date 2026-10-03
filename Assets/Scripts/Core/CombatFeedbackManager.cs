using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatFeedbackManager  : MonoBehaviour
{
    public static CombatFeedbackManager  Instance;

    [Header ("Damage Numbers")]
    public GameObject damageNumberPrefab;

    [Header ("Enemy Health Bars")]
    public GameObject enemyHealthBarPrefab;

    [Header ("UI")]
    public Canvas canvas;
    public Camera mainCamera;
    
    private Dictionary<GameObject, Coroutine> activeFlashes =
        new Dictionary<GameObject, Coroutine>();

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

    public void ApplyEliteVisual(GameObject ship, Color eliteColor)
    {
        if (ship == null)
            return;

        Renderer[] renderers =
            ship.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Color baseColor = renderer.material.color;

            Color tintedColor =
                Color.Lerp(
                    baseColor,
                    eliteColor,
                    0.5f
                );

            renderer.material.color = tintedColor;
        }
    }

    // Combat Flash
    public void HitFlash(GameObject ship)
    {
        FlashShip(ship, Color.white);
    }

    public void ArmorBlockedFlash(GameObject ship)
    {
        FlashShip(ship, Color.red);
    }

    private void FlashShip(GameObject ship, Color flashColor)
    {
        if (ship == null)
        {
            return;
        }

        // Ignore another flash while this ship is already flashing.
        if (activeFlashes.ContainsKey(ship))
        {
            return;
        }

        Coroutine flash =
            StartCoroutine(
                FlashRoutine(ship, flashColor)
            );

        activeFlashes.Add(ship, flash);
    }

    private IEnumerator FlashRoutine(
        GameObject ship,
        Color flashColor
    )
    {
        Renderer[] renderers =
            ship.GetComponentsInChildren<Renderer>();

        Color[] originalColors =
            new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] =
                renderers[i].material.color;

            renderers[i].material.color =
                flashColor;
        }

        yield return new WaitForSeconds(0.08f);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].material.color =
                    originalColors[i];
            }
        }

        activeFlashes.Remove(ship);
    }
}