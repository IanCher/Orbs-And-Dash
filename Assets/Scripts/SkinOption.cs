using UnityEngine;

[CreateAssetMenu(fileName = "SkinOption", menuName = "Appearance/Skin Option")]
public sealed class SkinOption : ScriptableObject
{
    [SerializeField] private string optionName;
    [SerializeField] private SkinManager.SlotType slotType;
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    public string OptionName => optionName;
    public SkinManager.SlotType SlotType => slotType;
    public Mesh Mesh => mesh;
    public Material Material => material;
}