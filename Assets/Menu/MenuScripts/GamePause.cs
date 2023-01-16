using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public static bool paused = false;
    private GameObject PauseMenu;
    private GameObject SettingsMenu;

    private void Start()
    {
        PauseMenu = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("PauseMenu").gameObject;
        SettingsMenu = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("SettingsMenu").gameObject;

        paused = false;
        isGamePaused();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerHp.playerAlive)
        {
            paused = !paused;
            PauseMenu.SetActive(paused);    
            isGamePaused();
            if (SettingsMenu.activeSelf == true)
            {
                SettingsMenu.SetActive(false);
            }
        }

    }

    public static void isGamePaused()
    {
        if (paused)
        {
            Time.timeScale = 0f;
            //AudioListener.pause = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            //AudioListener.pause = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
