using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float speedMultiplier = 1f;

    void Start()
    {
        PotionBomb.OnCollidedWithPotion += ApplyPotionEffects;
    }

    void OnDestroy()
    {
        PotionBomb.OnCollidedWithPotion -= ApplyPotionEffects;
    }

    void ApplyPotionEffects(PotionData potionData)
    {
        float reduceSpeedMultiplierBy = Mathf.Clamp01(potionData.PercentToSlowBy / 100f);
        speedMultiplier = 1f - reduceSpeedMultiplierBy;

        StopAllCoroutines();
        StartCoroutine(ResetSpeedAfter(potionData.SlowDuration));
    }

    IEnumerator ResetSpeedAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        speedMultiplier = 1f;
    }
}
