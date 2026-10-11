
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ship")]
    public Transform shipTransform;

    [Header("Gun")]
    public GameObject shellPrefab;
    public GameObject apShellPrefab;

    public Transform[] gunTransforms;
    public Transform[] firingPoints;

    [Header("Gun Rotation")]
    public float rotationSpeed = 360f;

    [Header("Gun Firing Arcs")]
    public float[] gunMinAngles;
    public float[] gunMaxAngles;
    public float[] gunBaseAngles;

    [Header("Gun Combat")]
    public float damage = 25f;
    public float apDamage = 40f;
    public float penetration = 25f;
    public float reloadTime = 1f;
    private float reloadTimer = 0f;

    [Header("Gun Range")]
    public float gunRange = 30f;
    public float shellSpeed = 30f;

    public float ReloadTimer => reloadTimer;

    [Header("Ammo Type")]
    public bool isAP = false;

    [Header("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform[] torpedoLaunchers;
    public Transform[] torpedoFiringPoints;

    [Header("Torpedo Firing Arcs")]
    public float[] torpedoMinAngles;
    public float[] torpedoMaxAngles;
    public float[] torpedoBaseAngles;

    [Header("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 5f;
    private float torpedoReloadTimer = 0f;

    [Header("Torpedo Range")]
    public float torpedoRange = 35f;
    public float torpedoSpeed = 20f;

    public float TorpedoReloadTimer => torpedoReloadTimer;

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
        {
            return;
        }

        HandleGunRotation();

        if (reloadTimer > 0f)
            reloadTimer -= Time.deltaTime;

        if (torpedoReloadTimer > 0f)
            torpedoReloadTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            isAP = !isAP;

        if (Input.GetMouseButton(0) && reloadTimer <= 0f)
            Fire();

        if (Input.GetMouseButton(1) && torpedoReloadTimer <= 0f)
            FireTorpedo();
    }

    private void HandleGunRotation()
    {
        if (shipTransform == null ||
            gunTransforms == null ||
            gunMinAngles == null ||
            gunMaxAngles == null ||
            gunBaseAngles == null ||
            gunTransforms.Length != gunMinAngles.Length ||
            gunTransforms.Length != gunMaxAngles.Length ||
            gunTransforms.Length != gunBaseAngles.Length)
        {
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
            return;

        Plane oceanPlane = new Plane(Vector3.up, transform.position);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!oceanPlane.Raycast(ray, out float distance))
            return;

        Vector3 targetPoint = ray.GetPoint(distance);

        float shipYaw = shipTransform.eulerAngles.y;

        for (int i = 0; i < gunTransforms.Length; i++)
        {
            Transform gun = gunTransforms[i];
            if (gun == null)
                continue;

            Vector3 direction = targetPoint - gun.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                continue;

            float targetYaw =
                Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;

            float relativeAngle =
                Mathf.DeltaAngle(shipYaw, targetYaw);

            float relativeToGun =
                Mathf.DeltaAngle(gunBaseAngles[i], relativeAngle);

            float clampedAngle = Mathf.Clamp(
                relativeToGun,
                gunMinAngles[i],
                gunMaxAngles[i]
            );

            float finalYaw =
                shipYaw + gunBaseAngles[i] + clampedAngle;

            Quaternion targetRotation =
                Quaternion.Euler(0f, finalYaw, 0f);

            gun.rotation = Quaternion.RotateTowards(
                gun.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (torpedoLaunchers == null ||
            torpedoMinAngles == null ||
            torpedoMaxAngles == null ||
            torpedoBaseAngles == null ||
            torpedoLaunchers.Length != torpedoMinAngles.Length ||
            torpedoLaunchers.Length != torpedoMaxAngles.Length ||
            torpedoLaunchers.Length != torpedoBaseAngles.Length)
        {
            return;
        }

        for (int i = 0; i < torpedoLaunchers.Length; i++)
        {
            Transform launcher = torpedoLaunchers[i];
            if (launcher == null)
                continue;

            Vector3 direction = targetPoint - launcher.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                continue;

            float targetYaw =
                Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;

            float relativeAngle =
                Mathf.DeltaAngle(shipYaw, targetYaw);

            float relativeToLauncher =
                Mathf.DeltaAngle(torpedoBaseAngles[i], relativeAngle);

            float clampedAngle = Mathf.Clamp(
                relativeToLauncher,
                torpedoMinAngles[i],
                torpedoMaxAngles[i]
            );

            float finalYaw =
                shipYaw + torpedoBaseAngles[i] + clampedAngle;

            Quaternion targetRotation =
                Quaternion.Euler(0f, finalYaw, 0f);

            launcher.rotation = Quaternion.RotateTowards(
                launcher.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void Fire()
    {
        GameObject selectedShell = isAP ? apShellPrefab : shellPrefab;

        if (selectedShell == null)
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
                selectedShell,
                point.position,
                point.rotation
            );

            Shell shell = shellObject.GetComponent<Shell>();

            if (shell != null)
            {
                shell.damage = isAP ? apDamage : damage;
                shell.isAP = isAP;
                shell.penetration = penetration;
                shell.maxDistance = gunRange;
                shell.speed = shellSpeed;
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

        if (torpedoFiringPoints == null ||
            torpedoFiringPoints.Length == 0)
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
                torpedo.maxDistance = torpedoRange;
                torpedo.speed = torpedoSpeed;
            }
        }

        torpedoReloadTimer = torpedoReloadTime;
    }
}