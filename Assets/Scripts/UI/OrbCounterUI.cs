using TMPro;
using UnityEngine;

public class OrbCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lowOrbCountText;
    // [SerializeField] private TextMeshProUGUI midOrbCountText;
    [SerializeField] private TextMeshProUGUI highOrbCountText;
    [SerializeField] private TextMeshProUGUI totalOrbCountText;


    private void Start()
    {
        lowOrbCountText.text = "0";
        highOrbCountText.text = "0";
        totalOrbCountText.text = "x0";
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest += OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OnDestroy()
    {
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest -= OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OrbCounterManager_OnSendOrbCountVisualUpdateRequest(int arg1, int arg2, int arg3)
    {
        lowOrbCountText.text = arg1.ToString();
        highOrbCountText.text = arg2.ToString();
        totalOrbCountText.text = "x" + arg3.ToString();
    }
}
