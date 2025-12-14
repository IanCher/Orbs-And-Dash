using System;
using UnityEngine;
using TMPro;
using NUnit.Framework.Internal;

public class EndLevel : MonoBehaviour
{
    public static event Action OnEndLevelTriggerd;
    public static event Action OnPlayerWon;

    [SerializeField] OrbCounterManager orbCounterManager;
    [SerializeField] TimerUI timerUI;
    [SerializeField] Canvas finishCanvas;
    [SerializeField] TMP_Text finishText;
    [SerializeField] RareOrbHandler rareOrbHandler;
    [SerializeField] bool isBonusStage = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        timerUI.StopTrackingTime();
        finishCanvas.gameObject.SetActive(true);

        if (isBonusStage) return;
        
        if (HasPlayerWon())
        {
            finishText.text = "You win!!!";
            OnPlayerWon?.Invoke();
        }
        else
        {
            finishText.text = "You lose...";
        }

        OnEndLevelTriggerd?.Invoke();
    }

    bool HasPlayerWon()
    {
        float finishTime = timerUI.GetTime();
        int totalOrbCount = orbCounterManager.GetTotalOrbCount();
        float timeLimit = rareOrbHandler.GetTimeLimit();

        return (finishTime <= timeLimit) && (totalOrbCount > rareOrbHandler.GetRareOrbCountRequired());
    }
}
