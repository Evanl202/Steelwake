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

    [Header("Torpedo UI")]
    public TMP_Text torpedoName;
    public TMP_Text torpedoReloadText;
    public Slider torpedoReloadBar;

    private void Update()
    {
        FindActiveWeapon();

        if (weapon == null)
            return;
        
        UpdateWeaponInfo();
        UpdateReload();
        UpdateTorpedoReload();
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

    private void UpdateTorpedoReload()
    {
        // Ship has no torpedo
        if (weapon.torpedoPrefab == null ||
            weapon.torpedoReloadTime <= 0f)
        {
            torpedoName.text = "TORPEDO";
            torpedoReloadText.text = "NO TORPEDOES";
            torpedoReloadBar.value = 0f;
            return;
        }

        float reloadTime = weapon.torpedoReloadTime;
        float remainingTime = weapon.TorpedoReloadTimer;

        torpedoName.text = "TORPEDO";

        if (remainingTime <= 0f)
        {
            torpedoReloadText.text = "RELOAD: READY";
            torpedoReloadBar.value = 1f;
        }
        else
        {
            torpedoReloadText.text =
                "RELOAD: " + remainingTime.ToString("0.0") + "s";

            float progress = 1f - (remainingTime / reloadTime);

            torpedoReloadBar.value = progress;
        }
    }
}