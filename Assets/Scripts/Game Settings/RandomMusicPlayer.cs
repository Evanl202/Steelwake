
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomMusicPlayer : MonoBehaviour
{
    [Header("Gameplay Music")]
    [SerializeField] private AudioClip[] musicTracks;

    private AudioSource musicSource;
    private int previousTrack = -1;

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = false;
    }

    private void Start()
    {
        PlayRandomTrack();
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0)
            return;

        int nextTrack;

        if (musicTracks.Length == 1)
        {
            nextTrack = 0;
        }
        else
        {
            do
            {
                nextTrack = Random.Range(0, musicTracks.Length);
            }
            while (nextTrack == previousTrack);
        }

        previousTrack = nextTrack;

        musicSource.clip = musicTracks[nextTrack];
        musicSource.Play();
    }
}
