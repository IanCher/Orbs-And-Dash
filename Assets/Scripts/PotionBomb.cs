using System;
using UnityEngine;

public class PotionBomb : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    [SerializeField] GameObject explosion;

    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(PotionsData);
        PlayExplosion();
        Destroy(gameObject);
    }

    private void PlayExplosion()
    {
        if (explosion == null) return;

        GameObject explosionInstance = Instantiate(
            explosion,
            transform.position,
            transform.rotation
        );
        AudioManager.instance.PlaySound("PotionExplosion");

        Destroy(explosionInstance, 1f);
    }
}
[Serializable]
public class PotionData
{
    public float PercentOrbsToLose;
    public int OrbsToLose;
    [Range(0f, 100f)]
    public float PercentToSlowBy;
    public int JumpNeeded;
    // public float SlowDuration;
    public OrbType OrbTypeToLose;
}
