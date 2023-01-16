using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxFixing : MonoBehaviour
{
    //add que sound to tell Done. For now
    public ObjectiveManager m;
    private float requiredTime = 6f;
    private float currentTime = 0f;
    private bool isDone = false;
    private bool isInside;

    private new Light light;    //We attach light in ObjectiveManager
    private Image backG;
    private Slider progressBar;

    private MoneySystem moneySystem;

    void Start()
    {
        moneySystem = GameObject.FindWithTag("Gamelogic").GetComponent<MoneySystem>();
        light = GetComponentInChildren<Light>();
        backG = GameObject.Find("ProgressBar").GetComponent<Image>();
        backG.enabled = false;
        progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
        progressBar.enabled = false;
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == 12)
        {
            if(!isDone)
            {
                // currentTime = 0f;
                progressBar.value = currentTime;
                switchStatus(true);
            }
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if(c.gameObject.layer == 12)
        {
            // currentTime = 0f;
            progressBar.value = 0;
            switchStatus(false);
        }
    }
    //should we disable collider when its done? Think it about in the future
    private void OnTriggerStay(Collider c)
    {
        if(c.gameObject.layer == 12)
        {
            currentTime += Time.deltaTime;

            if(!isDone)
            {
                progressBar.value = currentTime/requiredTime;
            }

            if(currentTime > requiredTime && !isDone)
            {
                m.objectiveSuccess();
                FindObjectOfType<AudioManager>().Play("powerOn");
                moneySystem.addMoney(25);
                light.color = Color.green;
                backG.enabled = false;
                progressBar.value = 0;
                progressBar.enabled = false;
                isDone = true;
            }
        }
    }

    private void switchStatus(bool status)
    {
        backG.enabled = status;
        progressBar.enabled = status;
    }
}
