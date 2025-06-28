using System;
using UnityEngine;

public class OrbCounterManager : MonoBehaviour
{
    public static event Action<int, int, int> OnSendOrbCountVisualUpdateRequest;

    private int lowOrbCount = 0;
    private int midOrbCount = 0;
    private int highOrbCount = 0;

    private void Start()
    {
        PlayerOrbCollector.OnOrbCollected += PlayerOrbCollector_OnOrbCollected;
    }

    private void OnDestroy()
    {
        PlayerOrbCollector.OnOrbCollected -= PlayerOrbCollector_OnOrbCollected;
    }

    private void PlayerOrbCollector_OnOrbCollected(Orb.OrbType obj)
    {
        switch (obj)
        {
            case Orb.OrbType.Low:
                lowOrbCount++;
                break;
            case Orb.OrbType.Mid:
                midOrbCount++;
                break;
            case Orb.OrbType.High:
                highOrbCount++;
                break;
            default:
                break;
        }

        OnSendOrbCountVisualUpdateRequest?.Invoke(lowOrbCount, midOrbCount, highOrbCount);
    }
}
