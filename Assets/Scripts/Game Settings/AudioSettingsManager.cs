using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance { get; private set; }

    [Header("Audio Mixer")]
    private AudioMixer audioMixer;

    [Header("Current Scene Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

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
        FindAudioMixer();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadVolumeSettings();
        ConnectSliders();
    }

    private void FindAudioMixer()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/Mixer/MainAudioMixer");

        if (audioMixer == null)
        {
            Debug.LogError("Could not find MainAudioMixer in Resources!");
        }
        else
        {
            Debug.Log("Audio Mixer found successfully!");
        }
    }

    private void ConnectSliders()
    {
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMasterVolume(float value)
    {
        Debug.Log("MASTER SLIDER CALLED: " + value);
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

        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        if (audioMixer == null)
        {
            Debug.LogError("Audio Mixer is not assigned!");
        }
        else
        {
            bool success = audioMixer.SetFloat(mixerParameter, decibels);

            Debug.Log(
                mixerParameter + " | Success: " + success +
                " | Volume: " + value +
                " | Decibels: " + decibels
            );
        }

        PlayerPrefs.SetFloat(saveKey, value);
        PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {
        float master = PlayerPrefs.GetFloat(MasterKey, 1f);
        float music = PlayerPrefs.GetFloat(MusicKey, 1f);
        float sfx = PlayerPrefs.GetFloat(SFXKey, 1f);

        ApplyVolume("MasterVolume", master);
        ApplyVolume("MusicVolume", music);
        ApplyVolume("SFXVolume", sfx);

        if (masterSlider != null)
            masterSlider.SetValueWithoutNotify(master);

        if (musicSlider != null)
            musicSlider.SetValueWithoutNotify(music);

        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(sfx);
    }

    private void ApplyVolume(string mixerParameter, float value)
    {
        value = Mathf.Clamp01(value);

        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        if (audioMixer != null)
        {
            audioMixer.SetFloat(mixerParameter, decibels);
        }
    }
}
