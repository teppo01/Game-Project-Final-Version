using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void MainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitClick()
    {
        print("Pause menu quit");
        Application.Quit();
    }
}
