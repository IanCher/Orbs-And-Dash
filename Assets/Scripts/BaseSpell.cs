using System;
using System.Collections;
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
            var value = (EffectType)(object)EffectKind;  // cast �boxing-safe� 
            CollisionEffects.SetType(value);
        }
        else
        {
            Debug.LogWarning($"{GetType().Name}: TEnum not an EffectType, Need to add it on the enum.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitter = other.gameObject;

        if (hitter.CompareTag("Player"))
        {
            playerStats = hitter.GetComponent<PlayerStats>();
        }
    }
    PlayerStats playerStats;
    [Header("VFX And SFX")]
    [SerializeField] EffectClass effects = new EffectClass();
    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(CollisionEffects);
        SpawnEffects();
        //this.SetActive(false);
        Destroy(gameObject);
    }
    private void SpawnEffects()
    {
        AudioManager.instance.PlaySound(effects.SoundName);

        if (effects.Vfx != null)
        {
            if (effects.AttachToPlayer)
            {
                GameObject effect = Instantiate(
                  effects.Vfx,
                  transform.position,
                  transform.rotation
                );
                if (playerStats != null)
                {

                    effect.transform.SetParent(playerStats.transform);   // attacco all'oggetto Player
                    effect.transform.localPosition = Vector3.zero;  // opzionale: posizionalo relativo al centro del player
                    effect.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning("Player non trovato!");
                }


                Destroy(effect, 1f);
            }
            else if (effects.EnableOnPlayer)
            {
                effects.Vfx.SetActive(true);
            }
            else
            {
                GameObject effect = Instantiate(
                    effects.Vfx,
                    transform.position,
                    transform.rotation
                );
                Destroy(effect, 1f);
            }


        }
        // if (effects.UIToSpawn != null)
        // {
        //     effects.UIToSpawn.Enable();
        //     //Destroy(uiEffect, 1f);
        // }
    }
  
}
