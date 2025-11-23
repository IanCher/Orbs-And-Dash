using System;
using UnityEngine;
using TMPro;

public class RareOrbHandler : MonoBehaviour
{
    public static event Action<string> OnRequirementSet;

    [SerializeField] private RareOrb rareOrb;

    [SerializeField] private float timeLimit;
    [SerializeField] private int orbCountRequired;
    public static int OrbCountRequired;
    [SerializeField] private TextMeshProUGUI rareOrbInstructionUI;

    [SerializeField] private TextMeshProUGUI timerInstructionUI;



    private void Start()
    {
        OrbCounterManager.OnOrbCollected += OrbCounterManager_OnOrbCollected;

        if (rareOrbInstructionUI) rareOrbInstructionUI.text = "Collect " + orbCountRequired.ToString() + " orbs";
        if (rareOrb) rareOrb.gameObject.SetActive(false);

        int timeLimitMinute = TimeSpan.FromSeconds(timeLimit).Minutes;
        int timeLimitSecond = TimeSpan.FromSeconds(timeLimit).Seconds;

        string text = "Complete Level under ";
        text += timeLimitMinute.ToString() + ":" + timeLimitSecond.ToString("D2");
        if (timerInstructionUI) timerInstructionUI.text = text;

        OnRequirementSet?.Invoke(rareOrbInstructionUI.text + " under " + timeLimitMinute.ToString() + ":" + timeLimitSecond.ToString("D2") + " minutes");
        OrbCountRequired = orbCountRequired;
    }


    private void OnDestroy()
    {
        OrbCounterManager.OnOrbCollected -= OrbCounterManager_OnOrbCollected;
    }

    private void OrbCounterManager_OnOrbCollected(float arg1, float arg2)
    {
        if(arg2>=orbCountRequired &&!PlayerData.RareOrbsTrack.Contains(rareOrb.id))
        {
            rareOrb.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (TimerUI.time >= timeLimit && rareOrb.gameObject.activeInHierarchy)
            rareOrb.gameObject.SetActive(false);
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }
    
    public int GetRareOrbCountRequired()
    {
        return orbCountRequired;
    }
}
