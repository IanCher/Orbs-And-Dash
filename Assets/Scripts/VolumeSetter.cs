using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    enum AudioManagerType { SFX, Music }; 
    
    [SerializeField] AudioManagerType audioManagerType;

    AudioSource audioSource;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void OnEnable()
    {
        if (audioSource == null)
        {
            if (audioManagerType == AudioManagerType.SFX)
            {
                audioSource = AudioManager.instance.GetComponent<AudioSource>();
            }
            else
            {
                audioSource = MusicManager.instance.GetComponent<AudioSource>();
            }
        }
        slider.value = audioSource.volume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        if (audioManagerType == AudioManagerType.Music)
        {
            MusicManager.instance.SetCurrentVolume(volume);
        }
    }
}
