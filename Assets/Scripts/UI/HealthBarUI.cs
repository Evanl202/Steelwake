using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    public PlayerShip player;

    private void Update()
    {
        FindActivePlayerShip();

        if (player == null)
            return;

        if (healthSlider == null || healthText == null)
            return;
        
        UpdateHealthBar();
    }

    private void FindActivePlayerShip()
    {
        PlayerShip activeShip = FindAnyObjectByType<PlayerShip>();

        if (activeShip != null)
        {
            player = activeShip;
        }
        else
        {
            player = null;
        }
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