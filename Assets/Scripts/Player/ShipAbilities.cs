using UnityEngine;

public class ShipAbilities : MonoBehaviour
{
    public enum AbilityType
    {
        None,
        Smoke,
        Repair
    }

    [Header ("Abilities")]
    public bool hasSmoke = false;
    public bool hasRepair = false;

    [Header ("Smoke")]
    public float smokeDuration = 8f;
    public float smokeCooldown = 20f;
    public ParticleSystem smokeEffect;

    [Header ("Repair")]
    public float repairCooldown = 60;
    public float repairPercent = 0.25f;

    private float smokeCooldownTimer = 0f;
    private float repairCooldownTimer = 0f;
    private float smokeDurationTimer = 0f;

    [Header ("Repair Party")]
    public bool hasRepairParty = false;

    public float repairPartyCooldown = 60f;
    private float repairPartyCooldownTimer = 0f;

    private PlayerShip playerShip;

    public bool IsInSmoke { get; private set; }

    private void Start()
    {
        playerShip = GetComponent<PlayerShip>();
    }

    private void Update()
    {
        if (smokeCooldownTimer > 0f)
        {
            smokeCooldownTimer -= Time.deltaTime;
        }

        if (smokeDurationTimer > 0f)
        {
            smokeDurationTimer -= Time.deltaTime;
        }

        if (repairCooldownTimer > 0f)
        {
            repairCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseSmoke();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseRepair();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            UseRepairParty();
        }
    }

    private void UseSmoke()
    {
        if (!hasSmoke)
        {
            return;
        }

        if (smokeCooldownTimer > 0f)
        {
            return;
        }
        smokeCooldownTimer = smokeCooldown;

        smokeDurationTimer = smokeDuration;
        IsInSmoke = true;

        if (smokeEffect != null)
        {
            smokeEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            smokeEffect.Play();
        }

        Debug.Log("Smoke activated!");

        StartCoroutine(SmokeRoutine());
    }

    private void UseRepair()
    {
        if (!hasRepair)
        {
            return;
        }

        if (repairCooldownTimer > 0f)
        {
            return;
        }

        if (playerShip == null)
        {
            playerShip = GetComponent<PlayerShip>();
        }

        if (playerShip == null)
        {
            Debug.LogWarning("ShipAbilities: PlayerShip not found.");
            return;
        }

        playerShip.RepairPercent(repairPercent);

        repairCooldownTimer = repairCooldown;

        Debug.Log("Repair activated!");
    }

    private void UseRepairParty()
    {
        if (!hasRepairParty || repairPartyCooldownTimer > 0f)
            return;

        ShipFire fire = GetComponent<ShipFire>();

        if (fire != null && fire.IsBurning)
        {
            fire.Extinguish();
            Debug.Log("Repair Party extinguished fire!");
        }

        repairPartyCooldownTimer = repairPartyCooldown;
        Debug.Log("Repair Party activated!");
    }

    private System.Collections.IEnumerator SmokeRoutine()
    {
        yield return new WaitForSeconds(smokeDuration);

        smokeDurationTimer = 0f;
        IsInSmoke = false;

        if (smokeEffect != null)
        {
            smokeEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        Debug.Log("Smoke ended!");
    }

    public float GetSmokeCooldownRemaining()
    {
        return Mathf.Max(0f, smokeCooldownTimer);
    }

    public float GetRepairCooldownRemaining()
    {
        return Mathf.Max(0f, repairCooldownTimer);
    }

    public float GetSmokeDurationRemaining()
    {
        return Mathf.Max(0f, smokeDurationTimer);
    }

    public float GetRepairPercent()
    {
        return repairPercent;
    }

    public float GetRepairPartyCooldownRemaining()
    {
        return Mathf.Max(0f, repairPartyCooldownTimer);
    }
}