using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header ("Target")]
    public Transform target;

    [Header ("Camera Position")]
    public Vector3 offset = new Vector3(0f, 30f, -15f);

    [Header ("Camera Movement")]
    public float followSpeed = 5f;

    [Header("Zoom")]
    public float zoomStep = 1f;
    public float zoomOutAmount = 10f;

    private Camera cam;
    private float baseZoom;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("CameraFollow must be attached to a Camera.");
            return;
        }

        baseZoom = cam.orthographicSize;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        HandleZoom();

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomStep;

            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize,
                baseZoom,
                baseZoom + zoomOutAmount
            );
        }
    }
}