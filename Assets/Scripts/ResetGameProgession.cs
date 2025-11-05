using UnityEngine;

public class ResetGameProgession : MonoBehaviour
{
    [SerializeField] MainMenuUI mainMenuUI;

    public void ResetGame()
    {
        PlayerPrefs.DeleteKey(PlayerData.NORMAL_ORBS);
        PlayerPrefs.DeleteKey(PlayerData.RARE_ORBS);
        PlayerPrefs.DeleteKey(PlayerData.RARE_ORBS_Track);

        mainMenuUI.SetProgressionText();
    }
}
