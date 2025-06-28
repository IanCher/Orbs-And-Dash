using UnityEngine;

public class Orb : MonoBehaviour
{
    public enum OrbType { Low,Mid,High };
    [SerializeField] private OrbType type;

    [SerializeField] int value = 1;
    bool wasCollected = false;

    public int GetValue() => value;
    public bool WasCollected() => wasCollected;
    public void SetWasCollected(bool value) => wasCollected = value;

    public OrbType GetOrbType() => type;
}
