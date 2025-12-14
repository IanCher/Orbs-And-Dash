using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbCounterManager : MonoBehaviour
{
    public static event Action<int, int, int> OnSendOrbCountVisualUpdateRequest;
    public static event Action<float,float> OnOrbCollected;
        
    public static int lowOrbCount = 0;
    private int highOrbCount = 0;
    
    private PlayerStats playerStats;
    [SerializeField] private bool enableOrbSpeedScaling = true;
    [SerializeField] OrbData lowOrbData;
    [SerializeField] OrbData highOrbData;

    private void Awake()
    {
        // Subscribe to player registration events - guaranteed to happen before any Start for placed objects in the level.
        PlayerStats.OnPlayerStatsReady += OnPlayerStatsRegistered;
        PlayerStats.OnPlayerStatsDestroyed += OnPlayerStatsDestroyed;
    }

    private void Start()
    {        
        Orb.OnOrbCollected += PlayerOrbCollector_OnOrbCollected;
        PotionBomb.OnCollidedWithPotion += PlayerOrbCollector_OnCollidedWithPotion;
        MysterySpell.OnCollideWithMysterySpell += PlayerOrbCollector_OnCollidedWithMysterySpell;
        EndLevel.OnPlayerWon += EndLevel_OnPlayerWon;
        lowOrbCount = 0;
    }

    private void EndLevel_OnPlayerWon()
    {
        PlayerData.NormalOrbs += lowOrbCount;
    }

    private void OnDestroy()
    {
        Orb.OnOrbCollected -= PlayerOrbCollector_OnOrbCollected;
        PotionBomb.OnCollidedWithPotion -= PlayerOrbCollector_OnCollidedWithPotion;
        MysterySpell.OnCollideWithMysterySpell -= PlayerOrbCollector_OnCollidedWithMysterySpell;
        EndLevel.OnPlayerWon -= EndLevel_OnPlayerWon;
        // unsubscribe to stop unity error when switching scenes
        PlayerStats.OnPlayerStatsReady -= OnPlayerStatsRegistered;
        PlayerStats.OnPlayerStatsDestroyed -= OnPlayerStatsDestroyed;
    }

    private void OnPlayerStatsRegistered(PlayerStats stats)
    {
        Debug.Log("OrbCounterManager: PlayerStats registered.");
        playerStats = stats;
    }

    private void OnPlayerStatsDestroyed()
    {
        Debug.Log("OrbCounterManager: PlayerStats unregistered.");
        playerStats = null;
    }

    private void PlayerOrbCollector_OnCollidedWithMysterySpell(MysterySpellEffect effect)
    {
        // Only orb-related side effects are handled here. Shield handling stays in PlayerStats.
        if (effect == null)
        {
            Debug.LogWarning("OrbCounterManager: MysterySpellEffect is null.");
            return;
        }
        switch (effect.effectType)
        {
            case MysterySpellEffectType.Damage:
                if (effect.damageData != null)
                {
                    PlayerOrbCollector_OnCollidedWithPotion(effect.damageData); //TODO: implement a separate handler if needed
                }
                else
                {
                    Debug.LogWarning("OrbCounterManager: Damage effect has null damageData.");
                }
                break;
        }
    }

    private void PlayerOrbCollector_OnOrbCollected(Orb orb)
    {
        if(playerStats != null && playerStats.Paralyzed)
        {
            Debug.Log("OrbCounterManager: Player is paralyzed - ignoring orb collection");
            return;
        }

        switch (orb.OrbType)
        {
            case OrbType.Low:
                lowOrbCount++;
                break;
            case OrbType.High:
                highOrbCount++;
                break;
            default:
                break;
        }

        if (enableOrbSpeedScaling)
        {
            OnOrbCollected?.Invoke(orb.SpeedGain,lowOrbCount);
        }
        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, highOrbCount, GetTotalOrbCount());
    }

    private void PlayerOrbCollector_OnCollidedWithPotion(PotionData potionData)
    {
        // Don't lose orbs if player is invulnerable
        if (playerStats != null && playerStats.IsInvulnerable)
        {
            Debug.Log("OrbCounterManager: Player is invulnerable - ignoring orb losses");
            return;
        }

        if (playerStats == null || !playerStats.IsInvulnerable)
        {
            if (playerStats == null)
            {
                Debug.LogWarning("OrbCounterManager: PlayerStats is null!  Precondition violation with event registration");
            }

            LoseOrbs(potionData);
        }
        else
        {
            Debug.Log("OrbCounterManager: Player is invulnerable - ignoring orb losses and de-buffs");
        }
    }

    private void LoseOrbs(PotionData potionData)
    {
        if (potionData.PercentOrbsToLose <= 0)
        {
            switch (potionData.OrbTypeToLose)
            {
                case OrbType.Low:
                    lowOrbCount -= potionData.OrbsToLose;
                    lowOrbCount = Mathf.Max(0, lowOrbCount);
                    break;
                case OrbType.High:
                    highOrbCount -= potionData.OrbsToLose;
                    highOrbCount = Mathf.Max(0, highOrbCount);
                    break;
                case OrbType.All:
                    lowOrbCount -= potionData.OrbsToLose;
                    lowOrbCount = Mathf.Max(0, lowOrbCount);

                    highOrbCount -= potionData.OrbsToLose;
                    highOrbCount = Mathf.Max(0, highOrbCount);
                    break;
            }
        }
        else
        {
            float percentToLose = potionData.PercentOrbsToLose / 100f;
            switch (potionData.OrbTypeToLose)
            {
                case OrbType.Low:
                    lowOrbCount -= Mathf.CeilToInt(lowOrbCount * percentToLose);
                    break;
                case OrbType.High:
                    highOrbCount -= Mathf.CeilToInt(highOrbCount * percentToLose);
                    break;
                case OrbType.All:
                    lowOrbCount -= Mathf.CeilToInt(lowOrbCount * percentToLose);
                    highOrbCount -= Mathf.CeilToInt(highOrbCount * percentToLose);
                    break;
            }
        }

        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, highOrbCount, GetTotalOrbCount());
    }

    public int GetTotalOrbCount()
    {
        return lowOrbCount * lowOrbData.value + highOrbCount * highOrbData.value;
    }
}
