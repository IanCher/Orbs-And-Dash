using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbCounterManager : MonoBehaviour
{
    public static event Action<int, int, int, int> OnSendOrbCountVisualUpdateRequest;

    private int lowOrbCount = 0;
    private int midOrbCount = 0;
    private int highOrbCount = 0;

    private void Start()
    {
        PlayerOrbCollector.OnOrbCollected += PlayerOrbCollector_OnOrbCollected;
        PlayerOrbCollector.OnCollidedWithPotion += PlayerOrbCollector_OnCollidedWithPotion;
    }

    private void OnDestroy()
    {
        PlayerOrbCollector.OnOrbCollected -= PlayerOrbCollector_OnOrbCollected;
        PlayerOrbCollector.OnCollidedWithPotion -= PlayerOrbCollector_OnCollidedWithPotion;
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

        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, midOrbCount, highOrbCount, TotalOrbCount());
    }


    private void PlayerOrbCollector_OnCollidedWithPotion(PotionData potionData)
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

        if (potionData.PercentToSlowBy > 0)
        {
            //slow player here
        }

        if (lowOrbCount < 0 || midOrbCount < 0 || highOrbCount < 0)
        {
            //game over here
            //temporary reload scene
            SceneManager.LoadScene(0);
        }

    }
    private int TotalOrbCount()
    {
        int totalCount = lowOrbCount + midOrbCount + highOrbCount;
        return totalCount;
    }

}
