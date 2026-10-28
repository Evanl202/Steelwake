
using UnityEngine;

public class GameAudioSFX : MonoBehaviour
{
    public static GameAudioSFX Instance { get; private set; }

    [Header("Pickup Sounds")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip xpPickupSound;

    [Header("Ship Destruction Sounds")]
    [SerializeField] private AudioClip enemyDestroyedSound;
    [SerializeField] private AudioClip playerDestroyedSound;

    [Header("Ship Hit Sounds")]
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip enemyHitSound;

    private float nextPlayerHitSoundTime;
    private float nextEnemyHitSoundTime;

    [SerializeField] private float hitSoundCooldown = 0.15f;

    [Header("Weapon Sounds")]
    [SerializeField] private AudioClip playerGunFireSound;
    [SerializeField] private AudioClip enemyGunFireSound;
    
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }
    public void PlayPlayerHit()
    {
        if (Time.time < nextPlayerHitSoundTime)
            return;

        nextPlayerHitSoundTime = Time.time + hitSoundCooldown;
        PlaySound(playerHitSound);
    }

    public void PlayEnemyHit()
    {
        if (Time.time < nextEnemyHitSoundTime)
            return;

        nextEnemyHitSoundTime = Time.time + hitSoundCooldown;
        PlaySound(enemyHitSound);
    }

    public void PlayPlayerGunFire()
    {
        PlaySound(playerGunFireSound);
    }

    public void PlayEnemyGunFire()
    {
        PlaySound(enemyGunFireSound);
    }

    public void PlayPickup()
    {
        PlaySound(pickupSound);
    }

    public void PlayXPPickup()
    {
        PlaySound(xpPickupSound);
    }

    public void PlayEnemyDestroyed()
    {
        PlaySound(enemyDestroyedSound);
    }

    public void PlayPlayerDestroyed()
    {
        PlaySound(playerDestroyedSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
