using System;
using UnityEngine;

[Serializable]
public class PotionData
{
    [SerializeField] private EffectType type = EffectType.NULL;


    [ShowIfEnum(nameof(type), nameof(EffectType.AccelerationPlatform))]
    [Range(0, 10)] public int Acceleration;
    [ShowIfEnum(nameof(type), nameof(EffectType.AccelerationPlatform))]
    public float TimeOfAcceleration;

    [ShowIfEnum(nameof(type), nameof(EffectType.ParalyzeSpell), nameof(EffectType.PotionBomb))]
    [Range(0f, 100f)] public float PercentToSlowBy;
    [ShowIfEnum(nameof(type), nameof(EffectType.ParalyzeSpell))]
    public int JumpNeeded;

    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
    public float PercentOrbsToLose;
    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
    public int OrbsToLose;
    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
    public OrbType OrbTypeToLose;

    //Uncomment AND implement if we decide to implement shield potion here instead of separate shield class (ShieldData)
    //[ShowIfEnum(nameof(type), nameof(EffectType.MysterySpell))]
    //public float ShieldDurationSeconds;
    //[ShowIfEnum(nameof(type),  nameof(EffectType.MysterySpell))]
    //public int ShieldValue;





    public EffectType Type => type;
    public void SetType(EffectType value) => type = value;
}

