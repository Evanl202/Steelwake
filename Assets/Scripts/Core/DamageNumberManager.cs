using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public static DamageNumberManager Instance;

    public GameObject damageNumberPrefab;
    public Canvas canvas;
    public Camera mainCamera;

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
}