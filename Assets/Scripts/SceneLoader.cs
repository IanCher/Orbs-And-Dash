using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] FadeInOut fadeInOut;

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
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
        StartCoroutine(LoadSceneAfterFadeOutTime(GetActiveSceneIdx() + 1));
    }

    private IEnumerator LoadSceneAfterFadeOutTime(int sceneIdx)
    {
        fadeInOut.StartFadeOut();
        yield return new WaitForSecondsRealtime(fadeInOut.GetFadeOutTime());
        SceneManager.LoadScene(sceneIdx);
    }

    private int GetActiveSceneIdx()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
