using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerModelManager : MonoBehaviour
{
    [System.Serializable]
    private class SlotRenderGroup
    {
        public SkinManager.SlotType slot;
        public List<SlotRenderTarget> targets = new List<SlotRenderTarget>();
    }

    [System.Serializable]
    private class SlotRenderTarget
    {
        public Renderer renderer;
        public MeshFilter meshFilter;

        public bool applyMesh = true;
        public bool applyMaterial = true;
    }

    [SerializeField] private List<SlotRenderGroup> slotRenderGroups = new List<SlotRenderGroup>();

    private readonly Dictionary<SkinManager.SlotType, SlotRenderGroup> groups = new Dictionary<SkinManager.SlotType, SlotRenderGroup>();
    private bool isSubscribed;

    private void Awake()
    {
        groups.Clear();
        foreach (var group in slotRenderGroups)
        {
            if (group == null) continue;
            groups[group.slot] = group;
        }
    }

    private void OnEnable()
    {
        TrySubscribe();

        if (!isSubscribed)
            StartCoroutine(DeferredSubscribe());
        if (SkinManager.Instance != null)
            RefreshAll();
    }
    private IEnumerator DeferredSubscribe()
    {
        float timeout = 1f;
        while (!isSubscribed && timeout > 0f)
        {
            TrySubscribe();
            if (isSubscribed)
            {
                RefreshAll();
                yield break;
            }
            timeout -= Time.unscaledDeltaTime;
            yield return null;
        }
    }
    private void TrySubscribe()
    {
        if (isSubscribed) return;
        var skinManager = SkinManager.Instance;
        if (skinManager == null) return;
        skinManager.SkinSelectionChanged -= OnSkinSelectionChanged;
        skinManager.SkinSelectionChanged += OnSkinSelectionChanged;
        isSubscribed = true;
    }
    private void OnDisable()
    {
        if (isSubscribed && SkinManager.Instance != null)
            SkinManager.Instance.SkinSelectionChanged -= OnSkinSelectionChanged;
        isSubscribed = false;
    }
    private void OnSkinSelectionChanged(SkinManager.SlotType slot, SkinManager.SkinOptionEntry entry)
    {
        ApplyEntry(slot, entry);
    }
    public void RefreshSlot(SkinManager.SlotType slot)
    {
        if (SkinManager.Instance == null) return;
        ApplyEntry(slot, SkinManager.Instance.GetCurrentEntry(slot));
    }
    public void RefreshAll()
    {
        if (SkinManager.Instance == null) return;
        foreach (var kvp in groups)
            ApplyEntry(kvp.Key, SkinManager.Instance.GetCurrentEntry(kvp.Key));
    }
    public void ApplySkin(SkinOption option)
    {
        if (option == null) return;
        ApplyEntry(option.SlotType, new SkinManager.SkinOptionEntry { option = option });
    }
    public void SetIndexAndRefresh(SkinManager.SlotType slot, int index)
    {
        if (SkinManager.Instance == null) return;
        switch (slot)
        {
            case SkinManager.SlotType.Hat: SkinManager.Instance.CurrentHatIndex = index; break;
            case SkinManager.SlotType.Hair: SkinManager.Instance.CurrentHairIndex = index; break;
            case SkinManager.SlotType.Clothes: SkinManager.Instance.CurrentClothesIndex = index; break;
            case SkinManager.SlotType.Broom: SkinManager.Instance.CurrentBroomIndex = index; break;
        }
        RefreshSlot(slot);
    }
    private void ApplyEntry(SkinManager.SlotType slot, SkinManager.SkinOptionEntry entry)
    {
        if (!groups.TryGetValue(slot, out var group) || group == null) return;
        var opt = entry?.option;
        if (opt == null) return;

        var mesh = opt.Mesh;
        var material = opt.Material;

        foreach (var target in group.targets)
        {
            if (target == null || target.renderer == null) continue;

            if (target.renderer is SkinnedMeshRenderer skinned)
            {
                if (target.applyMesh && mesh != null && skinned.sharedMesh != mesh)
                    skinned.sharedMesh = mesh;
                if (target.applyMaterial && material != null && skinned.sharedMaterial != material)
                    skinned.sharedMaterial = material;
            }
            else
            {
                if (target.applyMesh && target.meshFilter != null && mesh != null && target.meshFilter.sharedMesh != mesh)
                    target.meshFilter.sharedMesh = mesh;
                if (target.applyMaterial && material != null && target.renderer.sharedMaterial != material)
                    target.renderer.sharedMaterial = material;
            }
        }
    }
}