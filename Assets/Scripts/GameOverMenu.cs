using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles Game Over screen button actions: restart, return to main menu, and quit.
/// </summary>
public class GameOverMenu : MonoBehaviour
{
    private const string GameSceneName = "FinalGameScene";
    private const string MainMenuSceneName = "MainMenu";

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
