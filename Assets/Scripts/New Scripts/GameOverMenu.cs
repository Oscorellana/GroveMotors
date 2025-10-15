using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene"); // replace with your actual game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
