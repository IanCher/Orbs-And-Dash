using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbCounterManager : MonoBehaviour
{
    public static event Action<int, int, int, int> OnSendOrbCountVisualUpdateRequest;
    public static event Action<int> OnOrbCollected;
        
    private int lowOrbCount = 0;
    private int midOrbCount = 0;
    private int highOrbCount = 0;
    
    private PlayerStats playerStats;
    [SerializeField] private bool enableOrbSpeedScaling = true;

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
    }

    private void OnDestroy()
    {
        Orb.OnOrbCollected -= PlayerOrbCollector_OnOrbCollected;
        PotionBomb.OnCollidedWithPotion -= PlayerOrbCollector_OnCollidedWithPotion;
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

    private void PlayerOrbCollector_OnOrbCollected(OrbType obj)
    {
        switch (obj)
        {
            case OrbType.Low:
                lowOrbCount++;
                break;
            case OrbType.Mid:
                midOrbCount++;
                break;
            case OrbType.High:
                highOrbCount++;
                break;
            default:
                break;
        }

        if (enableOrbSpeedScaling)
        {
            OnOrbCollected?.Invoke(TotalOrbCount());
        }
        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, midOrbCount, highOrbCount, TotalOrbCount());
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

            if (lowOrbCount < 0 || midOrbCount < 0 || highOrbCount < 0)
            {
                //game over here
                //temporary reload scene
                SceneManager.LoadScene(0);
            }
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
                    break;
                case OrbType.Mid:
                    midOrbCount -= potionData.OrbsToLose;
                    break;
                case OrbType.High:
                    highOrbCount -= potionData.OrbsToLose;
                    break;
                case OrbType.All:
                    lowOrbCount -= potionData.OrbsToLose;
                    midOrbCount -= potionData.OrbsToLose;
                    highOrbCount -= potionData.OrbsToLose;
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
                case OrbType.Mid:
                    midOrbCount -= Mathf.CeilToInt(midOrbCount * percentToLose);
                    break;
                case OrbType.High:
                    highOrbCount -= Mathf.CeilToInt(highOrbCount * percentToLose);
                    break;
                case OrbType.All:
                    lowOrbCount -= Mathf.CeilToInt(lowOrbCount * percentToLose);
                    midOrbCount -= Mathf.CeilToInt(midOrbCount * percentToLose);
                    highOrbCount -= Mathf.CeilToInt(highOrbCount * percentToLose);
                    break;
            }
        }

        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, midOrbCount, highOrbCount, TotalOrbCount());
    }

    private int TotalOrbCount()
    {
        int totalCount = lowOrbCount + midOrbCount + highOrbCount;
        return totalCount;
    }
}
