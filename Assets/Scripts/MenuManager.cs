using UnityEngine;
using UnityEngine.UI;   

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        shopMenu.SetActive(false);
    }

    public void ShowShopMenu()
    {
        mainMenu.SetActive(false);
        shopMenu.SetActive(true);
    }
}
