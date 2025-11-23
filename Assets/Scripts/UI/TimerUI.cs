using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Text))]
public class TimerUI : MonoBehaviour
{
    private TMP_Text timerUIText;
    public static float time;
    bool stopTrackingTime = true;

    void Awake()
    {
        timerUIText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        time = 0;
        stopTrackingTime = true;
    }

    void Update()
    {
        if (stopTrackingTime) return;
        
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timerUIText.text = timeSpan.ToString(@"mm\:ss");
    }

    public float GetTime()
    {
        return time;
    }

    public static string GetTimeString()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return timeSpan.ToString(@"mm\:ss");
    }

    public void StopTrackingTime()
    {
        stopTrackingTime = true;
    }

    public void StartTrackingTime()
    {
        stopTrackingTime = false;
    }
}
