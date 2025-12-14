using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;   

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject resetGameMenu;
    [SerializeField] private GameObject selectLevelMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject settingsMenu;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        shopMenu.SetActive(false);
        resetGameMenu.SetActive(false);
        selectLevelMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void Start()
    {
        InputSystem.actions.FindAction("Enter").performed += MainMenuUI_performed; ;

    }
    private void OnDestroy()
    {
        InputSystem.actions.FindAction("Enter").performed += MainMenuUI_performed; ;
    }

    private void MainMenuUI_performed(InputAction.CallbackContext obj)
    {
        sceneLoader.LoadNextScene();

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

    public void ShowSelectLevelMenu()
    {
        mainMenu.SetActive(false);
        selectLevelMenu.SetActive(true);
    }

    public void ShowCreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
