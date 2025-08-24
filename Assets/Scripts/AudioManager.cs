using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private Sound[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        foreach (Sound sound in audioClips)
        {
            if (sound.name == name)
            {
                audioSource.PlayOneShot(sound.clip);
            }
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
