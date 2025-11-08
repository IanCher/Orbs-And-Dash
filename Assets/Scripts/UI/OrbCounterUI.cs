using TMPro;
using UnityEngine;

public class OrbCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lowOrbCountText;
    // [SerializeField] private TextMeshProUGUI midOrbCountText;
    [SerializeField] private TextMeshProUGUI highOrbCountText;
    // [SerializeField] private TextMeshProUGUI totalOrbCountText;


    private void Start()
    {
        lowOrbCountText.text = "x0";
        highOrbCountText.text = "x0";
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest += OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OnDestroy()
    {
        OrbCounterManager.OnSendOrbCountVisualUpdateRequest -= OrbCounterManager_OnSendOrbCountVisualUpdateRequest;
    }

    private void OrbCounterManager_OnSendOrbCountVisualUpdateRequest(int arg1, int arg3)
    {
        lowOrbCountText.text = "x" + arg1.ToString();
        highOrbCountText.text = "x" + arg3.ToString();
    }
}
