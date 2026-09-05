using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    public PlayerShip player;

    private void start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        if (player == null)
            return;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
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