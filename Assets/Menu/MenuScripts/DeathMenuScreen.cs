using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuScreen : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI acquiredScore;


    void Start()
    {
    }

    void Update()
    {
        if (!PlayerHp.playerAlive && GameOverScreen.activeSelf == false)
        {
            scoreText.text = "You died and reached score " + acquiredScore.text.ToString(); 
            GameOverScreen.SetActive(true);
            GamePause.paused = true;
            GamePause.isGamePaused();
        }
        
    }
}
