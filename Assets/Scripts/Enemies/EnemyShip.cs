using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [Header ("Health")]
    public float maxHealth = 50f;

    private float currentHealth;

    [Header ("Experience")]
    public int experienceReward = 10;
    
    [Header ("Movement")]
    public float moveSpeed = 3f;

    protected Transform player;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Enemy can't find Player");
        }
    }

    protected virtual void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }
            
        MoveTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        if (player == null)
            return;
        
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Destroyed");

        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.AddXP(
                experienceReward
            );
        }
        Destroy(gameObject);
    }
}