using UnityEngine;
using TMPro;

public class WaveAnnouncementUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI announcementText;

    public void ShowWave(int wave)
    {
        if (panel == null || announcementText == null)
        {
            Debug.LogWarning(
                "WaveAnnouncementUI: Panel or Announcement Text is not assigned!",
                this
            );

            return;
        }

        panel.SetActive(true);
        announcementText.text = "WAVE " + wave;

        CancelInvoke(nameof(HideWave));
        Invoke(nameof(HideWave), 3f);
    }

    private void HideWave()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}