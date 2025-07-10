using TMPro;
using UnityEngine;

public class OrbCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lowOrbCountText;
    [SerializeField] private TextMeshProUGUI midOrbCountText;
    [SerializeField] private TextMeshProUGUI highOrbCountText;
    [SerializeField] private TextMeshProUGUI totalOrbCountText;


    private void Start()
    {
        lowOrbCountText.text = "x0";
        midOrbCountText.text = "x0";
        highOrbCountText.text = "x0";
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest += OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OnDestroy()
    {
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest -= OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OrbCounterManager_OnSendOrbCountVisualUpdateRequest(int arg1, int arg2, int arg3, int arg4)
    {
        lowOrbCountText.text = "x" + arg1.ToString();
        midOrbCountText.text = "x" + arg2.ToString();
        highOrbCountText.text = "x" + arg3.ToString();
        totalOrbCountText.text = "x" + arg4.ToString();
    }
}
