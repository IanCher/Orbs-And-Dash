using UnityEngine;
using TMPro;
using System;
using static Orb;

public class PlayerOrbCollector : MonoBehaviour
{
    public static event Action<OrbType> OnOrbCollected;
    public static event Action<PotionData> OnCollidedWithPotion;


    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PotionBomb>(out PotionBomb potionBomb))
        {
            OnCollidedWithPotion?.Invoke(potionBomb.PotionsData);
            other.gameObject.SetActive(false);
        }

        if (!other.TryGetComponent<Orb>(out Orb orb)) return;

        if (!orb.WasCollected())
        {
            OnOrbCollected?.Invoke(orb.GetOrbType());
            orb.SetWasCollected(true);
        }

        Destroy(orb.gameObject);
    }
}
