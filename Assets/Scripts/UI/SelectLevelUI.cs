using UnityEngine;
using UnityEngine.UI;

public class SelectLevelUI : MonoBehaviour
{
    [SerializeField] private Image[] rareOrbCollected;
    [SerializeField] Button level1Button;
    [SerializeField] Button level2Button;
    [SerializeField] Button level3Button;
    [SerializeField] Button bonusStageButton;


    private void OnEnable()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (PlayerData.RareOrbsTrack.Contains(i.ToString()))
            {
                rareOrbCollected[i].gameObject.SetActive(true);
            }
            else
            {
                rareOrbCollected[i].gameObject.SetActive(false);
            }

        }
        
        level1Button.gameObject.SetActive(PlayerData.IsIntroComplete > 0);
        level2Button.gameObject.SetActive(PlayerData.UnlockedLevel2 > 0);
        level3Button.gameObject.SetActive(PlayerData.UnlockedLevel3 > 0);
        bonusStageButton.gameObject.SetActive(PlayerData.IsOutroComplete > 0);
    }
    
}
