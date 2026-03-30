using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reloads the active scene when the player presses R.
/// </summary>
public class Restart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();
    }

    /// <summary>Reloads the currently active scene.</summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
