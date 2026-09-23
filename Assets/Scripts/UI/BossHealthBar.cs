using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;

    private  EnemyShip boss;

    private void Update()
    {
        FindBoss()

        if (boss == null)
        {
            healthSlider.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            return;
        }

        healthSlider.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);

        UpdateHealthBar();
    }

    private void FindBoss()
    {
        GameObject bossObject =
            GameObject.FindGameObjectWithTag("Boss");

        if (bossObject != null)
        {
            boss = bossObject.GetComponent<EnemyShip>();
        }
        else
        {
            boss = null;
        }
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