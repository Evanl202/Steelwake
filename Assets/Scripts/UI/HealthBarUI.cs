using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public PlayerShip player;
    public Slider healthSlider;
    public TMP_Text healthText;

    private void Start()
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