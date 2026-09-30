using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifetime = 0.75f;
    public float moveSpeed = 1f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position +=
            Vector3.up * moveSpeed * Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        damageText.text = Mathf.CeilToInt(damage).ToString();
    }
}