using System.Collections.Generic;
using UnityEngine;

public class RammingCollision : MonoBehaviour
{
    private PlayerShip playerShip;

    // Prevents multiple colliders from damaging the same enemy repeatedly.
    private HashSet<EnemyShip> rammedEnemies = new HashSet<EnemyShip>();

    private void Awake()
    {
        playerShip = GetComponent<PlayerShip>();

        if (playerShip == null)
        {
            Debug.LogError(
                "RammingCollision requires PlayerShip on the same GameObject."
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerShip == null)
            return;

        EnemyShip enemy = other.GetComponentInParent<EnemyShip>();

        if (enemy == null || rammedEnemies.Contains(enemy))
            return;

        rammedEnemies.Add(enemy);

        // Calculate damage before destroying the enemy.
        float damagePercentage = GetDamagePercentage(
            playerShip.shipWeight,
            enemy.shipWeight
        );

        float playerDamage = enemy.maxHealth * damagePercentage;

        // Kill the enemy using its existing damage system.
        enemy.TakeDamage(enemy.currentHealth);

        // Apply ramming damage to the player.
        playerShip.TakeDamage(playerDamage);

        Debug.Log(
            "Ramming collision! Player weight: " +
            playerShip.shipWeight +
            ", Enemy weight: " +
            enemy.shipWeight +
            ", Player damage: " +
            playerDamage
        );
    }

    private float GetDamagePercentage(int playerWeight, int enemyWeight)
    {
        playerWeight = Mathf.Clamp(playerWeight, 1, 3);
        enemyWeight = Mathf.Clamp(enemyWeight, 1, 3);

        if (playerWeight == 1)
        {
            switch (enemyWeight)
            {
                case 1: return 0.50f;
                case 2: return 0.75f;
                case 3: return 1.00f;
            }
        }

        if (playerWeight == 2)
        {
            switch (enemyWeight)
            {
                case 1: return 0.35f;
                case 2: return 0.50f;
                case 3: return 0.75f;
            }
        }

        if (playerWeight == 3)
        {
            switch (enemyWeight)
            {
                case 1: return 0.20f;
                case 2: return 0.35f;
                case 3: return 0.50f;
            }
        }

        return 0.50f;
    }
}
