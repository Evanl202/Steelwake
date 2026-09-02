using UnityEngine;

public class Rammer : EnemyShip
{
    [Header ("Ramming")]
    public float damageMultiplier = 5f;
    public float minimumDamage = 10f;
    public float impactDistance = 1.5f;

    protected override void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }

        RamPlayer();
    }

    private void RamPlayer()
    {
        if(player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > impactDistance)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            Impact();
        }
    }

    private void Impact()
    {
        PlayerShip playerShip = player.GetComponent<PlayerShip>();

        if (playerShip == null)
            return;
        
        float impactSpeed = moveSpeed;

        float damage = Mathf.Max(
            minimumDamage,
            impactSpeed * damageMultiplier
        );

        Debug.Log(
            "Rammer Impact speed: " + impactSpeed +" | Damage: " + damage
        );

        playerShip.TakeDamage(damage);

        //Rammer dies on impact
        Destroy(gameObject);
    }
}