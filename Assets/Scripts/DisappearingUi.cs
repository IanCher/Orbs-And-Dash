using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DisappearingUi : MonoBehaviour
{
    [SerializeField()] private float timeToLive = 1f;
    [SerializeField] private string TextToShow = "";
    [SerializeField] TextField uiTextField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  

    public void Enable()
    {
        if (uiTextField != null)
        {
            uiTextField.value = TextToShow;
        }

        gameObject.SetActive(true);
        StartCoroutine(RemoveUiElement(gameObject, timeToLive));
    }

    IEnumerator RemoveUiElement(GameObject uiToSpawn, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        uiToSpawn.SetActive(false);
    }
}
