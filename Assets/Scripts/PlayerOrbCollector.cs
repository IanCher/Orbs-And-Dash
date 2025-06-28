using UnityEngine;
using TMPro;
using System;
using static Orb;

public class PlayerOrbCollector : MonoBehaviour
{
    public static event Action<OrbType> OnOrbCollected;

    [SerializeField] TMP_Text text;
    int orbCount = 0;

    void Start()
    {
        text.text = "0";
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Orb>(out Orb orb)) return;

        if (!orb.WasCollected())
        {
            OnOrbCollected?.Invoke(orb.GetOrbType());
            orbCount += orb.GetValue();
            orb.SetWasCollected(true);

            text.text = orbCount.ToString();
        }

        Destroy(orb.gameObject);
    }
}
