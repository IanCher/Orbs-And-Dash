using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerRuntimeModelApplier : MonoBehaviour
{
    [Serializable]
    private class SlotRenderGroup
    {
        public SkinManager.SlotType slot;
        public List<SlotRenderTarget> targets = new List<SlotRenderTarget>();
    }

    [Serializable]
    private class SlotRenderTarget
    {
        public Renderer renderer;
        public MeshFilter meshFilter;
        public bool applyMesh = true;
        public bool applyMaterial = true;
    }

    [SerializeField] private List<SlotRenderGroup> slotRenderGroups = new List<SlotRenderGroup>();
    [SerializeField] private bool retrySubscribeIfMissing = true;

    private readonly Dictionary<SkinManager.SlotType, SlotRenderGroup> groups = new Dictionary<SkinManager.SlotType, SlotRenderGroup>();
    private bool subscribed;
    private Coroutine deferredCoroutine;
    private bool initialApplied;

    private void Awake()
    {
        BuildLookup();
        AttemptImmediateApply(); 
    }
    private void OnEnable()
    {
        if (!subscribed)
            TrySubscribe();

        if (!subscribed && retrySubscribeIfMissing)
            deferredCoroutine = StartCoroutine(DeferredSubscribe());

        if (subscribed && !initialApplied)
        {
            RefreshAll();
            initialApplied = true;
        }
    }
    private void OnDisable()
    {
        if (subscribed && SkinManager.Instance != null)
            SkinManager.Instance.SkinSelectionChanged -= OnSkinSelectionChanged;

        subscribed = false;

        if (deferredCoroutine != null)
            StopCoroutine(deferredCoroutine);
        deferredCoroutine = null;
    }
    private void BuildLookup()
    {
        groups.Clear();
        foreach (var group in slotRenderGroups)
        {
            if (group == null) continue;
            groups[group.slot] = group;
        }
    }
    private void AttemptImmediateApply()
    {
        var sm = SkinManager.Instance;
        if (sm == null) return;

        TrySubscribe();
        RefreshAll();
        initialApplied = true;
    }
    private IEnumerator DeferredSubscribe()
    {
        float timeout = 1f;
        while (!subscribed && timeout > 0f)
        {
            TrySubscribe();
            if (subscribed)
            {
                if (!initialApplied)
                {
                    RefreshAll();
                    initialApplied = true;
                }
                yield break;
            }
            timeout -= Time.unscaledDeltaTime;
            yield return null;
        }
        deferredCoroutine = null;
    }
    private void TrySubscribe()
    {
        if (subscribed) return;
        var sm = SkinManager.Instance;
        if (sm == null) return;
        sm.SkinSelectionChanged -= OnSkinSelectionChanged;
        sm.SkinSelectionChanged += OnSkinSelectionChanged;
        subscribed = true;
    }
    private void OnSkinSelectionChanged(SkinManager.SlotType slot, SkinManager.SkinOptionEntry entry)
    {
        ApplyEntry(slot, entry);
    }
    public void RefreshAll()
    {
        if (SkinManager.Instance == null) return;
        foreach (var kvp in groups)
            ApplyEntry(kvp.Key, SkinManager.Instance.GetCurrentEntry(kvp.Key));
    }
    public void RefreshSlot(SkinManager.SlotType slot)
    {
        if (SkinManager.Instance == null) return;
        ApplyEntry(slot, SkinManager.Instance.GetCurrentEntry(slot));
    }
    public void ApplySkinPreview(SkinOption option)
    {
        if (option == null) return;
        ApplyEntry(option.SlotType, new SkinManager.SkinOptionEntry { option = option });
    }
    private void ApplyEntry(SkinManager.SlotType slotType, SkinManager.SkinOptionEntry skinEntry)
    {
        if (!groups.TryGetValue(slotType, out var slotGroup) || slotGroup == null) return;

        var skinOption = skinEntry?.option;
        if (skinOption == null) return;

        Mesh desiredMesh = skinOption.Mesh;
        Material desiredMaterial = skinOption.Material;

        var renderTargets = slotGroup.targets;
        if (renderTargets == null || renderTargets.Count == 0) return;

        for (int i = 0; i < renderTargets.Count; i++)
        {
            var renderTarget = renderTargets[i];
            if (renderTarget == null || renderTarget.renderer == null) continue;
            ApplyToRenderTarget(renderTarget, desiredMesh, desiredMaterial);
        }
    }
    private static void ApplyToRenderTarget(SlotRenderTarget renderTarget, Mesh mesh, Material material)
    {
        var targetRenderer = renderTarget.renderer;

        if (targetRenderer is SkinnedMeshRenderer skinnedRenderer)
        {
            if (renderTarget.applyMesh && mesh != null && skinnedRenderer.sharedMesh != mesh)
                skinnedRenderer.sharedMesh = mesh;

            if (renderTarget.applyMaterial && material != null && skinnedRenderer.sharedMaterial != material)
                skinnedRenderer.sharedMaterial = material;
            return;
        }

        if (renderTarget.applyMesh && renderTarget.meshFilter != null && mesh != null && renderTarget.meshFilter.sharedMesh != mesh)
            renderTarget.meshFilter.sharedMesh = mesh;

        if (renderTarget.applyMaterial && material != null && targetRenderer.sharedMaterial != material)
            targetRenderer.sharedMaterial = material;
    }
}