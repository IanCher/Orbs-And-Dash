using System;
using UnityEngine;

public class ParalysingSpell : MonoBehaviour, ICollideable
{
    public static event Action<PotionData> OnCollidedWithPotion;
    [Tooltip("For Percents, it is the value /100, so 5 is 5%")]
    public PotionData PotionsData;
    private bool wasHitByPlayer = false;

    [SerializeField] GameObject vfx;

    public void HandlePlayerCollision()
    {
        if (wasHitByPlayer) return;

        wasHitByPlayer = true;
        OnCollidedWithPotion?.Invoke(PotionsData);
        SlowPlayerEffects();
        Destroy(gameObject);
    }
    private void SlowPlayerEffects()
    {
        AudioManager.instance.PlaySound("ParalyzeSound");

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
