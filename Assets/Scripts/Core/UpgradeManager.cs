using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header ("Player")]
    private ShipMovement playerMovement;
    private PlayerShip playerShip;
    private Weapon playerWeapon;

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

    public float gunRangeFlatIncrease = 5f;
    public float shellSpeedFlatIncrease = 3f;
    public float torpedoRangeFlatIncrease = 5f;
    public float torpedoSpeedFlatIncrease = 3f;

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

    public float gunRangePercentIncrease = 0.10f;
    public float shellSpeedPercentIncrease = 0.10f;
    public float torpedoRangePercentIncrease = 0.10f;
    public float torpedoSpeedPercentIncrease = 0.10f;

    [Header("Ability Upgrades")]
    public float smokeDurationIncrease = 1f;
    public float smokeCooldownReduction = 1f;

    public float repairPercentIncrease = 0.05f;
    public float repairCooldownReduction = 1.5f;

    private ShipAbilities playerAbilities;

    // Base stats used for upgrade caps
    private float baseMaxSpeed;
    private float baseMaxHealth;
    private float baseArmor;
    private float baseGunRange;
    private float baseTorpedoRange;

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

    private void Start()
    {
        Invoke(nameof(FindPlayer), 0.1f);
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerMovement = player.GetComponent<ShipMovement>();
            playerShip = player.GetComponent<PlayerShip>();
            playerWeapon = player.GetComponentInChildren<Weapon>();
            playerAbilities = player.GetComponent<ShipAbilities>();

            // Save the original stats before any upgrades are applied
            if (playerMovement != null)
            {
                baseMaxSpeed = playerMovement.maxSpeed;
            }

            if (playerShip != null)
            {
                baseMaxHealth = playerShip.maxHealth;
                baseArmor = playerShip.armor;
            }

            if (playerWeapon != null)
            {
                baseGunRange = playerWeapon.gunRange;

                if (playerWeapon.torpedoPrefab != null)
                {
                    baseTorpedoRange = playerWeapon.torpedoRange;
                }
            }
        }
        else
        {
            Debug.LogWarning("UpgradeManager: No Player found.");
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
        
        float speedCap = baseMaxSpeed * 2f;

        playerMovement.maxSpeed = Mathf.Min(
            playerMovement.maxSpeed + speedFlatIncrease,
            speedCap
        );

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
        
        float speedCap = baseMaxSpeed * 2f;

        playerMovement.maxSpeed = Mathf.Min(
            playerMovement.maxSpeed * (1f + speedPercentIncrease),
            speedCap
        );

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
            0.5f,
            playerWeapon.reloadTime - reloadFlatReduction
        );

        playerWeapon.torpedoReloadTime = Mathf.Max(
            1f,
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
            0.5f,
            playerWeapon.reloadTime * (1f - reloadPercentReduction)
        );

        playerWeapon.torpedoReloadTime = Mathf.Max(
            1f,
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
        
        float healthCap = baseMaxHealth * 10f;

        playerShip.maxHealth = Mathf.Min(
            playerShip.maxHealth + healthFlatIncrease,
            healthCap
        );

        
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
        
        float healthCap = baseMaxHealth * 10f;

        playerShip.maxHealth = Mathf.Min(
            playerShip.maxHealth * (1f + healthPercentIncrease),
            healthCap
        );
        
        Debug.Log(
            "Health +%! New Max Health: " + 
            playerShip.maxHealth
        );
    }

    public void UpgradeGunRangeFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        float rangeCap = baseGunRange * 2f;

        playerWeapon.gunRange = Mathf.Min(
            playerWeapon.gunRange + gunRangeFlatIncrease,
            rangeCap
        );

        Debug.Log(
            "Main Battery Range +Flat! New range: " +
            playerWeapon.gunRange
        );
    }

    public void UpgradeGunRangePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        float rangeCap = baseGunRange * 2f;

        playerWeapon.gunRange = Mathf.Min(
            playerWeapon.gunRange * (1f + gunRangePercentIncrease),
            rangeCap
        );

        Debug.Log(
            "Main Battery Range +%! New range: " +
            playerWeapon.gunRange
        );
    }

    public void UpgradeShellSpeedFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.shellSpeed = Mathf.Min(
            playerWeapon.shellSpeed + shellSpeedFlatIncrease,
            50f
        );

        Debug.Log(
            "Shell Speed +Flat! New speed: " +
            playerWeapon.shellSpeed
        );
    }

    public void UpgradeShellSpeedPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.shellSpeed = Mathf.Min(
            playerWeapon.shellSpeed * (1f + shellSpeedPercentIncrease),
            50f
        );

        Debug.Log(
            "Shell Speed +%! New speed: " +
            playerWeapon.shellSpeed
        );
    }

    public void UpgradeTorpedoRangeFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        float rangeCap = baseTorpedoRange * 2f;

        playerWeapon.torpedoRange = Mathf.Min(
            playerWeapon.torpedoRange + torpedoRangeFlatIncrease,
            rangeCap
        );

        Debug.Log(
            "Torpedo Range +Flat! New range: " +
            playerWeapon.torpedoRange
        );
    }

    public void UpgradeTorpedoRangePercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        float rangeCap = baseTorpedoRange * 2f;

        playerWeapon.torpedoRange = Mathf.Min(
            playerWeapon.torpedoRange * (1f + torpedoRangePercentIncrease),
            rangeCap
        );

        Debug.Log(
            "Torpedo Range +%! New range: " +
            playerWeapon.torpedoRange
        );
    }

    public void UpgradeTorpedoSpeedFlat()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.torpedoSpeed = Mathf.Min(
            playerWeapon.torpedoSpeed + torpedoSpeedFlatIncrease,
            50f
        );

        Debug.Log(
            "Torpedo Speed +Flat! New speed: " +
            playerWeapon.torpedoSpeed
        );
    }

    public void UpgradeTorpedoSpeedPercent()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }

        playerWeapon.torpedoSpeed = Mathf.Min(
            playerWeapon.torpedoSpeed * (1f + torpedoSpeedPercentIncrease),
            50f
        );

        Debug.Log(
            "Torpedo Speed +%! New speed: " +
            playerWeapon.torpedoSpeed
        );
    }

    // Repair Ship
    public void RepairFlat()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.RepairFlat();
        
        Debug.Log("Ship Repaired +Flat!");
    }
    
    public void RepairHalf()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.RepairHalf();
        
        Debug.Log("Ship Repaired +Half!");
    }

    public void RepairFull()
    {
        if (playerShip == null)
        {
            Debug.LogWarning("UpgradeManager: Player Health not assigned");
            return;
        }
        
        playerShip.RepairFull();
        
        Debug.Log("Ship Repaired +Full!");
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

        float armorCap = baseArmor * 10f;

        playerShip.armor = Mathf.Min(
            playerShip.armor + armorFlatIncrease,
            armorCap
        );

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

        float armorCap = baseArmor * 10f;

        playerShip.armor = Mathf.Min(
            playerShip.armor * (1f + armorPercentIncrease),
            armorCap
        );

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
            0.5f,
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
            0.5f,
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
            1f,
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
            1f,
            playerWeapon.torpedoReloadTime * (1f - reloadPercentReduction)
        );
        
        Debug.Log(
            "Torpedo +%! New reload time: " + 
            playerWeapon.torpedoReloadTime
        );
    }

    // Ability Upgrades
    public void UpgradeSmokeDuration()
    {
        if (playerAbilities == null)
        {
            Debug.LogWarning("UpgradeManager: ShipAbilities not assigned.");
            return;
        }

        playerAbilities.smokeDuration = Mathf.Min(
            15f,
            playerAbilities.smokeDuration + smokeDurationIncrease
        );

        Debug.Log(
            "Smoke Duration upgraded! New duration: " +
            playerAbilities.smokeDuration
        );
    }

    public void UpgradeSmokeCooldown()
    {
        if (playerAbilities == null)
        {
            Debug.LogWarning("UpgradeManager: ShipAbilities not assigned.");
            return;
        }

        playerAbilities.smokeCooldown = Mathf.Max(
            10f,
            playerAbilities.smokeCooldown - smokeCooldownReduction
        );

        Debug.Log(
            "Smoke Cooldown upgraded! New cooldown: " +
            playerAbilities.smokeCooldown
        );
    }

    public void UpgradeRepairAmount()
    {
        if (playerAbilities == null)
        {
            Debug.LogWarning("UpgradeManager: ShipAbilities not assigned.");
            return;
        }

        playerAbilities.repairPercent = Mathf.Min(
            0.75f,
            playerAbilities.repairPercent + repairPercentIncrease
        );

        Debug.Log(
            "Repair Amount upgraded! New repair: " +
            (playerAbilities.repairPercent * 100f) +
            "%"
        );
    }

    public void UpgradeRepairCooldown()
    {
        if (playerAbilities == null)
        {
            Debug.LogWarning("UpgradeManager: ShipAbilities not assigned.");
            return;
        }

        playerAbilities.repairCooldown = Mathf.Max(
            45f,
            playerAbilities.repairCooldown - repairCooldownReduction
        );

        Debug.Log(
            "Repair Cooldown upgraded! New cooldown: " +
            playerAbilities.repairCooldown
        );
    }
}