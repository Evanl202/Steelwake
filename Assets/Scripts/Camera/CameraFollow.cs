using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header ("Target")]
    public Transform target;

    [Header ("Camera Position")]
    public Vector3 offset = new Vector3(0f, 25f, -10f);

    [Header ("Camera Movement")]
    public float followSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}