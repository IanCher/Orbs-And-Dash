using UnityEngine;
using UnityEngine.UI;

public class SelectLevelUI : MonoBehaviour
{
    [SerializeField] private Image[] rareOrbCollected;

    private void Start()
    {
        for(int i = 0; i <= 2; i++)
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
    }
}
