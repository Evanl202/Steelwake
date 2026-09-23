using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    public EnemyShip boss;

    private void Update()
    {
        if (boss == null)
            return;

        if (healthSlider == null || healthText == null)
            return;

        UpdateHealthBar();
    }

    public void SetBoss(EnemyShip newBoss)
    {
        boss = newBoss;
    }

    private void UpdateHealthBar()
    {
        float currentHealth = boss.currentHealth;
        float maxHealth = boss.maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthText.text =
            Mathf.CeilToInt(currentHealth) +
            " / " +
            Mathf.CeilToInt(maxHealth);
    }
}