using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Ship")]
    public Transform shipTransform;

    [Header ("Gun")]
    public GameObject shellPrefab;
    public GameObject apShellPrefab;

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
    public float apDamage = 40f;
    public float penetration = 25f;

    public float reloadTime = 1f;
    private float reloadTimer = 0f;

    [Header ("Gun Range")]
    public float gunRange = 30f;
    public float shellSpeed = 30f;

    public float ReloadTimer => reloadTimer;

    [Header("Ammo Type")]
    public bool isAP = false;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform[] torpedoLaunchers;
    public Transform[] torpedoFiringPoints;

    [Header ("Torpedo Firing Arcs")]
    public float[] torpedoMinAngles;
    public float[] torpedoMaxAngles;

    [Header("Torpedo Firing Direction Restriction")]
    public float torpedoSideMinAngle = 55f;
    public float torpedoSideMaxAngle = 125f;

    public float[] torpedoBaseAngles;

    [Header ("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 20f;
    private float torpedoReloadTimer = 0f;

    [Header ("Torpedo Range")]
    public float torpedoRange = 50f;
    public float torpedoSpeed = 20f;

    public float TorpedoReloadTimer => torpedoReloadTimer;

    // Fix Unity gun rotation from Blender
    private Quaternion[] gunRotationOffsets;
    private Quaternion[] torpedoRotationOffsets;

    private void Awake()
    {
        if (gunTransforms != null)
        {
            gunBaseAngles = new float[gunTransforms.Length];
            gunRotationOffsets = new Quaternion[gunTransforms.Length];

            for (int i = 0; i < gunTransforms.Length; i++)
            {
                Transform gun = gunTransforms[i];

                if (gun == null)
                    continue;

                gunBaseAngles[i] =
                    Mathf.DeltaAngle(0f, gun.localEulerAngles.y);

                float baseWorldAngle =
                    shipTransform.eulerAngles.y + gunBaseAngles[i];

                Quaternion baseYaw =
                    Quaternion.Euler(0f, baseWorldAngle, 0f);

                gunRotationOffsets[i] =
                    Quaternion.Inverse(baseYaw) * gun.rotation;
            }
        }

        if (torpedoLaunchers != null)
        {
            torpedoBaseAngles = new float[torpedoLaunchers.Length];
            torpedoRotationOffsets =
                new Quaternion[torpedoLaunchers.Length];

            for (int i = 0; i < torpedoLaunchers.Length; i++)
            {
                Transform launcher = torpedoLaunchers[i];

                if (launcher == null)
                    continue;

                torpedoBaseAngles[i] =
                    Mathf.DeltaAngle(0f, launcher.localEulerAngles.y);

                float baseWorldAngle =
                    shipTransform.eulerAngles.y + torpedoBaseAngles[i];

                Quaternion baseYaw =
                    Quaternion.Euler(0f, baseWorldAngle, 0f);

                torpedoRotationOffsets[i] =
                    Quaternion.Inverse(baseYaw) * launcher.rotation;
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
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            isAP = !isAP;
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

            // Get the turret's current angle relative to its allowed arc.
            float currentRelativeToShip = Mathf.DeltaAngle(
                shipTransform.eulerAngles.y,
                gun.eulerAngles.y
            );

            float currentRelativeToGun = Mathf.DeltaAngle(
                gunBaseAngles[i],
                currentRelativeToShip
            );

            // Keep the current angle inside the allowed arc.
            currentRelativeToGun = Mathf.Clamp(
                currentRelativeToGun,
                gunMinAngles[i],
                gunMaxAngles[i]
            );

            // Move numerically through the allowed arc instead of taking
            // the shortest quaternion path through the rear of the ship.
            float nextAngle = Mathf.MoveTowards(
                currentRelativeToGun,
                clampedAngle,
                rotationSpeed * Time.deltaTime
            );

            float finalAngle =
                shipTransform.eulerAngles.y +
                gunBaseAngles[i] +
                nextAngle;

            gun.rotation =
                Quaternion.Euler(0f, finalAngle, 0f)
                * gunRotationOffsets[i];
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

            // Get the launcher's current angle relative to its allowed arc.
            float currentRelativeToShip = Mathf.DeltaAngle(
                shipTransform.eulerAngles.y,
                launcher.eulerAngles.y
            );

            float currentRelativeToLauncher = Mathf.DeltaAngle(
                torpedoBaseAngles[i],
                currentRelativeToShip
            );

            // Keep the current angle inside the allowed arc.
            currentRelativeToLauncher = Mathf.Clamp(
                currentRelativeToLauncher,
                torpedoMinAngles[i],
                torpedoMaxAngles[i]
            );

            // Move through the permitted arc without taking the shortest
            // quaternion path through the blocked rear section.
            float nextAngle = Mathf.MoveTowards(
                currentRelativeToLauncher,
                clampedAngle,
                rotationSpeed * Time.deltaTime
            );

            float finalAngle =
                shipTransform.eulerAngles.y +
                torpedoBaseAngles[i] +
                nextAngle;

            launcher.rotation =
                Quaternion.Euler(0f, finalAngle, 0f)
                * torpedoRotationOffsets[i];
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
        // Only allow torpedo firing toward the sides of the ship.
        Camera cam = Camera.main;

        if (cam == null || shipTransform == null)
            return;

        Plane oceanPlane = new Plane(
            Vector3.up,
            transform.position
        );

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!oceanPlane.Raycast(ray, out float distance))
            return;

        Vector3 targetPoint = ray.GetPoint(distance);
        Vector3 directionToTarget = targetPoint - shipTransform.position;
        directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude < 0.001f)
            return;

        float targetAngle = Quaternion.LookRotation(
            directionToTarget,
            Vector3.up
        ).eulerAngles.y;

        float targetRelativeToShip = Mathf.Abs(
            Mathf.DeltaAngle(
                shipTransform.eulerAngles.y,
                targetAngle
            )
        );

        // Reject targets in the blocked front or rear sectors.
        if (targetRelativeToShip < torpedoSideMinAngle ||
            targetRelativeToShip > torpedoSideMaxAngle)
        {
            return;
        }

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

                torpedo.maxDistance = torpedoRange;
                torpedo.speed = torpedoSpeed;
            }
        }

        torpedoReloadTimer = torpedoReloadTime;

    }
}