using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private SceneLoader loader;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button cureButton;
    [SerializeField] private TextMeshProUGUI normalOrbCountText;
    [SerializeField] private TextMeshProUGUI rareOrbsCountText;
    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        if(PlayerData.IsOutroComplete == 1)
        {
            cureButton.gameObject.SetActive(false);
        }

        startButton.onClick.AddListener(() =>
        {
            if (PlayerData.IsIntroComplete == 0)
                loader.LoadNextScene();
            else
            {
                loader.LoadSceneByIndex(2);
            }
        });

        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        cureButton.onClick.AddListener(() =>
        {
            if(PlayerData.RareOrbs >= 3)
            {
                sceneLoader.LoadSceneByIndex(5);
            }
            else
            {
                StartCoroutine(AnimateText());
            }
        });

        SetProgressionText();
    }

    private float speed = 40;
    private IEnumerator AnimateText()
    {
        while(rareOrbsCountText.fontSize <= 35)
        {
        rareOrbsCountText.fontSize += Time.deltaTime * speed;
            yield return null;
        }

        while (rareOrbsCountText.fontSize >= 30)
        {
            rareOrbsCountText.fontSize -= Time.deltaTime * speed;
            yield return null;
        }

    }


    public void SetProgressionText()
    {
        normalOrbCountText.text = "ORBS : " + PlayerData.NormalOrbs;
        rareOrbsCountText.text = "Magical ORBS : " + PlayerData.RareOrbs +"/3";
    }
}
