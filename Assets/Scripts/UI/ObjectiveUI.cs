using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private float duration = 5;
    public static event Action OnHide;

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
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        OnHide?.Invoke();
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
