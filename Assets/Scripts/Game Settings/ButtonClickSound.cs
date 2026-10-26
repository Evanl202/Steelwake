
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private Button button;
    private AudioSource audioSource;

    private void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;

        button.onClick.AddListener(PlayClick);
    }

    private void PlayClick()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClick);
        }
    }
}
