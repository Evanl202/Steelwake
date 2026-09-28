using UnityEngine;

public class ShipAbilities : MonoBehaviour
{
    public enum AbilityType
    {
        None,
        Smoke,
        Repair
    }

    [Header("Ability")]
    public AbilityType ability = AbilityType.None;

    [Header("Smoke")]
    public float smokeDuration = 8f;
    public float smokeCooldown = 20f;

    [Header("Repair")]
    public float repairAmount = 50f;
    public float repairCooldown = 20f;

    private float smokeCooldownTimer = 0f;
    private float repairCooldownTimer = 0f;

    private PlayerShip playerShip;

    public bool IsInSmoke { get; private set; }

    private void Update()
    {
        if (smokeCooldownTimer > 0f)
        {
            smokeCooldownTimer -= Time.deltaTime;
        }

        if (repairCooldownTimer > 0f)
        {
            repairCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseAbility();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseRepair();
        }
    }

    private void UseRepair()
    {
        if (ability != AbilityType.Repair)
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

        playerShip.RepairHalf();

        repairCooldownTimer = repairCooldown;

        Debug.Log("Repair activated!");
    }

    private void UseSmoke()
    {
        if (ability == AbilityType.Smoke)
        {
            UseSmoke();
        }

        if (smokeCooldownTimer > 0f)
        {
            return;
        }

        smokeCooldownTimer = smokeCooldown;
        IsInSmoke = true;

        Debug.Log("Smoke activated!");

        StartCoroutine(SmokeRoutine());
    }

    private System.Collections.IEnumerator SmokeRoutine()
    {
        yield return new WaitForSeconds(smokeDuration);

        IsInSmoke = false;

        Debug.Log("Smoke ended!");
    }
}