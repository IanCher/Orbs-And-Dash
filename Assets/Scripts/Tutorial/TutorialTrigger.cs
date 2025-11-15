using System;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public static event Action<int> OnTutorialTrigger;
    [SerializeField] private int id;

    private void OnTriggerEnter(Collider other)
    {
        OnTutorialTrigger?.Invoke(id);
        gameObject.SetActive(false);
    }
}
