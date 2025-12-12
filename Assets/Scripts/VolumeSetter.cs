using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    MusicManager musicManager;

    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void GetMusicManager()
    {
        foreach(MusicManager mm in FindObjectsByType<MusicManager>(FindObjectsSortMode.None))
        {
            if (mm.IsTheOne())
            {
                musicManager = mm;
            }
        }
    }

    void OnEnable()
    {
        if (musicManager == null) GetMusicManager();
        slider.value = musicManager.GetCurrentVolume();
    }

    public void SetVolume(float volume)
    {
        musicManager.SetCurrentVolume(volume);
    }
}
