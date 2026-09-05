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
    public float healthIncrease = 20f;

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
        if (playerMovement == null)
        {
            Debug.LogWarning("UpgradeManager: Player # not assigned");
            return;
        }
        
        // playerMovement.maxSpeed *= 1f + speedIncrease;

        Debug.Log("Damage Upgrade! New damage: " );
    }

    public void UpgradeReload()
    {
        if (playerWeapon == null)
        {
            Debug.LogWarning("UpgradeManager: Player Weapon not assigned");
            return;
        }
        
        playerWeapon.reloadTime *= 1f - reloadReduction;
        
        Debug.Log("Weapon Upgrade! New reload time: " + playerWeapon.reloadTime);
        
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
}