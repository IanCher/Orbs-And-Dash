using System;
using UnityEngine;

public class Orb : MonoBehaviour, ICollideable
{
    public static event Action<Orb> OnOrbCollected;
    [SerializeField] OrbType orbType;
    public OrbType OrbType => orbType;
    
    [SerializeField] OrbData data;
    public float SpeedGain => data.speedGain;

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
