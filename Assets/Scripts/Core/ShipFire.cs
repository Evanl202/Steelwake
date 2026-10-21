
using System.Collections;
using UnityEngine;

public class ShipFire : MonoBehaviour
{
    [Range(0f, 1f)]
    public float damagePercentPerSecond = 0.02f;

    public float fireDuration = 15f;

    private bool isBurning = false;
    private Coroutine fireCoroutine;

    public bool IsBurning => isBurning;

    [Header("Fire Effect")]
    public GameObject fireEffectPrefab;

    private GameObject activeFireEffect;
    
    public bool TryStartFire(bool isAP, float ignitionChance)
    {
        // AP shells cannot start fires.
        if (isAP || isBurning)
        {
            return false;
        }

        if (Random.value > ignitionChance)
        {
            return false;
        }

        fireCoroutine = StartCoroutine(FireRoutine());

        if (activeFireEffect == null && fireEffectPrefab != null)
        {
            activeFireEffect = Instantiate(
                fireEffectPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );

            activeFireEffect.transform.localPosition =
                new Vector3(0f, 1f, 0f);

            // Starts firee
            ParticleSystem[] particles =
                activeFireEffect.GetComponentsInChildren<ParticleSystem>(true);

            foreach (ParticleSystem particle in particles)
            {
                particle.Play(true);
            }
        }

        return true;
    }


    private IEnumerator FireRoutine()
    {
        isBurning = true;

        float elapsed = 0f;

        while (elapsed < fireDuration)
        {
            yield return new WaitForSeconds(1f);

            elapsed += 1f;

            if (elapsed > fireDuration)
            {
                break;
            }

            PlayerShip player = GetComponent<PlayerShip>();

            if (player != null)
            {
                player.TakeDamage(
                    player.maxHealth * damagePercentPerSecond
                );
            }
            else
            {
                EnemyShip enemy = GetComponent<EnemyShip>();

                if (enemy != null)
                {
                    enemy.TakeDamage(
                        enemy.maxHealth * damagePercentPerSecond
                    );
                }
            }

            // Stop if the ship was destroyed.
            if (this == null || gameObject == null)
            {
                yield break;
            }
        }

        isBurning = false;
        fireCoroutine = null;
    }

    public void Extinguish()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        if (activeFireEffect != null)
        {
            Destroy(activeFireEffect);
            activeFireEffect = null;
        }

        isBurning = false;
    }
}
