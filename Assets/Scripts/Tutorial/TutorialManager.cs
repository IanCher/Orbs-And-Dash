using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private bool enableTutorial = false;
    private void Start()
    {
        TutorialTrigger.OnTutorialTrigger += TutorialTrigger_OnTutorialTrigger;
        transform.position = Vector3.zero;

        if(PlayerData.TutorialCompleted == 1)
        {
            enableTutorial = false;
        }
    }

    private void OnDestroy()
    {
        TutorialTrigger.OnTutorialTrigger -= TutorialTrigger_OnTutorialTrigger;
    }

    private void TutorialTrigger_OnTutorialTrigger()
    {
        if (!enableTutorial) return;

        StartCoroutine(ShowTutorialForFixedTime());

        PlayerData.TutorialCompleted = 1;
    }

    private IEnumerator ShowTutorialForFixedTime()
    {
        Pause();
        tutorialUI.EnableVisual();
        yield return new WaitForSecondsRealtime(3f);
        UnPause();
        tutorialUI.DisableVisual();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
    }
}
