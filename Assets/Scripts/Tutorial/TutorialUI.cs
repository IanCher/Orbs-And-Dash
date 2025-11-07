using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialVisual;

    public void EnableVisual()
    {
        tutorialVisual.SetActive(true);
    }
    public void DisableVisual()
    {
        tutorialVisual.SetActive(false);
    }
}
