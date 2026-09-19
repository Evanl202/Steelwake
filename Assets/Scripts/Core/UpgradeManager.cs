using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header ("Player")]
    public ShipMovement playerMovement;
    public PlayerShip playerShip;
    public Weapon playerWeapon;

    [Header ("Upgrade Amount")]
    public float speedIncrease = .10f;
    public float reloadReduction = .10f;
    public float healthIncrease = 50f;

    public float damageIncrease = .10f;
    public float penetrationIncrease = .10f;
    public float armorIncrease = .10f;

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
    public void UpgradeSpeed()
    {
        if (playerMovement == null)
        {
            Debug.LogWarning("UpgradeManager: Player Movement not assigned");
            return;
        }
        
        playerMovement.maxSpeed *= 1f + speedIncrease;

        Debug.Log("Speed Upgrade! New Max Speed: " + playerMovement.maxSpeed);
    }

    public void UpgradeDamage()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.damage *= 1f + damageIncrease;
        playerWeapon.apDamage *= 1f + damageIncrease;
        playerWeapon.torpedoDamage *= 1f + damageIncrease;

        Debug.Log("Universal Damage Upgrade!");
    }

    public void UpgradeReload()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime *= 1f - reloadReduction;
        playerWeapon.torpedoReloadTime *= 1f - reloadReduction;
        
        Debug.Log("Weapon Upgrade! New reload time: " + playerWeapon.reloadTime + " Torpedo: " + playerWeaon.torpedoReloadTime);
        
    }

    public void UpgradeHealth()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.maxHealth += healthIncrease;
        
        Debug.Log("Health Upgrade! New Max Health: " + playerShip.maxHealth);
        
    }

    // Specific Upgrades

    public void UpgradeHEDamage()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.damage *= 1f + (damageIncrease + .1f);

        Debug.Log("HE Damage Upgrade! New damage: " + playerWeapon.damage);
    }

    public void UpgradeAPDamage()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.apDamage *= 1f + (damageIncrease + .1f);

        Debug.Log("AP Damage Upgrade! New damage: " + playerWeapon.apDamage);
    }

    public void UpgradeTorpedoDamage()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.torpedoDamage *= 1f + (damageIncrease + .1f);

        Debug.Log(
            "Torpedo Damage Upgrade! New torpedo damage: " +
            playerWeapon.torpedoDamage
        );
    }

    public void UpgradeAPPenetration()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.penetration *= 1f + penetrationIncrease;

        Debug.Log(
            "AP Penetration Upgrade! New penetration: " +
            playerWeapon.penetration
        );
    }

    public void UpgradeArmor()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Ship not assigned");
            return;
        }

        playerShip.armor *= 1f + armorIncrease;

        Debug.Log(
            "Armor Upgrade! New armor: " +
            playerShip.armor
        );
    }


    public void UpgradeWeaponReload()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime *= 1f - (reloadReduction + .05f);
        
        Debug.Log("Weapon Upgrade! New reload time: " + playerWeapon.reloadTime);
        
    }

    public void UpgradeTorpedoReload()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.torpedoReloadTime *= 1f - (reloadReduction + .05f);
        
        Debug.Log("Torpedo Upgrade! New reload time: " + playerWeapon.torpedoReloadTime);
        
    }
}