using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 100f;

    [Header ("Armor")]
    public float armor = 40f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        
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

}