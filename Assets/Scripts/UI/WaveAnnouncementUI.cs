using UnityEngine;
using TMPro;

public class WaveAnnouncementUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI announcementText;

    public void ShowWave(int wave)
    {
        panel.SetActive(true);
        announcementText.text = "WAVE " + wave;

        CancelInvoke(nameof(HideWave));
        Invoke(nameof(HideWave), 3f);
    }

    private void HideWave()
    {
        panel.SetActive(false);
    }
}