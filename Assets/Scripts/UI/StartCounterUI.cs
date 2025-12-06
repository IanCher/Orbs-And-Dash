using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StartCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startCounterText;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 targetScale = Vector3.one;

    private Color startColor;
    private Vector3 startScale;
    private RectTransform textRect;

    private void Awake()
    {
        textRect = startCounterText.rectTransform;
        startColor = startCounterText.color;
        startScale = startCounterText.rectTransform.localScale;

        StartCounter.OnStep += StartCounterUI_OnStep;
        StartCounter.OnFinished += StartCounterUI_OnFinished;

        startCounterText.gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        StartCounter.OnStep -= StartCounterUI_OnStep;
        StartCounter.OnFinished -= StartCounterUI_OnFinished;
    }


    private void StartCounterUI_OnFinished()
    {
        startCounterText.gameObject.SetActive(false);
    }

    private void    StartCounterUI_OnStep(string obj)
    {
        startCounterText.gameObject.SetActive(true);
        ResetTextState();

        startCounterText.text = obj;
        StartCoroutine(FadeOutAndScaleUp());
    }

    private void ResetTextState()
    {
        startCounterText.color = startColor;
        startCounterText.rectTransform.localScale = startScale;
    }


    private IEnumerator FadeOutAndScaleUp()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            startCounterText.color = new Color(startColor.r,startColor.g,startColor.b,Mathf.Lerp(1,0,t)) ;

            textRect.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

    }
}
