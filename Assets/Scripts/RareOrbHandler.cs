using UnityEngine;

public class RareOrbHandler : MonoBehaviour
{
    [SerializeField] private RareOrb rareOrb;

    [SerializeField] private float timeLimit;
    [SerializeField] private int orbCountRequired;



    private void Start()
    {
        OrbCounterManager.OnOrbCollected += OrbCounterManager_OnOrbCollected;
        rareOrb.gameObject.SetActive(false);
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
        if(TimerUI.time >= timeLimit && rareOrb.gameObject.activeInHierarchy)
            rareOrb.gameObject. SetActive(false);
    }
}
