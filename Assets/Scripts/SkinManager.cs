using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public sealed class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    public enum SlotType
    {
        Hat,
        Hair,
        Clothes,
        Broom
    }

    [Serializable]
    public class SkinOptionEntry
    {
        public SkinOption option;
        public bool unlocked;
        [Min(0)] public int price;
    }

    [SerializeField] private List<SkinOptionEntry> hatSkinEntries = new List<SkinOptionEntry>();
    [SerializeField] private List<SkinOptionEntry> hairSkinEntries = new List<SkinOptionEntry>();
    [SerializeField] private List<SkinOptionEntry> clothesSkinEntries = new List<SkinOptionEntry>();
    [SerializeField] private List<SkinOptionEntry> broomSkinEntries = new List<SkinOptionEntry>();

    [Min(0)] private int currentHatIndex = 0;
    [Min(0)] private int currentHairIndex = 0;
    [Min(0)] private int currentClothesIndex = 0;
    [Min(0)] private int currentBroomIndex = 0;

    [SerializeField] private TMP_Text hatNameText;
    [SerializeField] private TMP_Text hairNameText;
    [SerializeField] private TMP_Text clothesNameText;
    [SerializeField] private TMP_Text broomNameText;

    private readonly Dictionary<SlotType, List<SkinOptionEntry>> skinEntries = new Dictionary<SlotType, List<SkinOptionEntry>>();
    private readonly Dictionary<SlotType, int> currentIndices = new Dictionary<SlotType, int>();
    public Dictionary<SlotType, TMP_Text> nameTexts = new Dictionary<SlotType, TMP_Text>();

    private static readonly SlotType[] AllSlots = (SlotType[])Enum.GetValues(typeof(SlotType));

    public event Action<SlotType, SkinOptionEntry> SkinSelectionChanged;

    public IReadOnlyList<SkinOptionEntry> HatSkins => skinEntries[SlotType.Hat];
    public IReadOnlyList<SkinOptionEntry> HairSkins => skinEntries[SlotType.Hair];
    public IReadOnlyList<SkinOptionEntry> ClothesSkins => skinEntries[SlotType.Clothes];
    public IReadOnlyList<SkinOptionEntry> BroomSkins => skinEntries[SlotType.Broom];

    public int CurrentHatIndex { get => currentIndices[SlotType.Hat]; set => SetIndex(SlotType.Hat, value); }
    public int CurrentHairIndex { get => currentIndices[SlotType.Hair]; set => SetIndex(SlotType.Hair, value); }
    public int CurrentClothesIndex { get => currentIndices[SlotType.Clothes]; set => SetIndex(SlotType.Clothes, value); }
    public int CurrentBroomIndex { get => currentIndices[SlotType.Broom]; set => SetIndex(SlotType.Broom, value); }

    public SkinOptionEntry CurrentHatEntry => GetEntry(skinEntries[SlotType.Hat], currentIndices[SlotType.Hat]);
    public SkinOptionEntry CurrentHairEntry => GetEntry(skinEntries[SlotType.Hair], currentIndices[SlotType.Hair]);
    public SkinOptionEntry CurrentClothesEntry => GetEntry(skinEntries[SlotType.Clothes], currentIndices[SlotType.Clothes]);
    public SkinOptionEntry CurrentBroomEntry => GetEntry(skinEntries[SlotType.Broom], currentIndices[SlotType.Broom]);
    public SkinOptionEntry GetCurrentEntry(SlotType slot)
    {
        return GetEntry(skinEntries[slot], currentIndices[slot]);
    }
    private SkinOptionEntry GetEntry(List<SkinOptionEntry> list, int index)
    {
        if (list == null || list.Count == 0) return null;
        if (index < 0 || index >= list.Count) return null;
        return list[index];
    }
    private void InitializeDictionaries()
    {
        skinEntries[SlotType.Hat] = hatSkinEntries;
        skinEntries[SlotType.Hair] = hairSkinEntries;
        skinEntries[SlotType.Clothes] = clothesSkinEntries;
        skinEntries[SlotType.Broom] = broomSkinEntries;

        currentIndices[SlotType.Hat] = currentHatIndex;
        currentIndices[SlotType.Hair] = currentHairIndex;
        currentIndices[SlotType.Clothes] = currentClothesIndex;
        currentIndices[SlotType.Broom] = currentBroomIndex;

        // nameTexts[SlotType.Hat] = hatNameText;
        // nameTexts[SlotType.Hair] = hairNameText;
        // nameTexts[SlotType.Clothes] = clothesNameText;
        // nameTexts[SlotType.Broom] = broomNameText;
    }
    private void SyncIndexField(SlotType slot, int value)
    {
        switch (slot)
        {
            case SlotType.Hat: currentHatIndex = value; break;
            case SlotType.Hair: currentHairIndex = value; break;
            case SlotType.Clothes: currentClothesIndex = value; break;
            case SlotType.Broom: currentBroomIndex = value; break;
        }
    }
    private void SetIndex(SlotType slot, int value)
    {
        var list = skinEntries[slot];
        if (list == null || list.Count == 0) return;
        int clamped = Mathf.Clamp(value, 0, list.Count - 1);
        currentIndices[slot] = clamped;
        SyncIndexField(slot, clamped);
        RaiseSelectionChanged(slot);
    }
    private void CycleSkin(SlotType slot, int direction)
    {
        var list = skinEntries[slot];
        if (list == null || list.Count == 0) return;
        int newIndex = (currentIndices[slot] + direction + list.Count) % list.Count;
        currentIndices[slot] = newIndex;
        SyncIndexField(slot, newIndex);
        RaiseSelectionChanged(slot);
    }
    public void NextSkin(SlotType slot) => CycleSkin(slot, 1);
    public void PreviousSkin(SlotType slot) => CycleSkin(slot, -1);

    public void NextHatSkin() => NextSkin(SlotType.Hat);
    public void NextHairSkin() => NextSkin(SlotType.Hair);
    public void NextClothesSkin() => NextSkin(SlotType.Clothes);
    public void NextBroomSkin() => NextSkin(SlotType.Broom);

    public void PreviousHatSkin() => PreviousSkin(SlotType.Hat);
    public void PreviousHairSkin() => PreviousSkin(SlotType.Hair);
    public void PreviousClothesSkin() => PreviousSkin(SlotType.Clothes);
    public void PreviousBroomSkin() => PreviousSkin(SlotType.Broom);

    private void RaiseSelectionChanged(SlotType slot)
    {
        UpdateUIText(slot);
        SkinSelectionChanged?.Invoke(slot, GetCurrentEntry(slot));
    }
    public void NotifyAllSelections()
    {
        foreach (var slot in AllSlots)
            RaiseSelectionChanged(slot);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeDictionaries();
    }
    private void Start()
    {
        UpdateAllUIText();
    }
    private void UpdateUIText(SlotType slot)
    {
        var entry = GetCurrentEntry(slot);
        string display = (entry != null && entry.option != null) ? entry.option.OptionName : "-";
        TMP_Text txt;
        if (nameTexts.TryGetValue(slot, out txt) && txt != null)
            txt.text = display;
    }
    public void UpdateAllUIText()
    {
        foreach (var slot in AllSlots)
            UpdateUIText(slot);
    }

    public string GetSlotName(SlotType slot)
    {
        var entry = GetCurrentEntry(slot);
        string display = (entry != null && entry.option != null) ? entry.option.OptionName : "-";
        return display;
    }
}