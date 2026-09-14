using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header ("Weapon")]
    public GameObject shellPrefab;
    public Transform[] firingPoints;
    public Transform[] guns;
    
    [Header ("Turret Firing Arcs")]
    public float[] gunMinAngles;
    public float[] gunMaxAngles;
    public float[] gunBaseAngles;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform[] torpedoFiringPoints;
    public Transform[] torpedoLaunchers;

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

            for (int i = 0; i < torpedoLaunchers.Length; i++);
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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

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
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
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

        float distance = Vector3.Distance( transform.position, player.position);

        //Gun
        if (distance <= gunFiringRange && reloadTimer <= 0f)
        {
            FireGun();
        }

        //Torpedo
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
        //Aim Gun
        if (guns != null &&
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

                Quaternion targetRotation =
                    GetClampedWeaponRotation(
                        guns[i],
                        gunBaseAngles[i],
                        gunMinAngles[i],
                        gunMaxAngles[i],
                        shellSpeed
                    );

                guns[i].rotation = 
                    Quaternion.RotateTowards(
                        guns[i].rotation,
                        targetRotation,
                        360 * Time.deltaTime
                    );
            }
        }

        //Aim Torpedo
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
                if torpedoLaunchers[i] == null
                    continue;

                Quaternion torpedoTarget = 
                    GetClampedWeaponRotation(
                        torpedoLaunchers[i],
                        torpedoBaseAngles[i],
                        torpedoMinAngles[i],
                        torpedoMaxAngles[i],
                        15f
                    );

                torpedoLaunchers[i].rotation =
                    Quaternion.RotateTowards(
                        torpedoLaunchers[i].rotation,
                        torpedoTarget,
                        360f * Time.deltaTime
                    );
            }
        }
    }

    private float GetPlayerSpeed()
    {
        ShipMovement playerMovement = player.GetComponent<ShipMovement>();

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
        Vector3 toTarget = targetPosition - shooterPosition;

        toTarget.y = 0f;
        targetVelocity.y = 0f;

        //Projectile interception
        float a = 
            Vector3.Dot(targetVelocity, targetVelocity)
            - projectileSpeed * projectileSpeed;

        //If ship speed and shell speed same
        if (Mathf.Abs(a) < 0.001f)
        {
            return targetPosition;
        }

        float b = 2 * Vector3.Dot(toTarget, targetVelocity);

        float c = Vector3.Dot(toTarget, toTarget);

        float discriminant = b * b - 4f * a * c;

        //If no interception point
        if (discriminant < 0f)
        {
            return targetPosition;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);

        float t1 = (-b - sqrtDiscriminant) / (2f * a);

        float t2 = (-b + sqrtDiscriminant) / (2f * a);

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

        return targetPosition + targetVelocity * travelTime;
    }

    private float GetTargetRelativeAngle(
        Transform weapon,
        float baseAngle,
        float projectileSpeed)
    {
        Vector3 target =
            CalculateInterceptPoint(
                weapon.position,
                player.position,
                player.forward * GetPlayerSpeed(),
                projectileSpeed
            );

        Vector3 direction =
            target - weapon.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return 0f;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction,
                Vector3.up
            );

        float targetAngle =
            targetRotation.eulerAngles.y;

        float targetRelativeToShip =
            Mathf.DeltaAngle(
                transform.eulerAngles.y,
                targetAngle
            );

        return Mathf.DeltaAngle(
            baseAngle,
            targetRelativeToShip
        );
    }

    private Quaternion GetClampedWeaponRotation(
        Transform weapon,
        float baseAngle,
        float minAngle,
        float maxAngle,
        float projectileSpeed)
    {
        float targetRelativeAngle =
            GetTargetRelativeAngle(
                weapon,
                baseAngle,
                projectileSpeed
            );

        float clampedAngle =
            Mathf.Clamp(
                targetRelativeAngle,
                minAngle,
                maxAngle
            );

        float finalAngle =
            transform.eulerAngles.y
            + baseAngle
            + clampedAngle;

        return Quaternion.Euler(
            0f,
            finalAngle,
            0f
        );
    }

    private bool IsTargetInArc(
        Transform weapon,
        float baseAngle,
        float minAngle,
        float maxAngle,
        float projectileSpeed)
    {
        float targetRelativeAngle =
            GetTargetRelativeAngle(
                weapon,
                baseAngle,
                projectileSpeed
            );

        return targetRelativeAngle >= minAngle &&
            targetRelativeAngle <= maxAngle;
    }

    private void FireGun()
    {
        if (shellPrefab == null || firingPoints == null || guns == null)
        {
            Debug.LogWarning("Missing shell, firing points, or guns");
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
            if (guns[i] == null || firingPoints[i] == null)
                continue;

            if (!IsTargetInArc(
                    guns[i],
                    gunBaseAngles[i],
                    gunMinAngles[i],
                    gunMaxAngles[i],
                    shellSpeed))
            {
                continue;
            }

            GameObject shellObject = Instantiate(
                shellPrefab,
                firingPoints[i].position,
                firingPoints[i].rotation
            );

            EnemyShell shell = shellObject.GetComponent<EnemyShell>();

            if (shell != null)
            {
                shell.damage = gunDamage;
            }

            fired = true;
        }
            
        if(fired)
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
            Debug.LogWarning("Missing torpedo, torpedo firing points, or launcher");
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

        for (int i = 0; i < torpedoCount; i++);
        {
            if (torpedoLaunchers[i] == null || torpedoFiringPoints[i] == null)
                continue;

            if (!IsTargetInArc(
                torpedoLaunchers[i],
                torpedoBaseAngles[i],
                torpedoMinAngles[i],
                torpedoMaxAngles[i],
                15f))
            {
                continue;
            }

            GameObject torpedoObject = Instantiate(
                torpedoPrefab,
                torpedoFiringPoints[i].position,
                torpedoFiringPoints[i].rotation
            );

            EnemyTorpedo torpedo = torpedoObject.GetComponent<EnemyTorpedo>();

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