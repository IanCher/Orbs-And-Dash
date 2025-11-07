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

    private async void TutorialTrigger_OnTutorialTrigger()
    {
        if (!enableTutorial) return;

        Pause();
        tutorialUI.EnableVisual();
        await Task.Delay(3000);
        UnPause();
        tutorialUI.DisableVisual();

        PlayerData.TutorialCompleted = 1;
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
