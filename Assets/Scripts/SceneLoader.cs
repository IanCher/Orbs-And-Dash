using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] FadeInOut fadeInOut;
    MusicManager musicManager;

    void Start()
    {
        foreach(MusicManager mm in FindObjectsByType<MusicManager>(FindObjectsSortMode.None))
        {
            if (mm.IsTheOne())
            {
                musicManager = mm;
                musicManager.SelectSong(GetActiveSceneIdx());
                musicManager.FadeIn();
            }
        }
    }

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(LoadSceneAfterFadeOutTime(index));
    }

    public void ReloadCurrentScene()
    {
        int activeScene = GetActiveSceneIdx();
        StartCoroutine(LoadSceneAfterFadeOutTime(activeScene));
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadSceneAfterFadeOutTime(0));
    }

    public void LoadNextScene()
    {
        int sceneIdx = GetActiveSceneIdx();
        int nextSceneIdx = sceneIdx + 1 == SceneManager.sceneCountInBuildSettings ? 0 : sceneIdx + 1;
        StartCoroutine(LoadSceneAfterFadeOutTime(nextSceneIdx));
    }

    private IEnumerator LoadSceneAfterFadeOutTime(int sceneIdx)
    {
        fadeInOut.StartFadeOut();
        musicManager.FadeOut();
        yield return new WaitForSecondsRealtime(fadeInOut.GetFadeOutTime());
        SceneManager.LoadScene(sceneIdx);
    }

    private int GetActiveSceneIdx()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
