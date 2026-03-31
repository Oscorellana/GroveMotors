using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu button actions: play, instructions panel toggle, and quit.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private const string GameSceneName = "GM";

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject instructionsPanel;

    /// <summary>Loads the main game scene.</summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    /// <summary>Shows the instructions panel and hides the main menu.</summary>
    public void OpenInstructions()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    /// <summary>Shows the main menu panel and hides the instructions panel.</summary>
    public void BackToMenu()
    {
        instructionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    /// <summary>Quits the application.</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
