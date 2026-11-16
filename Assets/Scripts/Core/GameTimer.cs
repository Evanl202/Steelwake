using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header ("Timer")]
    public float elapsedTime = 0f;

    [Header ("UI")]
    public TMP_Text timerText;
    private int lastDisplayedSecond = -1;

    private void Update()
    {
        if (GameManager.Instance != null &&
            !GameManager.Instance.gameRunning)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(elapsedTime);

        if (currentSecond != lastDisplayedSecond)
        {
            UpdateTimerText();
            lastDisplayedSecond = currentSecond;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes, seconds
        );
    }
}