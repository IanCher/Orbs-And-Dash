using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] FadeInOut fadeInOut;
    public void ReloadCurrentScene()
    {
        StartCoroutine(LoadSceneAfterFadeOutTime(GetActiveSceneIdx()));
    }

    public void BackToMenu()
    {
        if (Time.timeScale < 1e-8)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        StartCoroutine(LoadSceneAfterFadeOutTime(0));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneAfterFadeOutTime(GetActiveSceneIdx() + 1));
    }

    private IEnumerator LoadSceneAfterFadeOutTime(int sceneIdx)
    {
        fadeInOut.StartFadeOut();
        yield return new WaitForSeconds(fadeInOut.GetFadeOutTime());
        SceneManager.LoadScene(sceneIdx);
    }

    private int GetActiveSceneIdx()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
