using System;
using UnityEngine;
using TMPro;
using NUnit.Framework.Internal;

public class EndLevel : MonoBehaviour
{
    public static event Action OnPlayerWon;

    [SerializeField] TimerUI timerUI;
    [SerializeField] Canvas finishCanvas;
    [SerializeField] TMP_Text finishText;
    [SerializeField] RareOrbHandler rareOrbHandler;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        float finishTime = timerUI.GetTime();
        timerUI.StopTrackingTime();
        finishCanvas.gameObject.SetActive(true);

        float timeLimit = rareOrbHandler.GetTimeLimit();

        if (finishTime > timeLimit)
        {
            finishText.text = "You lose...";
        }
        else
        {
            finishText.text = "You win!!!";
            OnPlayerWon?.Invoke();
        }
    }
}
