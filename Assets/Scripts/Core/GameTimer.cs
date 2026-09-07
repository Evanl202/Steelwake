using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header ("Timer")]
    public float elapsedTime = 0f;

    [Header ("UI")]
    public TMP_Text timerText;

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }
        
        elapsedTime += Time.deltaTime;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes, seconds
        );
    }
}