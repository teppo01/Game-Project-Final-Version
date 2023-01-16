using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelScript : MonoBehaviour
{
    GameObject passiveScreen;
    Image background;

    void Start()
    {
        passiveScreen = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("PassiveShopMenu").gameObject;
    }

        
    private void OnTriggerEnter(Collider col)
    {
        
        //Better would be TAg
        if (col.gameObject.tag == "Player")
        {
            GamePause.paused = true;
            GamePause.isGamePaused();
            passiveScreen.SetActive(true);
        }
    }
}
