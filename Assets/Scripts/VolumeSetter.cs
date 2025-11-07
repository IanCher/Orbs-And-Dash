using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        slider.value = AudioManager.instance.GetVolume();
    }
    public void SetVolume(float volume)
    {
        AudioManager.instance.SetVolume(volume);
    }
}
