using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header ("Weapon")]
    public GameObject shellPrefab;
    public Transform firingPoint;

    [Header ("Torpedo")]
    public GameObject torpedoPrefab;
    public Transform torpedoFiringPoint;

    [Header ("Gun Combat")]
    public float gunDamage = 10f;
    public float gunReloadTime = 2f;
    public float gunFiringRange = 25f;

    [Header ("Torpedo Combat")]
    public float torpedoDamage = 50f;
    public float torpedoReloadTime = 8f;
    public float torpedoFiringRange = 35f;

    private float reloadTimer = 0f;
    private float torpedoReloadTimer = 0f;

    private Transform player;

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

        AimAtPlayer();

        float distance = Vector3.Distance( transform.position, player.position);

        //Gun
        if (distance <= gunFiringRange && reloadTimer <= 0f)
        {
            FireGun();
        }

        //Torpedo
        if (distance <= torpedoFiringRange && torpedoReloadTimer <= 0f)
        {
            FireTorpedo();
        }
    }

    private void AimAtPlayer()
    {
        //Aim Gun
        Vector3 gunDirection = player.position - transform.position;

        gunDirection.y = 0f;

        if (gunDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(gunDirection);
        }

        //Aim Torpedo
        if (torpedoFiringPoint != null)
        {
            Transform launcher = torpedoFiringPoint.parent;

            Vector3 torpedoDirection = player.position - launcher.position;

            torpedoDirection.y = 0f;

            if (torpedoDirection.sqrMagnitude > 0.01f)
            {
                launcher.rotation = Quaternion.LookRotation(torpedoDirection);
            }
        }
    }

    private void FireGun()
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

        EnemyShell shell = shellObject.GetComponent<EnemyShell>();

        if (shell != null)
        {
            shell.damage = gunDamage;
        }

        reloadTimer = gunReloadTime;
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

        EnemyTorpedo torpedo = torpedoObject.GetComponent<EnemyTorpedo>();

        if (torpedo != null)
        {
            torpedo.damage = torpedoDamage;
        }

        torpedoReloadTimer = torpedoReloadTime;
    }
}