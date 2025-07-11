using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject canvasForPauseMenu;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference pauseActionRef;

    private bool isPaused = false;

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

        Time.timeScale = isPaused ? 0f : 1f;
        canvasForPauseMenu.SetActive(isPaused);

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        if (isPaused)
            TogglePause();
    }
}
