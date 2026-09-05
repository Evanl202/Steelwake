using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    private PlayerShip player;

    private void start()
    {
        player = FindFirstObjectByType<PlayerShip>();
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateHealthBarUI();
    }

    private void UpdateHealthBarUI ()
    {
        float currentHealth = player.CurrentHealth;
        float maxHealth = player.maxHealth;

        //Update bar
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        //Update text
        healthText.text =
            Mathf.CeilToInt(currentHealth) + " / " + Mathf.CeilToInt(maxHealth);
        
    }
}