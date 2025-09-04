using System;
using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [Tooltip("Time limit in seconds")]
    [SerializeField] float timeLimit = 60f;
    [SerializeField] TimerUI timerUI;
    [SerializeField] Canvas finishCanvas;
    [SerializeField] TMP_Text finishText;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        float finishTime = timerUI.GetTime();
        timerUI.StopTrackingTime();
        finishCanvas.gameObject.SetActive(true);

        if (finishTime > timeLimit)
        {
            finishText.text = "You lose...";
        }
        else
        {
            finishText.text = "You win!!!";
        }
    }
}
