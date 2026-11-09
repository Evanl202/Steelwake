
using System.Collections;
using TMPro;
using UnityEngine;

public class PickupNotificationUI : MonoBehaviour
{
    public static PickupNotificationUI Instance;

    [Header("UI References")]
    [SerializeField] private CanvasGroup notificationGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine notificationRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        notificationGroup.alpha = 0f;
        notificationGroup.gameObject.SetActive(false);
    }

    public void ShowNotification(string title, string description)
    {
        if (notificationGroup == null ||
            titleText == null ||
            descriptionText == null)
            return;

        titleText.text = title;
        descriptionText.text = description;

        notificationGroup.gameObject.SetActive(true);

        if (notificationRoutine != null)
        {
            StopCoroutine(notificationRoutine);
        }

        notificationRoutine = StartCoroutine(DisplayNotification());
    }

    private IEnumerator DisplayNotification()
    {
        float timer = 0f;

        // Fade in
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            notificationGroup.alpha =
                Mathf.Clamp01(timer / fadeDuration);

            yield return null;
        }

        notificationGroup.alpha = 1f;

        // Stay visible
        yield return new WaitForSecondsRealtime(displayDuration);

        // Fade out
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            notificationGroup.alpha =
                1f - Mathf.Clamp01(timer / fadeDuration);

            yield return null;
        }

        notificationGroup.alpha = 0f;
        notificationGroup.gameObject.SetActive(false);

        notificationRoutine = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
