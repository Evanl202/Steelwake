
using UnityEngine;

public class PickupEffects : MonoBehaviour
{
    public bool IsInvulnerable { get; private set; }
    public float DamageMultiplier { get; private set; } = 1f;
    public float ReloadMultiplier { get; private set; } = 1f;

    private float damageTimer;
    private float reloadTimer;
    private float invulnerabilityTimer;

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
            return;

        damageTimer -= Time.deltaTime;
        attackSpeedTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;
        invulnerabilityTimer -= Time.deltaTime;

        if (damageTimer <= 0f)
            DamageMultiplier = 1f;

        if (attackSpeedTimer <= 0f)
            AttackSpeedMultiplier = 1f;

        if (reloadTimer <= 0f)
            ReloadMultiplier = 1f;

        if (invulnerabilityTimer <= 0f)
            IsInvulnerable = false;
    }

    public void ActivateDamageBoost()
    {
        DamageMultiplier = 1.5f;
        damageTimer = 10f;
    }

    public void ActivateReloadBoost()
    {
        ReloadMultiplier = 0.6f;
        reloadTimer = 10f;
    }

    public void ActivateInvulnerability()
    {
        IsInvulnerable = true;
        invulnerabilityTimer = 5f;
    }
    
}