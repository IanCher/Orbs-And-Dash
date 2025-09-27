using System;
using UnityEngine;
using static AccelerationPlatform;

public abstract class BaseSpell<TEnum> : MonoBehaviour, ICollideable where TEnum : struct, Enum
{   
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    [SerializeField()] protected PotionData CollisionEffects;
    private bool wasHitByPlayer = false;


    protected abstract TEnum EffectKind { get; }
    protected virtual void Reset()
    {
        if (CollisionEffects == null) CollisionEffects = new PotionData();
        ApplyEffectKind();
    }

    protected virtual void OnValidate()
    {
        if (CollisionEffects != null) ApplyEffectKind();
    }
    private void ApplyEffectKind()
    {      
        if (typeof(TEnum) == typeof(EffectType))
        {
            var value = (EffectType)(object)EffectKind;  // cast “boxing-safe” 
            CollisionEffects.SetType(value);
        }
        else
        {           
            Debug.LogWarning($"{GetType().Name}: TEnum not an EffectType, Need to add it on the enum.");
        }
    }

    [Header("VFX And SFX")]
    [SerializeField] EffectClass effects = new EffectClass();
    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(CollisionEffects);
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
