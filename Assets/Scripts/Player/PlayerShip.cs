using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 100f;

    [Header ("Armor")]
    public float armor = 40f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;

    private Renderer[] shipRenderers;
    private Color[] originalColors;

    private bool isFlashing = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Find all renderers on the player ship and its children
        shipRenderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[shipRenderers.Length];

        for (int i = 0; i < shipRenderers.Length; i++)
        {
            originalColors[i] =
                shipRenderers[i].material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        HitFlash();

        if (CombatFeedbackManager.Instance != null)
        {
            CombatFeedbackManager.Instance.ShowDamage(
                damage,
                transform.position
            );
        }

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        
    }

    public void ArmorBlockedFlash()
    {
        if (isFlashing)
            return;

        StartCoroutine(RedFlashRoutine());
    }

    private void HitFlash()
    {
        if (isFlashing)
            return;

        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        isFlashing = true;

        for (int i = 0; i < shipRenderers.Length; i++)
        {
            shipRenderers[i].material.color = Color.white;
        }

        yield return new WaitForSeconds(0.08f);

        for (int i = 0; i < shipRenderers.Length; i++)
        {
            shipRenderers[i].material.color = originalColors[i];
        }

        isFlashing = false;
    }

    private IEnumerator RedFlashRoutine()
    {
        isFlashing = true;

        for (int i = 0; i < shipRenderers.Length; i++)
        {
            shipRenderers[i].material.color = Color.red;
        }

        yield return new WaitForSeconds(0.08f);

        for (int i = 0; i < shipRenderers.Length; i++)
        {
            shipRenderers[i].material.color = originalColors[i];
        }

        isFlashing = false;
    }

    private void Die()
    {
        Debug.Log("Ship destroyed");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        
        Destroy(gameObject);
    }

    // Repairs
    public void RepairFlat()
    {
        currentHealth = Mathf.Min(currentHealth + 100f, maxHealth);
    }

    public void RepairHalf()
    {
        currentHealth = Mathf.Min(currentHealth + (maxHealth / 2f), maxHealth);
    }

    public void RepairFull()
    {
        currentHealth = maxHealth;
    }

    public void RepairPercent(float percent)
    {
        float repairAmount = maxHealth * percent;

        currentHealth = Mathf.Min(
            currentHealth + repairAmount,
            maxHealth
        );
    }
}