using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header ("Gun")]
    public GameObject shellPrefab;
    public Transform[] gunTransforms;
    public Transform[] firingPoints;
    
    [Header ("Gun Rotation")]
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

        if (Input.GetMouseButton(1) && torpedoReloadTimer <= 0f)
        {
            FireTorpedo();
        }
    }

    private void HandleGunRotation()
    {
        if (gunTransforms == null || gunTransforms.Length == 0)
            return;

        Camera cam = Camera.main;

        if (cam == null)
            return;

        //Horizontal plane at gun height
        Plane oceanPlane = new Plane(
            Vector3.up,
            transform.position
        );

        //Aimming
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!oceanPlane.Raycast(ray, out float distance))
            return;

        Vector3 targetPoint = ray.GetPoint(distance);

        foreach (Transform gun in gunTransforms)
        {
            if (gun == null)
                continue;

            Vector3 direction = targetPoint - gun.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                continue;

            Quaternion targetRotation =
                Quaternion.LookRotation(direction, Vector3.up);

            //Rotate Turret
            gun.rotation = Quaternion.RotateTowards(
                gun.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (torpedoLauncher != null)
            {
                torpedoLauncher.rotation = gun.rotation;
            }
        }
    }

    private void Fire()
    {
        if (shellPrefab == null || firingPoint == null)
        {
            Debug.LogWarning("Missing shell prefab");
            return;
        }

        if (firingPoints == null || firingPoints.Length == 0)
        {
            Debug.LogWarning("Missing firing point");
            return;
        }
        foreach (Transform point in firingPoints)
        {
            if (point == null)
                continue;
            
            GameObject shellObject = Instantiate(
                shellPrefab,
                point.position,
                point.rotation
            );

            Shell shell = shellObject.GetComponent<Shell>();

            if (shell != null)
            {
                shell.damage = damage;
            }
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