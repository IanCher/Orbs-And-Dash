using System;
using UnityEngine;

public class PotionBomb : MonoBehaviour
{
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
}
[Serializable]
public class PotionData
{
    public float PercentOrbsToLose;
    public int OrbsToLose;
    public float PercentToSlowBy;
    public OrbType OrbTypeToLose;
}
