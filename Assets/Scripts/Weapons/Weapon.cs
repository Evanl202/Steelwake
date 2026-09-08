using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header ("Gun")]
    public GameObject shellPrefab;
    public Transform firingPoint;

    [Header ("Gun Rotation")]
    public Transform gunTransform;
    public float rotationSpeed = 360f;

    [Header ("Gun Combat")]
    public float damage = 25f;
    public float reloadTime = 1f;
    private float reloadTimer = 0f;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform torpedoFiringPoint;
    public Transform torpedoLauncher;

    [Header ("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 5f;
    private float torpedoReloadTimer = 0f;

    void Update()
    {

        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }

        HandleGunRotation();

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }

        if (torpedoReloadTimer > 0f)
        {
            torpedoReloadTimer -=Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && reloadTimer <= 0f)
        {
            Fire();
        }

        if (Input.GetMouseButton(1) && reloadTimer <= 0f)
        {
            FireTorpedo();
        }
    }

    private void HandleGunRotation()
    {
        if (gunTransform == null)
            return;

        Camera cam = Camera.main;

        if (cam == null)
            return;

        //Horizontal plane at gun height
        Plane oceanPlane = new Plane(
            Vector3.up,
            gunTransform.position
        );

        //Aimming
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (oceanPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            Vector3 direction = targetPoint - gunTransform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(direction, Vector3.up);

            //Rotate Turret
            gunTransform.rotation = Quaternion.RotateTowards(
                gunTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (torpedoLauncher != null)
            {
                torpedoLauncher.rotation = gunTransform.rotation;
            }
        }
    }

    private void Fire()
    {
        if (shellPrefab == null || firingPoint == null)
        {
            Debug.LogWarning("Missing shell or firing point");
            return;
        }

        GameObject shellObject = Instantiate(
            shellPrefab,
            firingPoint.position,
            firingPoint.rotation
        );

        Shell shell = shellObject.GetComponent<Shell>();

        if (shell != null)
        {
            shell.damage = damage;
        }

        reloadTimer = reloadTime;
    }

    private void FireTorpedo()
    {
        if (torpedoPrefab == null || torpedoFiringPoint == null)
        {
            Debug.LogWarning("Missing torpedo or torpedo firing point");
            return;            
        }

        GameObject torpedoObject = Instantiate(
            torpedoPrefab,
            torpedoFiringPoint.position,
            torpedoFiringPoint.rotation
        );

        Torpedo torpedo = torpedoObject.GetComponent<Torpedo>();

        if (torpedo != null)
        {
            torpedo.damage = torpedoDamage;
        }

        torpedoReloadTimer = torpedoReloadTime;

    }
}