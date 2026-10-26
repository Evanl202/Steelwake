
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolume(MasterKey, "MasterVolume");
        LoadVolume(MusicKey, "MusicVolume");
        LoadVolume(SFXKey, "SFXVolume");
    }

    public void SetMasterVolume(float value)
    {
        SetVolume(MasterKey, "MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(MusicKey, "MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(SFXKey, "SFXVolume", value);
    }

    private void SetVolume(string saveKey, string mixerParameter, float value)
    {
        value = Mathf.Clamp01(value);

        // A slider value of 0 means effectively silent.
        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat(mixerParameter, decibels);
        }

        PlayerPrefs.SetFloat(saveKey, value);
        PlayerPrefs.Save();
    }

    private void LoadVolume(string saveKey, string mixerParameter)
    {
        float value = PlayerPrefs.GetFloat(saveKey, 1f);

        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat(mixerParameter, decibels);
        }
    }

}
