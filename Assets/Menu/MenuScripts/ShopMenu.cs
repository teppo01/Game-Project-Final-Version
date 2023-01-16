using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    [Header("Passives")]
    [SerializeField] private GameObject firstPassive;
    [SerializeField] private GameObject secondPassive;
    [SerializeField] private GameObject thirdPassive;
    private TextMeshProUGUI[][] textFields = new TextMeshProUGUI[3][];

    private List<Modifiers> shopItems = new();

    [Header("Menus")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject popUp;
    [SerializeField] private Image background; 

    [Header("Reset button")]
    [SerializeField] private TextMeshProUGUI rerollButtonText;
    [SerializeField] private int rerollPrice = 10;
    
    private TextMeshProUGUI[] chosenBuff;
    private PlayerModifiers playerModifiers;
    private MoneySystem moneySystem;

    void Start()
    {
        moneySystem = GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<MoneySystem>();
        playerModifiers = GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<PlayerModifiers>();

        textFields[0] = firstPassive.GetComponentsInChildren<TextMeshProUGUI>();
        textFields[1] = secondPassive.GetComponentsInChildren<TextMeshProUGUI>();
        textFields[2] = thirdPassive.GetComponentsInChildren<TextMeshProUGUI>();
        rerollButtonText.text = $"Reroll ({rerollPrice})";

        RandomizeShopItems();
        GenerateShop(shopItems);

        StartCoroutine(FadeBackground()); 
    }

    private void RandomizeShopItems()
    {
        // pseudo-randomly order, then pick 3 as a list to show in the shop 
        var rng = new System.Random();
        shopItems = playerModifiers.buffs.OrderBy(addModifier => rng.Next()).Take(3).ToList();
    }

    private void GenerateShop(List<Modifiers> shopItems)
    {
        int counter = 0;
        string text = "";
        foreach (var item in shopItems)
        {
            for (int i = 0; i<textFields.Count(); i++)
            {
                switch(i)
                {
                    case 0:
                        text = item.name;
                        break;
                    case 1:
                        text = item.effectDesc;
                        break;
                    case 2:
                        text = "Cost: " + item.cost.ToString();
                        break;
                }

                textFields[counter][i].text = text;
            }
            counter += 1;
        }
    }

    public void PurchaseButton(string upgradeNumber)
    {
        switch (upgradeNumber)
        {
            case "0":
                chosenBuff = textFields[0];
                break;
            case "1":
                chosenBuff = textFields[1];
                break;
            case "2":
                chosenBuff = textFields[2];
                break;
        }
        int buffCost = Int32.Parse(chosenBuff[2].text.Split(' ')[1]);

        if (moneySystem.MoneyCheck() >= buffCost) 
        {
            moneySystem.MoneyChange(buffCost);
            print("Purchased " + chosenBuff[0].text);
            playerModifiers.addModifier(chosenBuff[0].text);
            RandomizeShopItems();
            GenerateShop(shopItems);
            // kommentoi pois jos halutaan että voi ostaa vain yhden passiven
            // shopMenu.SetActive(false);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            StartCoroutine(PopUpDelay());
        }
    }

    public void ResetButton()
    {
        if (moneySystem.MoneyCheck() >= rerollPrice) 
        {
            moneySystem.MoneyChange(rerollPrice);
            RandomizeShopItems();
            GenerateShop(shopItems);
        }
        else 
        {
            StartCoroutine(PopUpDelay());
            print("Not enough money for reroll!");
        }
    }

    public void CloseButton()
    {
        shopMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator PopUpDelay() 
    {
        popUp.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        popUp.SetActive(false);
    }

    IEnumerator FadeBackground()
    {
        for (float i = 0; i <= 1; i += 0.0025f)
        {
            background.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

}
