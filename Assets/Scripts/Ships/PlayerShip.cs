using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 100f;

    private float currentHealth;

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
        Debug.Log("Ship destroyed")

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        
        Destroy(gameObject);
    }

}