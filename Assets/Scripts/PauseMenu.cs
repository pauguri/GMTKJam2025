using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextButton resumeButton;
    [SerializeField] private TextButton quitButton;
    bool isPaused = false;

    private void Start()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.PauseEvent += HandlePauseEvent;
        }
        resumeButton.onClick += TogglePauseMenu;
        quitButton.onClick += ExitGame;
    }

    public void HandlePauseEvent(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (PlayerInputHook.Instance != null)
        {
            PlayerInputHook.Instance.PauseEvent -= HandlePauseEvent;
        }
        resumeButton.onClick -= TogglePauseMenu;
        quitButton.onClick -= ExitGame;
    }
}
