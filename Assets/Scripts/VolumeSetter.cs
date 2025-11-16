using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        slider.value = soundSource.volume;
    }

    public void SetVolume(float volume)
    {
        soundSource.volume = volume;
    }
}
