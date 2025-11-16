using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject canvasForPauseMenu;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference pauseActionRef;
    [SerializeField] GameObject soundMenu;
    [SerializeField] GameObject commandMenu;
    [SerializeField] GameObject pauseButtons;
    [SerializeField] SceneLoader sceneLoader;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void OnEnable()
    {
        pauseActionRef.action.performed += OnPausePerformed;
        pauseActionRef.action.Enable();
    }

    private void OnDisable()
    {
        pauseActionRef.action.performed -= OnPausePerformed;
        pauseActionRef.action.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused) ShowPauseButtons();

        Time.timeScale = isPaused ? 0f : 1f;
        canvasForPauseMenu.SetActive(isPaused);

        // Cursor.visible = isPaused;
        // Cursor.lockState = isPaused
        //     ? CursorLockMode.None
        //     : CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        if (isPaused)
            TogglePause();
    }

    public void RestartLevel()
    {
        ResumeGame();
        sceneLoader.ReloadCurrentScene();
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        sceneLoader.BackToMenu();
    }

    public void ShowPauseButtons()
    {
        pauseButtons.SetActive(true);
        soundMenu.SetActive(false);
        commandMenu.SetActive(false);
    }

    public void ShowSoundMenu()
    {
        pauseButtons.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void ShowCommandMenu()
    {
        commandMenu.SetActive(true);
        pauseButtons.SetActive(false);
    }
}
