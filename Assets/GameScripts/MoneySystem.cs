using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    private TMPro.TextMeshProUGUI moneyText;
    private int money;
    void Start()
    {
        moneyText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("PlayerMoney").GetComponent<TMPro.TextMeshProUGUI>();
        money = PlayerPrefs.GetInt("money");
        moneyText.text = money.ToString(); 
        
    }

    public void addMoney(int killPoints)
    {
        money += killPoints;
        PlayerPrefs.SetInt("money", money);
        moneyText.text = money.ToString();
    }

    public int MoneyCheck() 
    {
        return money;
    }
    public void MoneyChange(int cost)
    {
        money -= cost;
        PlayerPrefs.SetInt("money", money);
        moneyText.text = money.ToString();
    }
}
