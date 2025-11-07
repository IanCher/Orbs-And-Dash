using System;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public static event Action OnTutorialTrigger;
    [SerializeField] private int id;

    private void OnTriggerEnter(Collider other)
    {
        OnTutorialTrigger?.Invoke();
        gameObject.SetActive(false);
    }
}
