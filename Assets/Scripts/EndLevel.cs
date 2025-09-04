using System;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [Tooltip("Time limit in seconds")]
    [SerializeField] float timeLimit = 60f;
    [SerializeField] TimerUI timerUI;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        float finishTime = timerUI.GetTime();
        timerUI.StopTrackingTime();
        
        if (finishTime > timeLimit)
        {
            Debug.Log("Player lost");
        }
        else
        {
            Debug.Log("Player wins");
        }
    }
}
