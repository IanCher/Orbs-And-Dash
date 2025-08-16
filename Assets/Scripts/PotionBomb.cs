using System;
using UnityEngine;

public class PotionBomb : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(PotionsData);
        Destroy(gameObject);
    }
}
[Serializable]
public class PotionData
{
    public float PercentOrbsToLose;
    public int OrbsToLose;
    public float PercentToSlowBy;
    public float SlowDuration;
    public OrbType OrbTypeToLose;
}
