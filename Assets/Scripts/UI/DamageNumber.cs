using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifetime = 0.75f;
    public float moveSpeed = 60f;

    [Header("Spread")]
    public float horizontalSpread = 35f;
    public float verticalSpread = 15f;

    private float timer;
    private RectTransform rectTransform;
    private Vector2 movementDirection;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Give each damage number a slightly different direction.
        movementDirection = new Vector2(
            Random.Range(-0.5f, 0.5f),
            1f
        ).normalized;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition +=
                movementDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position +=
                (Vector3)(movementDirection * moveSpeed * Time.deltaTime);
        }

        if (timer >= lifetime)
            Destroy(gameObject);
    }

    public void SetDamage(float damage, bool isCritical = false)
    {
        if (damageText == null)
            return;

        string damageValue = Mathf.CeilToInt(damage).ToString();

        if (isCritical)
        {
            damageText.text = "CRIT! " + damageValue;
            damageText.color = Color.yellow;
            damageText.fontStyle = FontStyles.Bold;
        }
        else
        {
            damageText.text = damageValue;
        }
    }

    public Vector2 GetRandomOffset()
    {
        return new Vector2(
            Random.Range(-horizontalSpread, horizontalSpread),
            Random.Range(-verticalSpread, verticalSpread)
        );
    }
}