using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header ("UI")]
    public TMP_Text weaponName;
    public TMP_Text ammoType;
    public TMP_Text reloadText;
    public Slider reloadBar;

    private Weapon weapon;

    private void Update()
    {
        FindActiveWeapon();

        if (weapon == null)
            return;
        
        UpdateWeaponInfo();
        UpdateReload();
    }

    private void FindActiveWeapon()
    {
        PlayerShip playerShip = FindAnyObjectByType<PlayerShip>();

        if (playerShip == null)
        {
            weapon = null;
            return;
        }

        Weapon playerWeapon = playerShip.GetComponentInChildren<Weapon>();

        if (playerWeapon != null)
        {
            weapon = playerWeapon;
        }
        else
        {
            weapon = null;
        }
    }
    
    private void UpdateWeaponInfo()
    {
        weaponName.text = "MAIN BATTERY";
        ammoType.text = "AMMO: HE";
    }

    private void UpdateReload()
    {
        float reloadTime = weapon.reloadTime;
        float remainingTime = weapon.ReloadTimer;

        if (remainingTime <= 0f)
        {
            reloadText.text = "RELOAD: READY";
            reloadBar.value = 1f;
        }
        else
        {
            reloadText.text = "RELOAD: " + remainingTime.ToString("0.0") + "s";

            float progress = 1f - (remainingTime / reloadTime);
            reloadBar.value = progress;
        }
    }
}