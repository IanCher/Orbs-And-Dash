using UnityEngine;
using System;

public class MysterySpell : MonoBehaviour, ICollideable
{
    public static event Action<MysterySpellEffect> OnCollideWithMysterySpell;

    [Header("MysterySpell Weights normalized to 100%")]
    [Min(0f)] public float takeDamageChance = 50f;
    [Min(0f)] public float shieldChance = 50f;

    [Header("Effect Data")]
    public PotionData damageData;
    public ShieldData shieldData;

    private bool wasHitByPlayer = false;

    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;
        wasHitByPlayer = true;

        CalculateWeights(out float damageWeight, out float shieldWeight, out float totalWeight);

        if (totalWeight <= 0f)
        {
            Debug.LogWarning("MysterySpell: All effect weights are zero or negative; no effect will be applied.");
            Destroy(gameObject);
            return;
        }

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        MysterySpellEffect effect;

        if (roll < damageWeight)
        {
            effect = new MysterySpellEffect
            {
                effectType = MysterySpellEffectType.Damage,
                damageData = damageData
            };
        }
        else
        {
            effect = new MysterySpellEffect
            {
                effectType = MysterySpellEffectType.Shield,
                shieldData = shieldData
            };
        }

        OnCollideWithMysterySpell?.Invoke(effect);
        Destroy(gameObject);
    }

    private void CalculateWeights(out float damageWeight, out float shieldWeight, out float totalWeight)
    {
        damageWeight = Mathf.Max(0f, takeDamageChance);
        shieldWeight = Mathf.Max(0f, shieldChance);
        totalWeight = damageWeight + shieldWeight;
    }
}
public enum MysterySpellEffectType
{
    Damage,
    Shield
}
public class MysterySpellEffect
{
    public MysterySpellEffectType effectType;
    public PotionData damageData;
    public ShieldData shieldData;
}