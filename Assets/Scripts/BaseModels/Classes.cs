using System;
using UnityEngine;

[Serializable]
public class PotionData
{
    [SerializeField] private EffectType type = EffectType.NULL;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.AccelerationPlatform))]
# endif

    [Range(0, 10)] public int Acceleration;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.AccelerationPlatform))]
# endif
    public float TimeOfAcceleration;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.ParalyzeSpell), nameof(EffectType.PotionBomb))]
# endif
    [Range(0f, 100f)] public float PercentToSlowBy;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.ParalyzeSpell))]
# endif
    public int JumpNeeded;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
# endif
    public float PercentOrbsToLose;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
# endif
    public int OrbsToLose;

#if UNITY_EDITOR
    [ShowIfEnum(nameof(type), nameof(EffectType.PotionBomb), nameof(EffectType.MysterySpell))]
# endif
    public OrbType OrbTypeToLose;

    public EffectType Type => type;
    public void SetType(EffectType value) => type = value;
}

