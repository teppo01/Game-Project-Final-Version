using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    private TMPro.TextMeshProUGUI hpText;
    private int health = 10;
    private float invinsibleFrames = 0.3f;
    private float canTakeDmg;
    public static bool playerAlive = true;

    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<PlayerModifiers>().playerBasicModi("HEALTH", health);
        hpText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("PlayerHP").GetComponent<TMPro.TextMeshProUGUI>();
        hpText.text = health.ToString();
        playerAlive = true;
        canTakeDmg = Time.time;
    }

    public void decreeseHealth(int damage)
    {
        if(Time.time >= canTakeDmg)
        {
            FindObjectOfType<AudioManager>().Play("TakingDamage");
            health -= damage;
            if(health >= 1)
            {
                hpText.text = health.ToString();
                canTakeDmg = Time.time + invinsibleFrames;
            }
            else
            {
                // playerAlive aktivoi GamePause.cs pausen ja kuolemascreenin 
                hpText.text = health.ToString();
                playerAlive = false;
            }
        }
    }

    public void addHealth(int amount)
    {
        health += amount;
        hpText.text = health.ToString();
    }
}
