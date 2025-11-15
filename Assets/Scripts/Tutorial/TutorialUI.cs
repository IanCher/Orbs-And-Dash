using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialVisuals;

    public void EnableVisual(int id)
    {
        tutorialVisuals[id].SetActive(true);
    }
    public void DisableVisual(int id)
    {
        tutorialVisuals[id].SetActive(false);
    }
}
