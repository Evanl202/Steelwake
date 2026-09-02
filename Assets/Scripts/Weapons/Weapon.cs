using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header ("Weapon")]
    public GameObject shellPrefab;
    public Transform firingPoint;

    [Header ("Reload")]
    public float reloadTime = 1f;

    private float reloadTimer = 0f;

    void Update()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer -= TIme.deltaTime;
        }

        if (Input.GetMouseButton(0) && reloadTimer <= 0f)
        {
            Fire()
        }
    }

    private void Fire()
    {
        if (shellPrefab == null || firingPoint == null)
        {
            Debug.LogWarning("Missing shell or firing point");
            return;
        }

        Instantiate(
            shellPrefab,
            firingPoint.position,
            firingPoint.rotation
        );

        reloadTimer = reloadTime;
    }
}