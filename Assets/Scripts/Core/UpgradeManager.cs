using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header ("Player")]
    public ShipMovement playerMovement;
    public PlayerShip playerShip;
    public Weapon playerWeapon;

    [Header ("Flat Upgrades")]
    public float healthFlatIncrease = 20f;
    public float armorFlatIncrease = 5f;

    public float damageFlatIncrease = 10f;
    public float heDamageFlatIncrease = 10f;
    public float apDamageFlatIncrease = 10f;
    public float torpedoDamageFlatIncrease = 10f;

    public float penetrationFlatIncrease = 5f;
    public float reloadFlatReduction = 0.05f;
    public float speedFlatIncrease = 1f;

    [Header ("Percentage Upgrades")]
    public float healthPercentIncrease = 0.10f;
    public float armorPercentIncrease = 0.10f;

    public float damagePercentIncrease = 0.10f;
    public float heDamagePercentIncrease = 0.10f;
    public float apDamagePercentIncrease = 0.10f;
    public float torpedoDamagePercentIncrease = 0.10f;

    public float penetrationPercentIncrease = 0.10f;
    public float reloadPercentReduction = 0.05f;
    public float speedPercentIncrease = 0.10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
          Destroy(gameObject);  
        }
    }
    

    // Universal Upgrades

    public void UpgradeSpeedFlat()
    {
        if (playerMovement == null)
        {
            Debug.LogWarning("UpgradeManager: Player Movement not assigned");
            return;
        }
        
        playerMovement.maxSpeed += speedFlatIncrease;

        Debug.Log(
            "Speed +Flat! New Max Speed: " + 
            playerMovement.maxSpeed
        );
    }

    public void UpgradeSpeedPercent()
    {
        if (playerMovement == null)
        {
            Debug.LogWarning("UpgradeManager: Player Movement not assigned");
            return;
        }
        
        playerMovement.maxSpeed *= 1f + speedPercentIncrease;

        Debug.Log(
            "Speed +%! New Max Speed: " + 
            playerMovement.maxSpeed
        );
    }

    public void UpgradeDamageFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.damage += damageFlatIncrease;
        playerWeapon.apDamage += damageFlatIncrease;
        playerWeapon.torpedoDamage += damageFlatIncrease;

        Debug.Log("Universal Damage +Flat!");
    }

    public void UpgradeDamagePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.damage *= 1f + damagePercentIncrease;
        playerWeapon.apDamage *= 1f + damagePercentIncrease;
        playerWeapon.torpedoDamage *= 1f + damagePercentIncrease;

        Debug.Log("Universal Damage +%!");
    }

    public void UpgradeReloadFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.reloadTime = Mathf.Max(
            0.1f,
            playerWeapon.reloadTime - reloadFlatReduction
        );

        playerWeapon.torpedoReloadTime = Mathf.Max(
            0.1f,
            playerWeapon.torpedoReloadTime - reloadFlatReduction
        );

        Debug.Log("Universal Reload +Flat!");
    }

    public void UpgradeReloadPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime = Mathf.Max(
            0.1f,
            playerWeapon.reloadTime * (1f - reloadPercentReduction)
        );

        playerWeapon.torpedoReloadTime = Mathf.Max(
            0.1f,
            playerWeapon.torpedoReloadTime * (1f - reloadPercentReduction)
        );
        
        Debug.Log("Universal Reload +%!");
    }

    public void UpgradeHealthFlat()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.maxHealth += healthFlatIncrease;
        
        Debug.Log(
            "Health +Flat! New Max Health: " + 
            playerShip.maxHealth
        );
        
    }

    public void UpgradeHealthPercent()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.maxHealth *= 1f + healthPercentIncrease;
        
        Debug.Log(
            "Health +%! New Max Health: " + 
            playerShip.maxHealth
        );
        
    }

    // Specific Upgrades

    public void UpgradeHEDamageFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.damage += heDamageFlatIncrease;

        Debug.Log(
            "HE Damage +FLat! New damage: " + 
            playerWeapon.damage
        );
    }

    public void UpgradeHEDamagePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.damage *= 1f + heDamagePercentIncrease;

        Debug.Log(
            "HE Damage +%! New damage: " + 
            playerWeapon.damage
        );
    }


    public void UpgradeAPDamageFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.apDamage += apDamageFlatIncrease;

        Debug.Log(
            "AP Damage +Flat! New damage: " + 
            playerWeapon.apDamage
        );
    }

    public void UpgradeAPDamagePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.apDamage *= 1f + apDamagePercentIncrease;

        Debug.Log(
            "AP Damage +%! New damage: " + 
            playerWeapon.apDamage
        );
    }

    public void UpgradeTorpedoDamageFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.torpedoDamage += torpedoDamageFlatIncrease;

        Debug.Log(
            "Torpedo Damage +Flat! New torpedo damage: " +
            playerWeapon.torpedoDamage
        );
    }

    public void UpgradeTorpedoDamagePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.torpedoDamage *= 1f + torpedoDamagePercentIncrease;

        Debug.Log(
            "Torpedo Damage +%! New torpedo damage: " +
            playerWeapon.torpedoDamage
        );
    }

    public void UpgradeAPPenetrationFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.penetration += penetrationFlatIncrease;

        Debug.Log(
            "AP Penetration +Flat! New penetration: " +
            playerWeapon.penetration
        );
    }

    public void UpgradeAPPenetrationPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.penetration *= 1f + penetrationPercentIncrease;

        Debug.Log(
            "AP Penetration +%! New penetration: " +
            playerWeapon.penetration
        );
    }

    public void UpgradeArmorFlat()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Ship not assigned");
            return;
        }

        playerShip.armor += armorFlatIncrease;

        Debug.Log(
            "Armor +Flat! New armor: " +
            playerShip.armor
        );
    }

    public void UpgradeArmorPercent()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Ship not assigned");
            return;
        }

        playerShip.armor *= 1f + armorPercentIncrease;

        Debug.Log(
            "Armor +%! New armor: " +
            playerShip.armor
        );
    }

    public void UpgradeWeaponReloadFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime = Mathf.Max(
            0.1f,
            playerWeapon.reloadTime - reloadFlatReduction
        );

        Debug.Log(
            "Weapon +Flat! New reload time: " + 
            playerWeapon.reloadTime
        );
    }

    public void UpgradeWeaponReloadPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime = Mathf.Max(
            0.1f,
            playerWeapon.reloadTime * (1f - reloadPercentReduction)
        );

        Debug.Log(
            "Weapon +%! New reload time: " + 
            playerWeapon.reloadTime
        );
    }

    public void UpgradeTorpedoReloadFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.torpedoReloadTime = Mathf.Max(
            0.1f,
            playerWeapon.torpedoReloadTime - reloadFlatReduction
        );
        
        Debug.Log(
            "Torpedo +Flat! New reload time: " + 
            playerWeapon.torpedoReloadTime
        );
    }

    public void UpgradeTorpedoReloadPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.torpedoReloadTime = Mathf.Max(
            0.1f,
            playerWeapon.torpedoReloadTime * (1f - reloadPercentReduction)
        );
        
        Debug.Log(
            "Torpedo +%! New reload time: " + 
            playerWeapon.torpedoReloadTime
        );
    }
}