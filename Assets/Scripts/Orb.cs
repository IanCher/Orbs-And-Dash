using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] int value = 1;
    bool wasCollected = false;

    public int GetValue() => value;
    public bool WasCollected() => wasCollected;
    public void SetWasCollected(bool value) => wasCollected = value;
}
