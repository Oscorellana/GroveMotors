using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles Win screen button actions: restart game, return to main menu, and quit.
/// </summary>
public class WinMenu : MonoBehaviour
{
    private const string GameSceneName = "GM";
    private const string MainMenuSceneName = "MainMenu";

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>Reloads the main game scene.</summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    /// <summary>Loads the main menu scene.</summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    /// <summary>Quits the application.</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
