using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header ("Weapon")]
    public GameObject shellPrefab;
    public Transform firingPoint;

    [Header ("Combat")]
    public float damage = 10f;
    public float reloadTime = 2f;
    public float firingRange = 25f;

    private float reloadTimer = 0f;
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

        if (distance <= firingRange && reloadTimer <= 0f)
        {
            Fire();
        }
    }

    private void AimAtPlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
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

        EnemyShell shell = shellObject.GetComponent<EnemyShell>();

        if (shell != null)
        {
            shell.damage = damage;
        }

        reloadTimer = reloadTime;
    }
}