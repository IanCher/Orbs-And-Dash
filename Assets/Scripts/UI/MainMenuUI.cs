using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI normalOrbCountText;
    [SerializeField] private TextMeshProUGUI rareOrbsCountText;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });

        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        SetProgressionText();
    }

    public void SetProgressionText()
    {
        normalOrbCountText.text = "NORMAL ORBS : " + PlayerData.NormalOrbs;
        rareOrbsCountText.text = "RARE ORBS : " + PlayerData.RareOrbs +"/4";
    }
}
