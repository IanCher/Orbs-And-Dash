using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class TimerUI : MonoBehaviour
{
    private TMP_Text timerUIText;
    private float time;
    bool stopTrackingTime = false;

    void Awake()
    {
        timerUIText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (stopTrackingTime) return;
        
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timerUIText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }

    public float GetTime()
    {
        return time;
    }

    public void StopTrackingTime()
    {
        stopTrackingTime = true;
    }
}
