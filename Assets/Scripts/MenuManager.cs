using UnityEngine;
using UnityEngine.UI;   

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject resetGameMenu;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        shopMenu.SetActive(false);
        resetGameMenu.SetActive(false);
    }

    public void ShowShopMenu()
    {
        mainMenu.SetActive(false);
        shopMenu.SetActive(true);
    }

    public void ShowResetGameMenu()
    {
        mainMenu.SetActive(false);
        resetGameMenu.SetActive(true);
    }
}
