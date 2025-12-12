using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private static MusicManager instance;

    [Tooltip("The tracks should be in the same order as the scenes in the build profile.")]
    [SerializeField] AudioClip[] tracks;
    
    private float currentVolume = 0.5f;

    bool isFadingIn = false;
    bool isFadingOut = false;

    [SerializeField] float timeToFade = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = 0;
    }

    public bool IsTheOne()
    {
        return instance != null;
    }
    
    public void SelectSong(int idx)
    {
        audioSource.clip = tracks[idx];
    }

    public void SetCurrentVolume(float volume)
    {
        currentVolume = volume;

        if (!isFadingOut && !isFadingIn) audioSource.volume = volume;
    }
    public float GetCurrentVolume()
    {
        return currentVolume;
    }

    public void FadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
    }

    public void FadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            audioSource.volume += Time.deltaTime / timeToFade;
            if (audioSource.volume >= currentVolume)
            {
                audioSource.volume = currentVolume;
                isFadingIn = false;
            }

            if (!audioSource.isPlaying) audioSource.Play();
        }

        if (isFadingOut)
        {
            audioSource.volume -= Time.deltaTime / timeToFade;
            if (audioSource.volume <= 0)
            {
                audioSource.volume = 0;
                isFadingOut = false;
            }
        }
    }
}
