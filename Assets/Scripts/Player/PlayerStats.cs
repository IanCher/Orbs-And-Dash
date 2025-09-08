using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Use eventing to broadcast the player state to other game objects
    public static event Action<PlayerStats> OnPlayerStatsReady;
    public static event Action OnPlayerStatsDestroyed;
    
    public float speedMultiplier = 1f;
    [SerializeField] private float baseSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 50.0f;
    public float MaxSpeed => maxSpeed;

    [SerializeField] private float timeBasedAcceleration = 1.0f;
    [SerializeField] private float speedPerOrb = 0.5f;
    [SerializeField] GameObject shieldVFX;
    
    public int activeInvulnerabilityCount = 0;

    public bool IsInvulnerable => activeInvulnerabilityCount > 0;
    public float currentSpeed;
    
    void Start()
    {
        currentSpeed = baseSpeed;
        // Start fires after all Awake methods have been called,
        // so we can safely assume all placed components are initialized.
        // Any dynamically spawned components would need to read it from level with 
        // FindObjectsByType<PlayerStats>(FindObjectsSortMode.None)
        // Or we could make PlayerStats a singleton to avoid the FindObjectsByType call in that case.
        OnPlayerStatsReady?.Invoke(this);

        PotionBomb.OnCollidedWithPotion += ApplyPotionEffects;
        Shield.OnCollidedWithShield += ApplyShieldEffects;
        OrbCounterManager.OnOrbCollected += OrbCounterManagerOnOnOrbCollected;
    }

    private void OrbCounterManagerOnOnOrbCollected(float speedGain)
    {
        UpdateOrbSpeed(speedGain);
    }
    
    public float GetCurrentSpeed()
    {
        return currentSpeed * speedMultiplier;
    }
    public float GetSpeedRatio()
    {
        return currentSpeed / maxSpeed;
    }

    public void UpdateSpeed()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += Time.deltaTime * timeBasedAcceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
    }
    
    private void UpdateOrbSpeed(float speedGain)
    {
        currentSpeed = Mathf.Clamp(currentSpeed * (1 + speedGain), baseSpeed, maxSpeed);
    }

    void ApplyPotionEffects(PotionData potionData)
    {
        if (IsInvulnerable)
        {
            Debug.Log("Ignoring effect of slow down potion since player is invulnerable to damage and de-buffs");
            activeInvulnerabilityCount--;
            if (activeInvulnerabilityCount <= 0)
            {
                shieldVFX.SetActive(false);
            }
            return;
        }

        // float reduceSpeedMultiplierBy = Mathf.Clamp01(potionData.PercentToSlowBy / 100f);

        currentSpeed *= (100 - potionData.PercentToSlowBy) / 100f;
        currentSpeed = Mathf.Max(currentSpeed, baseSpeed);
        // speedMultiplier = 1f - reduceSpeedMultiplierBy;
        // StopCoroutine(nameof(ResetSpeedAfter));
        // StartCoroutine(ResetSpeedAfter(potionData.SlowDuration));
    }

    void ApplyShieldEffects(ShieldData shieldData)
    {
        activeInvulnerabilityCount += shieldData.value;
        shieldVFX.SetActive(true);

// #if UNITY_EDITOR
//         Debug.Log($"Shield applied with duration: {shieldData.DurationSeconds} seconds; activeInvulnerabilityCount={activeInvulnerabilityCount}");
// #endif

//         // Stop any existing shield coroutines to reset the timer
//         StopCoroutine(nameof(RemoveInvulnerabilityAfter));
//         StartCoroutine(RemoveInvulnerabilityAfter(shieldData.DurationSeconds));
    }

    IEnumerator ResetSpeedAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        speedMultiplier = 1f;
    }

    IEnumerator RemoveInvulnerabilityAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        activeInvulnerabilityCount = Mathf.Max(0, activeInvulnerabilityCount - 1);
        shieldVFX.SetActive(false);

#if UNITY_EDITOR
        Debug.Log($"Shield effect expired: activeInvulnerabilityCount={activeInvulnerabilityCount}");
#endif
    }
    
    void OnDestroy()
    {
        PotionBomb.OnCollidedWithPotion -= ApplyPotionEffects;
        Shield.OnCollidedWithShield -= ApplyShieldEffects;

        OnPlayerStatsDestroyed?.Invoke();
    }

}
