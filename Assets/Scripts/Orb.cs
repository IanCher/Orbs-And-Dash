using System;
using UnityEngine;

public class Orb : MonoBehaviour, ICollideable
{
    public static event Action<OrbType> OnOrbCollected;
    [SerializeField] private OrbType orbType;

    [SerializeField] int value = 1;
    bool wasCollected = false;

    public int GetValue() => value;
    public bool WasCollected() => wasCollected;
    public void SetWasCollected(bool value) => wasCollected = value;

    public OrbType GetOrbType() => orbType;

    public void HandlePlayerCollision()
    {
        OnOrbCollected?.Invoke(orbType);
        wasCollected = true;
        Destroy(gameObject);
    }
}
public enum OrbType { Low, Mid, High, All };
