using UnityEngine;
using System;

public class MysterySpell : MonoBehaviour, ICollideable
{
    public static event Action<MysterySpellEffect> OnCollideWithMysterySpell;
    [SerializeField] GameObject[] assetsForVisualisation;
    [SerializeField] float alternatingTimeBetweenAssets = 0.5f;
    private float timeSinceLastAssetChange = 0;
    private int currentActiveAssetIdx = 0;


    protected virtual void Reset()
    {
        if (damageData == null) damageData = new PotionData();
        damageData.SetType(EffectType.MysterySpell);
    }

    protected virtual void OnValidate()
    {
        if (damageData == null) damageData = new PotionData();
        damageData.SetType(EffectType.MysterySpell);
    }


    [Header("MysterySpell Weights normalized to 100%")]
    [Min(0f)] public float takeDamageChance = 50f;
    [Min(0f)] public float shieldChance = 50f;

    [Header("Effect Data")]
    public PotionData damageData;
    public ShieldData shieldData;

    [Header("VFX And SFX")]
    [SerializeField] EffectClass shieldEffects = new EffectClass();
    [SerializeField] EffectClass damageEffects = new EffectClass();

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
            SpawnEffects(damageEffects);
        }
        else
        {
            effect = new MysterySpellEffect
            {
                effectType = MysterySpellEffectType.Shield,
                shieldData = shieldData
            };
            SpawnEffects(shieldEffects);
        }

        OnCollideWithMysterySpell?.Invoke(effect);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitter = other.gameObject;

        Debug.Log($"Hitted: {hitter.name}");

        if (hitter.CompareTag("Player"))
        {
            playerStats = hitter.GetComponent<PlayerStats>();
        }
    }
    PlayerStats playerStats;
    private void SpawnEffects(EffectClass fx)
    {
        AudioManager.instance.PlaySound(fx.SoundName);

        if (fx.Vfx != null)
        {
            if (fx.AttachToPlayer)
            {
                GameObject effect = Instantiate(
                  fx.Vfx,
                  transform.position,
                  transform.rotation
                );
                if (playerStats != null)
                {

                    effect.transform.SetParent(playerStats.transform);   // attacco all'oggetto Player
                    effect.transform.localPosition = Vector3.zero;  // opzionale: posizionalo relativo al centro del player
                    effect.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning("Player non trovato!");
                }


                Destroy(effect, 1f);
            }
            else if (fx.EnableOnPlayer)
            {
                fx.Vfx.SetActive(true);
            }
            else
            {
                GameObject effect = Instantiate(
                    fx.Vfx,
                    transform.position,
                    transform.rotation
                );
                Destroy(effect, 1f);
            }


        }
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

[Serializable]
public class EffectClass
{
    public GameObject Vfx;
    public DisappearingUi UIToSpawn;

    public bool AttachToPlayer;
#if UNITY_EDITOR
    [AtMostOneTrueWith(nameof(AttachToPlayer))]
#endif
    public bool EnableOnPlayer;

    public string SoundName;


}
