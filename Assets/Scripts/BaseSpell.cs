using System;
using UnityEngine;

public class BaseSpell : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    [Header("VFX And SFX")]
    [SerializeField] EffectClass effects = new EffectClass();
    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(PotionsData);
        SpawnEffects();
        Destroy(gameObject);
    }
    private void SpawnEffects()
    {
        AudioManager.instance.PlaySound(effects.SoundName);

        if (effects.Vfx != null)
        {

            GameObject effect = Instantiate(
                effects.Vfx,
                transform.position,
                transform.rotation
            );
            
            Destroy(effect, 1f);
        }       
    }
}
