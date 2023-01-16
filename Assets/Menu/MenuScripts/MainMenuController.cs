using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayClick()
    {
        PlayerPrefs.SetInt("money", 0);
        PlayerPrefs.SetInt("points", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitClick()
    {
        print("Pressed exit!");
        Application.Quit();
    }
}
