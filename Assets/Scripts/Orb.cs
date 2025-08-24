using System;
using UnityEngine;

public class Orb : MonoBehaviour, ICollideable
{
    public static event Action<Orb> OnOrbCollected;
    [SerializeField] OrbType orbType;
    public OrbType OrbType => orbType;

    [Range(0, 1)][SerializeField] float speedGain = 0.1f;
    public float SpeedGain => speedGain;

    bool wasCollected = false;

    public void HandlePlayerCollision()
    {
        if (wasCollected) return;
        OnOrbCollected?.Invoke(this);
        AudioManager.instance.PlaySound("OrbCollect");
        wasCollected = true;
        Destroy(gameObject);
    }
}
public enum OrbType { Low, Mid, High, All };
