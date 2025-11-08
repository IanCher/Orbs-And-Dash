using UnityEngine;

[CreateAssetMenu(fileName = "OrbData", menuName = "Scriptable Objects/OrbData")]
public class OrbData : ScriptableObject
{
    public int value = 1;
    [Range(0, 1)] public float speedGain = 0.1f;
}
