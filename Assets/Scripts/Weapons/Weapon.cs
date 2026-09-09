using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ship")]
    public Transform shipTransform;

    [Header ("Gun")]
    public GameObject shellPrefab;
    public Transform[] gunTransforms;
    public Transform[] firingPoints;
    
    [Header ("Gun Rotation")]
    public float rotationSpeed = 360f;

    [Header ("Gun Firing Arcs")]
    public float[] gunMinAngles;
    public float[] gunMaxAngles;

    public float[] gunBaseAngles;

    [Header ("Gun Combat")]
    public float damage = 25f;
    public float reloadTime = 1f;
    private float reloadTimer = 0f;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform[] torpedoLaunchers;
    public Transform[] torpedoFiringPoints;

    [Header ("Torpedo Firing Arcs")]
    public float[] torpedoMinAngles;
    public float[] torpedoMaxAngles;

    public float[] torpedoBaseAngles;

    [Header ("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 5f;
    private float torpedoReloadTimer = 0f;

    private void Awake()
    {
        if (gunTransforms != null)
        {
            gunBaseAngles = new float[gunTransforms.Length];

            for (int i = 0; i < gunTransforms.Length; i++)
            {
                if (gunTransforms[i] != null)
                {
                    gunBaseAngles[i] =
                        Mathf.DeltaAngle(0f, gunTransforms[i].localEulerAngles.y);
                }
            }
        }

        if (torpedoLaunchers != null)
        {
            torpedoBaseAngles = new float[torpedoLaunchers.Length];

            for (int i = 0; i < torpedoLaunchers.Length; i++)
            {
                if (torpedoLaunchers[i] != null)
                {
                    torpedoBaseAngles[i] =
                        Mathf.DeltaAngle(0f, torpedoLaunchers[i].localEulerAngles.y);
                }
            }
        }
    }

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
        //Handle index error
        if (gunTransforms == null ||
            gunTransforms.Length == 0 ||
            gunMinAngles == null ||
            gunMaxAngles == null ||
            gunMinAngles.Length != gunTransforms.Length ||
            gunMaxAngles.Length != gunTransforms.Length)
        {
            return;
        }

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

        for (int i = 0; i < gunTransforms.Length; i++)
        {
            Transform gun = gunTransforms[i];

            if (gun == null)
                continue;

            Vector3 direction = targetPoint - gun.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                continue;

            Quaternion targetRotation =
                Quaternion.LookRotation(direction, Vector3.up);
            
            float targetAngle = targetRotation.eulerAngles.y;

            float targetRelativeToShip = Mathf.DeltaAngle(
                shipTransform.eulerAngles.y,
                targetAngle
            );

            float targetRelativeToGun = Mathf.DeltaAngle(
                gunBaseAngles[i],
                targetRelativeToShip
            );

            float clampedAngle = Mathf.Clamp(
                targetRelativeToGun,
                gunMinAngles[i],
                gunMaxAngles[i]
            );

            float finalAngle =
                shipTransform.eulerAngles.y +
                gunBaseAngles[i] +
                clampedAngle;

            targetRotation = Quaternion.Euler(
                0f,
                finalAngle,
                0f
            );

            //Rotate Turret
            gun.rotation = Quaternion.RotateTowards(
                gun.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (torpedoLaunchers == null ||
            torpedoMinAngles == null ||
            torpedoMaxAngles == null ||
            torpedoMinAngles.Length != torpedoLaunchers.Length ||
            torpedoMaxAngles.Length != torpedoLaunchers.Length)
        {
            return;
        }
        for (int i = 0; i < torpedoLaunchers.Length; i++)
        {
            Transform launcher = torpedoLaunchers[i];

            if (launcher == null)
                continue;

            Vector3 torpedoDirection = targetPoint - launcher.position;

            torpedoDirection.y = 0f;

            if (torpedoDirection.sqrMagnitude < 0.001f)
                continue;

            Quaternion torpedoRotation = 
                Quaternion.LookRotation(
                    torpedoDirection,
                    Vector3.up
                );
            
            float targetAngle = torpedoRotation.eulerAngles.y;

            float targetRelativeToShip = Mathf.DeltaAngle(
                shipTransform.eulerAngles.y,
                targetAngle
            );

            float targetRelativeToLauncher = Mathf.DeltaAngle(
                torpedoBaseAngles[i],
                targetRelativeToShip
            );

            float clampedAngle = Mathf.Clamp(
                targetRelativeToLauncher,
                torpedoMinAngles[i],
                torpedoMaxAngles[i]
            );

            float finalAngle =
                shipTransform.eulerAngles.y +
                torpedoBaseAngles[i] +
                clampedAngle;

            torpedoRotation = Quaternion.Euler(
                0f,
                finalAngle,
                0f
            );

            launcher.rotation = Quaternion.RotateTowards(
                launcher.rotation,
                torpedoRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void Fire()
    {
        if (shellPrefab == null)
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
        if (torpedoPrefab == null)
        {
            Debug.LogWarning("Missing torpedo prefab");
            return;            
        }

        if (torpedoFiringPoints == null || torpedoFiringPoints.Length == 0)
        {
            Debug.LogWarning("Missing torpedo firing points");
            return;            
        }

        foreach (Transform point in torpedoFiringPoints)
        {
            if (point == null)
                continue;
            
            GameObject torpedoObject = Instantiate(
                torpedoPrefab,
                point.position,
                point.rotation
            );

            Torpedo torpedo = torpedoObject.GetComponent<Torpedo>();

            if (torpedo != null)
            {
                torpedo.damage = torpedoDamage;
            }
        }

        torpedoReloadTimer = torpedoReloadTime;

    }
}