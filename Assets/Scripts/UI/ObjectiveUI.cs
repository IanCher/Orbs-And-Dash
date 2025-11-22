using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;

    private void Awake()
    {
        RareOrbHandler.OnRequirementSet += RareOrbHandler_OnRequirementSet;
    }

    private void Start()
    {

        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        RareOrbHandler.OnRequirementSet -= RareOrbHandler_OnRequirementSet;
    }

    private void RareOrbHandler_OnRequirementSet(string arg1)
    {
        objectiveText.text = arg1;
    }
}
