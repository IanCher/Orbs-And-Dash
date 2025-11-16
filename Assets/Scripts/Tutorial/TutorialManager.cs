using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private bool enableTutorial = false;


    private void Awake()
    {

    }

    private void Update()
    {
     

    }

    

    private async Task Start()
    {
        InputSystem.actions.FindAction("Jump").performed += TutorialManager_performed; 

        TutorialTrigger.OnTutorialTrigger += TutorialTrigger_OnTutorialTrigger;
        transform.position = Vector3.zero;

        if (PlayerData.TutorialCompleted == 1)
        {
            enableTutorial = false;
            return;
        }
    }
    

    private void TutorialManager_performed(InputAction.CallbackContext obj)
    {
        if (enableTutorial)
        {
            UnPause();
            tutorialUI.DisableVisual(4);

        }
    }

    private void OnDestroy()
    {
        TutorialTrigger.OnTutorialTrigger -= TutorialTrigger_OnTutorialTrigger;
        InputSystem.actions.FindAction("Jump").performed -= TutorialManager_performed; ;

    }

    private void TutorialTrigger_OnTutorialTrigger(int id)
    {
        if (!enableTutorial) return;

        switch (id)
        {
            case 0:
                StartCoroutine(ShowTutorialForFixedTime(id));

                break;
            case 4:
                tutorialUI.EnableVisual(id);
                Pause();
                break;

                case 5:
                PlayerData.TutorialCompleted = 1;
                StartCoroutine(ShowTutorialForFixedTime(id));

                break;
            default:
                StartCoroutine(ShowTutorialForFixedTime(id));

                break;
        }


        // PlayerData.TutorialCompleted = 1;
    }

    private IEnumerator ShowTutorialForFixedTime(int id)
    {
        tutorialUI.EnableVisual(id);
        yield return new WaitForSecondsRealtime(3f);
        tutorialUI.DisableVisual(id);
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
