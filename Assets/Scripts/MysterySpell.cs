using UnityEngine;
using System;

public class MysterySpell : MonoBehaviour, ICollideable
{
    public static event Action<MysterySpellEffect> OnCollideWithMysterySpell;
    [SerializeField] GameObject[] assetsForVisualisation;
    [SerializeField] float alternatingTimeBetweenAssets = 0.5f;
    private float timeSinceLastAssetChange = 0;
    private int currentActiveAssetIdx = 0;

    [Header("MysterySpell Weights normalized to 100%")]
    [Min(0f)] public float takeDamageChance = 50f;
    [Min(0f)] public float shieldChance = 50f;

    [Header("Effect Data")]
    public PotionData damageData;
    public ShieldData shieldData;

    private bool wasHitByPlayer = false;

    void Start()
    {
        foreach (GameObject asset in assetsForVisualisation)
        {
            asset.SetActive(false);
        }
        assetsForVisualisation[0].SetActive(true);
    }

    void Update()
    {
        if (timeSinceLastAssetChange > alternatingTimeBetweenAssets)
        {
            assetsForVisualisation[currentActiveAssetIdx].SetActive(false);

            currentActiveAssetIdx++;
            if (currentActiveAssetIdx >= assetsForVisualisation.Length) currentActiveAssetIdx = 0;

            assetsForVisualisation[currentActiveAssetIdx].SetActive(true);
            timeSinceLastAssetChange = 0;
        }
        else
        {
            timeSinceLastAssetChange += Time.deltaTime;
        }
    }

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