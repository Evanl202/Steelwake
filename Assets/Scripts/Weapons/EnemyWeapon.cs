using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header ("Weapon")]
    public GameObject shellPrefab;
    public Transform[] guns;
    public Transform[] firingPoints;

    [Header ("Ship")]
    public Transform shipTransform;

    [Header ("Turret Firing Arcs")]
    public float[] gunMinAngles;
    public float[] gunMaxAngles;
    public float[] gunBaseAngles;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform[] torpedoLaunchers;
    public Transform[] torpedoFiringPoints;

    public float[] torpedoMinAngles;
    public float[] torpedoMaxAngles;

    private float[] torpedoBaseAngles;

    [Header ("Gun Combat")]
    public float gunDamage = 10f;
    public float gunReloadTime = 2f;
    public float gunFiringRange = 25f;
    public float shellSpeed = 20f;

    [Header ("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 8f;
    public float torpedoFiringRange = 35f;

    private float reloadTimer = 0f;
    private float torpedoReloadTimer = 0f;

    private Transform player;

    private void Awake()
    {
        if (shipTransform == null)
        {
            shipTransform = transform.root;
        }

        if (guns != null)
        {
            gunBaseAngles = new float[guns.Length];

            for (int i = 0; i < guns.Length; i++)
            {
                if (guns[i] != null)
                {
                    gunBaseAngles[i] =
                        Mathf.DeltaAngle(
                            0f,
                            guns[i].localEulerAngles.y
                        );
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
                        Mathf.DeltaAngle(
                            0f,
                            torpedoLaunchers[i].localEulerAngles.y
                        );
                }
            }
        }
    }

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("EnemyWeapon can't find Player");
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
        {
            return;
        }

        if (player == null)
            return;
        
        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }

        if (torpedoReloadTimer > 0f)
        {
            torpedoReloadTimer -= Time.deltaTime;
        }
        
        AimAtPlayer();

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // Gun
        if (distance <= gunFiringRange &&
            reloadTimer <= 0f)
        {
            FireGun();
        }

        // Torpedo
        if (torpedoPrefab != null &&
            torpedoFiringPoints != null &&
            torpedoLaunchers != null &&
            distance <= torpedoFiringRange &&
            torpedoReloadTimer <= 0f)
        {
            FireTorpedo();
        }
    }

    private void AimAtPlayer()
    {
        // Aim Gun
        if (guns != null &&
            gunFiringPoints != null &&
            gunBaseAngles != null &&
            gunMinAngles != null &&
            gunMaxAngles != null)
        {
            int gunCount = Mathf.Min(
                firingPoints.Length,
                guns.Length,
                gunBaseAngles.Length,
                gunMinAngles.Length,
                gunMaxAngles.Length
            );

            for (int i = 0; i < gunCount; i++)
            {
                if (guns[i] == null)
                    continue;

                float targetAngle =
                    GetTargetAngle(
                        guns[i],
                        shellSpeed
                    );

                float clampedAngle =
                    ClampWeaponAngle(
                        targetAngle,
                        gunBaseAngles[i],
                        gunMinAngles[i],
                        gunMaxAngles[i]
                    );

                Quaternion targetRotation =
                    GetWeaponRotation(
                        gunBaseAngles[i],
                        clampedAngle
                    );

                RotateWeapon(
                    guns[i],
                    targetRotation
                );
            }
        }

        // Aim Torpedo
        if (torpedoLaunchers != null &&
            torpedoFiringPoints != null &&
            torpedoBaseAngles != null &&
            torpedoMinAngles != null &&
            torpedoMaxAngles != null)
        {
            int torpedoCount = Mathf.Min(
                torpedoLaunchers.Length,
                torpedoFiringPoints.Length,
                torpedoBaseAngles.Length,
                torpedoMinAngles.Length,
                torpedoMaxAngles.Length
            );
            
            for (int i = 0; i < torpedoCount; i++)
            {
                if (torpedoLaunchers[i] == null)
                    continue;

                float targetAngle =
                    GetTargetAngle(
                        torpedoLaunchers[i],
                        15f
                    );

                float clampedAngle =
                    ClampWeaponAngle(
                        targetAngle,
                        torpedoBaseAngles[i],
                        torpedoMinAngles[i],
                        torpedoMaxAngles[i]
                    );

                Quaternion targetRotation =
                    GetWeaponRotation(
                        torpedoBaseAngles[i],
                        clampedAngle
                    );

                RotateWeapon(
                    torpedoLaunchers[i],
                    targetRotation
                );
            }
        }
    }

    // Helper Functions
    private float GetTargetAngle(
        Transform weapon,
        float projectileSpeed)
    {
        Vector3 target =
            CalculateInterceptPoint(
                weapon.position,
                player.position,
                player.forward * GetPlayerSpeed(),
                projectileSpeed
            );

        return GetTargetAngle(
            weapon,
            target
        );
    }

    private float GetTargetAngle(
        Transform weapon,
        Vector3 targetPosition)
    {
        Vector3 direction =
            targetPosition - weapon.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return 0f;

        Vector3 localDirection =
            shipTransform.InverseTransformDirection(
                direction.normalized
            );

        return Mathf.Atan2(
            localDirection.x,
            localDirection.z
        ) * Mathf.Rad2Deg;
    }

    private float ClampWeaponAngle(
        float targetAngle,
        float baseAngle,
        float minAngle,
        float maxAngle)
    {
        float relativeAngle =
            Mathf.DeltaAngle(
                baseAngle,
                targetAngle
            );

        return Mathf.Clamp(
            relativeAngle,
            minAngle,
            maxAngle
        );
    }

    private Quaternion GetWeaponRotation(
        float baseAngle,
        float clampedAngle)
    {
        float finalAngle =
            shipTransform.eulerAngles.y +
            baseAngle +
            clampedAngle;

        return Quaternion.Euler(
            0f,
            finalAngle,
            0f
        );
    }

    private void RotateWeapon(
        Transform weapon,
        Quaternion targetRotation)
    {
        weapon.rotation =
            Quaternion.RotateTowards(
                weapon.rotation,
                targetRotation,
                360f * Time.deltaTime
            );
    }

    // Firing Prediction
    private float GetPlayerSpeed()
    {
        ShipMovement playerMovement =
            player.GetComponent<ShipMovement>();

        if (playerMovement != null)
        {
            return playerMovement.CurrentSpeed;
        }

        return 0f;
    }

    private Vector3 CalculateInterceptPoint(
        Vector3 shooterPosition,
        Vector3 targetPosition,
        Vector3 targetVelocity,
        float projectileSpeed)
    {
        Vector3 toTarget =
            targetPosition - shooterPosition;

        toTarget.y = 0f;
        targetVelocity.y = 0f;

        // Projectile interception
        float a =
            Vector3.Dot(
                targetVelocity,
                targetVelocity
            ) -
            projectileSpeed * projectileSpeed;

        // If ship speed and shell speed are the same
        if (Mathf.Abs(a) < 0.001f)
        {
            return targetPosition;
        }

        float b =
            2 * Vector3.Dot(
                toTarget,
                targetVelocity
            );

        float c =
            Vector3.Dot(
                toTarget,
                toTarget
            );

        float discriminant =
            b * b - 4f * a * c;

        // If no interception point
        if (discriminant < 0f)
        {
            return targetPosition;
        }

        float sqrtDiscriminant =
            Mathf.Sqrt(discriminant);

        float t1 =
            (-b - sqrtDiscriminant) /
            (2f * a);

        float t2 =
            (-b + sqrtDiscriminant) /
            (2f * a);

        float travelTime = 0f;

        if (t1 > 0f)
        {
            travelTime = t1;
        }
        else if (t2 > 0f)
        {
            travelTime = t2;
        }

        if (travelTime <= 0f)
        {
            return targetPosition;
        }

        return targetPosition +
               targetVelocity * travelTime;
    }

    private bool IsTargetInArc(
        Transform weapon,
        float baseAngle,
        float minAngle,
        float maxAngle,
        float projectileSpeed)
    {
        float targetAngle =
            GetTargetAngle(
                weapon,
                projectileSpeed
            );

        float relativeAngle =
            Mathf.DeltaAngle(
                baseAngle,
                targetAngle
            );

        return relativeAngle >= minAngle &&
               relativeAngle <= maxAngle;
    }

    // Firing
    private void FireGun()
    {
        if (shellPrefab == null ||
            firingPoints == null ||
            guns == null)
        {
            Debug.LogWarning(
                "Missing shell, firing points, or guns"
            );

            return;
        }

        int gunCount = Mathf.Min(
            firingPoints.Length,
            guns.Length,
            gunBaseAngles.Length,
            gunMinAngles.Length,
            gunMaxAngles.Length
        );

        bool fired = false;

        for (int i = 0; i < gunCount; i++)
        {
            if (guns[i] == null ||
                firingPoints[i] == null)
            {
                continue;
            }

            if (!IsTargetInArc(
                    guns[i],
                    gunBaseAngles[i],
                    gunMinAngles[i],
                    gunMaxAngles[i],
                    shellSpeed))
            {
                continue;
            }

            GameObject shellObject =
                Instantiate(
                    shellPrefab,
                    firingPoints[i].position,
                    firingPoints[i].rotation
                );

            EnemyShell shell =
                shellObject.GetComponent<EnemyShell>();

            if (shell != null)
            {
                shell.damage = gunDamage;
            }

            fired = true;
        }
            
        if (fired)
        {
            reloadTimer = gunReloadTime;
        }
    }

    private void FireTorpedo()
    {
        if (torpedoPrefab == null ||
            torpedoFiringPoints == null || 
            torpedoLaunchers == null)
        {
            Debug.LogWarning(
                "Missing torpedo, torpedo firing points, or launcher"
            );

            return;
        }

        int torpedoCount = Mathf.Min(
            torpedoLaunchers.Length,
            torpedoFiringPoints.Length,
            torpedoBaseAngles.Length,
            torpedoMinAngles.Length,
            torpedoMaxAngles.Length
        );

        bool fired = false;

        for (int i = 0; i < torpedoCount; i++)
        {
            if (torpedoLaunchers[i] == null ||
                torpedoFiringPoints[i] == null)
            {
                continue;
            }

            if (!IsTargetInArc(
                torpedoLaunchers[i],
                torpedoBaseAngles[i],
                torpedoMinAngles[i],
                torpedoMaxAngles[i],
                15f))
            {
                continue;
            }

            GameObject torpedoObject =
                Instantiate(
                    torpedoPrefab,
                    torpedoFiringPoints[i].position,
                    torpedoFiringPoints[i].rotation
                );

            EnemyTorpedo torpedo =
                torpedoObject.GetComponent<EnemyTorpedo>();

            if (torpedo != null)
            {
                torpedo.damage = torpedoDamage;
            }

            fired = true;
        }

        if (fired)
        {
            torpedoReloadTimer = torpedoReloadTime;
        }
    }
}