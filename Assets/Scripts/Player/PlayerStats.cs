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
    // [SerializeField] private float speedPerOrb = 0.5f;
    [SerializeField] GameObject shieldVFX;

    public int activeInvulnerabilityCount = 0;

    public bool IsInvulnerable => activeInvulnerabilityCount > 0;
    public float currentSpeed;
    public bool Paralyzed { get; set; }

    [SerializeField] ParticleSystem orbCollectedVFX;

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
        ParalysingSpell.OnCollidedWithPotion += ApplyPotionEffects;
        //AccelerationPlatform.OnCollidedWithPotion += ApplyPotionEffects;

        Shield.OnCollidedWithShield += ApplyShieldEffects;
        OrbCounterManager.OnOrbCollected += OrbCounterManagerOnOnOrbCollected;
        MysterySpell.OnCollideWithMysterySpell += HandleMysterySpellEffect;
    }

    private void OrbCounterManagerOnOnOrbCollected(float speedGain,float lowOrbCount)
    {
        UpdateOrbSpeed(speedGain);
        ParticleSystem vfxInstance = Instantiate(orbCollectedVFX, transform);
        Destroy(vfxInstance.gameObject, 0.6f);
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
    void HandleMysterySpellEffect(MysterySpellEffect effect)
    {
        switch (effect.effectType)
        {
            case MysterySpellEffectType.Damage:
                ApplyPotionEffects(effect.damageData);
                break;
            case MysterySpellEffectType.Shield:
                ApplyShieldEffects(effect.shieldData);
                break;
        }
    }

    private void UpdateOrbSpeed(float speedGain)
    {
        currentSpeed = Mathf.Clamp(currentSpeed * (1 + speedGain), baseSpeed, maxSpeed);
    }

    void ApplyPotionEffects(PotionData potionData)
    {

        if (potionData != null && potionData.Type == EffectType.AccelerationPlatform)
        {
            speedMultiplier = potionData.Acceleration;
            StartCoroutine(ResetSpeedAfter(potionData.TimeOfAcceleration));
            return;
        }


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
        if (potionData.PercentToSlowBy > 90)// or equal 100 depending on how we want to handle it
        {
            Paralyzed = true;
            jumpNeeded = potionData.JumpNeeded;//Hardcoded for now, can be part of potion data later?
        }
        else
        {
            Paralyzed = false;
        }

        //currentSpeed = Mathf.Max(currentSpeed, baseSpeed);
        // speedMultiplier = 1f - reduceSpeedMultiplierBy;
        // StopCoroutine(nameof(ResetSpeedAfter));
        // StartCoroutine(ResetSpeedAfter(potionData.SlowDuration));
    }

    private int jumpNeeded = 0;
    public void Jumped()
    {
        if (Paralyzed)
        {
            jumpNeeded--;
            if (jumpNeeded <= 0)
            {
                Paralyzed = false;
                currentSpeed = Mathf.Max(currentSpeed, baseSpeed);
            }
        }

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
        ParalysingSpell.OnCollidedWithPotion -= ApplyPotionEffects;
        Shield.OnCollidedWithShield -= ApplyShieldEffects;
        MysterySpell.OnCollideWithMysterySpell -= HandleMysterySpellEffect;

        OnPlayerStatsDestroyed?.Invoke();
    }

}
