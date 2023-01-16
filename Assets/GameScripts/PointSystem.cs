using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    private TMPro.TextMeshProUGUI pointText;
    public PlayerModifiers modit;
    private int points;
    void Start()
    {
        pointText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("PlayerPoints").GetComponent<TMPro.TextMeshProUGUI>();
        points = PlayerPrefs.GetInt("points");
        pointText.text = points.ToString(); 

    }

    public void addPoints(int killPoints)
    {
        points += killPoints;
        PlayerPrefs.SetInt("points", points);
        pointText.text = points.ToString();
    }
}
