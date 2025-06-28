using UnityEngine;
using TMPro;

public class PlayerOrbCollector : MonoBehaviour
{
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
            orbCount += orb.GetValue();
            orb.SetWasCollected(true);

            text.text = orbCount.ToString();
        }

        Destroy(orb.gameObject);
    }
}
