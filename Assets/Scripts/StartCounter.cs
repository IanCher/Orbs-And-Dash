using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class StartCounter : MonoBehaviour
{
    public static event Action<string> OnStep;
    public static event Action OnFinished;
    public static event Action OnGo;


    private void Awake()
    {
        ObjectiveUI.OnHide += ObjectiveUI_OnHide;
    }

    private void OnDestroy()
    {
        ObjectiveUI.OnHide -= ObjectiveUI_OnHide;
    }

    private void ObjectiveUI_OnHide()
    {
        BeginCountDown();
    }

    public void BeginCountDown()
    {
        StartCoroutine(RunCountDown());
    }

    private IEnumerator RunCountDown()
    {
        string[] steps = { "3", "2", "1", "GO" };

        foreach (string step in steps)
        {
            OnStep?.Invoke(step);
            AudioManager.instance.PlaySound("Jump");
            if(step == "GO")
                OnGo?.Invoke();
            yield return new WaitForSeconds(1);
        }
        OnFinished?.Invoke();
    }
}
