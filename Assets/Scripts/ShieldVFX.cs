using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldVFX : MonoBehaviour
{
    [SerializeField] GameObject[] particleSystems = null;

    public void ActivateAllParticle()
    {
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.SetActive(true);
        }
    }

    void OnEnable()
    {
        ActivateAllParticle();
    }

    public void DeactivateOneParticleSystem()
    {
        foreach(var particleSystem in particleSystems)
        {
            if (particleSystem.activeSelf)
            {
                particleSystem.SetActive(false);
                break;
            }
        }
    }
}
