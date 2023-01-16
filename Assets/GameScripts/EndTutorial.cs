using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTutorial : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        
            SceneManager.LoadScene(sceneName: "MainMenu");

            GamePause.paused = true;
            GamePause.isGamePaused();

    }
}
