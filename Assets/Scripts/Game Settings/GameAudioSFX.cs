
using UnityEngine;

public enum ShipClass
{
    Destroyer,
    Cruiser,
    Battleship
}

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
    [SerializeField] private AudioClip[] destroyerGunSounds;
    [SerializeField] private AudioClip[] cruiserGunSounds;
    [SerializeField] private AudioClip[] battleshipGunSounds;
    
    [Header("Torpedo Sounds")]
    [SerializeField] private AudioClip torpedoLaunchSound;
    [SerializeField] private AudioClip underwaterExplosionSound;

    [Header("Level Up Sound")]
    [SerializeField] private AudioClip levelUpSound;

    [Header("Upgrade Sounds")]
    [SerializeField] private AudioClip upgradeClickSound;

    [Header("Gun Fire Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float playerGunVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float enemyGunVolume = 0.4f;

    [Header("Torpedo Sound Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float playerTorpedoVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float enemyTorpedoVolume = 0.4f;
    public void PlayUpgradeClick()
    {
        PlaySound(upgradeClickSound);
    }

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

    public void PlayPlayerGunFire(ShipClass shipClass)
    {
        PlayGunFire(shipClass, playerGunVolume);
    }

    public void PlayEnemyGunFire(ShipClass shipClass)
    {
        PlayGunFire(shipClass, enemyGunVolume);
    }

    private void PlayGunFire(ShipClass shipClass, float volume)
    {
        AudioClip[] sounds = shipClass switch
        {
            ShipClass.Destroyer => destroyerGunSounds,
            ShipClass.Cruiser => cruiserGunSounds,
            ShipClass.Battleship => battleshipGunSounds,
            _ => null
        };

        if (sounds == null || sounds.Length == 0)
            return;

        AudioClip clip = sounds[Random.Range(0, sounds.Length)];

        PlaySound(clip, volume);
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

    public void PlayLevelUp()
    {
        PlaySound(levelUpSound);
    }

    public void PlayTorpedoLaunch(bool isPlayer)
    {
        PlaySound(
            torpedoLaunchSound,
            isPlayer ? playerTorpedoVolume : enemyTorpedoVolume
        );
    }

    public void PlayUnderwaterExplosion(bool isPlayer)
    {
        PlaySound(
            underwaterExplosionSound,
            isPlayer ? playerTorpedoVolume : enemyTorpedoVolume
        );
    }

    public void PlayUnderwaterExplosion()
    {
        PlaySound(underwaterExplosionSound);
    }

    private void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
