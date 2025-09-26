using System;
using UnityEngine;

public class BaseSpell : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    [SerializeField] GameObject vfx;
    [SerializeField] string soundName;

    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(PotionsData);
        SpawnEffects();
        Destroy(gameObject);
    }
    private void SpawnEffects()
    {
        AudioManager.instance.PlaySound(soundName);

        if (vfx != null)
        {

            GameObject effect = Instantiate(
                vfx,
                transform.position,
                transform.rotation
            );
            
            Destroy(effect, 1f);
        }


       
    }
}
