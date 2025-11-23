using TMPro;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    private void OnEnable()
    {
        resultText.text = $"Orbs Collected {OrbCounterManager.lowOrbCount}/{RareOrbHandler.OrbCountRequired}\r\nTime Took {TimerUI.GetTimeString()}";
    }
}
