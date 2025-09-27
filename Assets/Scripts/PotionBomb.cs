using System;
using UnityEngine;

public class PotionBomb : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    [SerializeField] GameObject explosion;
    protected virtual void Reset()
    {
        if (PotionsData == null) PotionsData = new PotionData();
        PotionsData.SetType(EffectType.PotionBomb);    
    }

    protected virtual void OnValidate()
    {
        if (PotionsData == null) PotionsData = new PotionData();
        PotionsData.SetType(EffectType.PotionBomb);
    }
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
