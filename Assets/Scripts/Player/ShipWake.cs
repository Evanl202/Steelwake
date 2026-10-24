
using System.Collections.Generic;
using UnityEngine;

public class ShipWake : MonoBehaviour
{
    [Header("References")]
    public ShipMovement shipMovement;
    public GameObject wakeSegmentPrefab;

    [Header("Wake Settings")]
    public float minimumSpeed = 0.5f;
    public float spawnInterval = 0.12f;
    public float sternOffset = 4f;
    public float waterHeightOffset = 0.15f;

    [Header("Appearance")]
    public float minimumWidth = 0.7f;
    public float maximumWidth = 1.8f;
    public float lifetime = 3f;

    private float spawnTimer;

    private class WakeSegment
    {
        public GameObject gameObject;
        public Renderer renderer;
        public MaterialPropertyBlock properties;
        public Color originalColor;
        public int colorProperty;
        public float age;
    }

    private readonly List<WakeSegment> activeSegments =
        new List<WakeSegment>();

    private void Awake()
    {
        if (shipMovement == null)
            shipMovement = GetComponent<ShipMovement>();
    }

    private void Update()
    {
        UpdateSegments();

        if (shipMovement == null || wakeSegmentPrefab == null)
            return;

        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
            return;

        float speed = Mathf.Abs(shipMovement.CurrentSpeed);

        if (speed < minimumSpeed)
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnWake(speed);
        }
    }

    private void SpawnWake(float speed)
    {
        Vector3 forward = transform.forward;

        Vector3 position =
            transform.position
            - forward * sternOffset
            + Vector3.up * waterHeightOffset;

        Quaternion rotation =
            Quaternion.LookRotation(Vector3.up, forward);

        GameObject segment = Instantiate(
            wakeSegmentPrefab,
            position,
            rotation
        );

        float speedRatio = Mathf.Clamp01(
            speed / shipMovement.maxSpeed
        );

        Vector3 scale = segment.transform.localScale;
        scale.x = Mathf.Lerp(
            minimumWidth,
            maximumWidth,
            speedRatio
        );
        segment.transform.localScale = scale;

        Renderer segmentRenderer =
            segment.GetComponent<Renderer>();

        if (segmentRenderer == null)
        {
            Destroy(segment);
            return;
        }

        Material material = segmentRenderer.sharedMaterial;

        int colorProperty = material.HasProperty("_BaseColor")
            ? Shader.PropertyToID("_BaseColor")
            : Shader.PropertyToID("_Color");

        Color originalColor = material.HasProperty(colorProperty)
            ? material.GetColor(colorProperty)
            : Color.white;

        activeSegments.Add(new WakeSegment
        {
            gameObject = segment,
            renderer = segmentRenderer,
            properties = new MaterialPropertyBlock(),
            originalColor = originalColor,
            colorProperty = colorProperty,
            age = 0f
        });
    }

    private void UpdateSegments()
    {
        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            WakeSegment segment = activeSegments[i];

            if (segment.gameObject == null)
            {
                activeSegments.RemoveAt(i);
                continue;
            }

            segment.age += Time.deltaTime;

            float alpha = Mathf.Clamp01(
                1f - segment.age / Mathf.Max(lifetime, 0.01f)
            );

            Color color = segment.originalColor;
            color.a *= alpha;

            segment.renderer.GetPropertyBlock(segment.properties);
            segment.properties.SetColor(
                segment.colorProperty,
                color
            );
            segment.renderer.SetPropertyBlock(segment.properties);

            if (segment.age >= lifetime)
            {
                Destroy(segment.gameObject);
                activeSegments.RemoveAt(i);
            }
        }
    }

    private void OnDisable()
    {
        foreach (WakeSegment segment in activeSegments)
        {
            if (segment.gameObject != null)
                Destroy(segment.gameObject);
        }

        activeSegments.Clear();
    }
}
