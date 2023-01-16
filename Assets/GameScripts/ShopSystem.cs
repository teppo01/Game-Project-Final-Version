using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ShopSystem : MonoBehaviour
{
    //switch this to shop UI
    public TMPro.TextMeshProUGUI tips;
    // We only want only change hp
    private PlayerHp hp;
    // We only wanna sybtract money for upgrades
    private PointSystem cash;
    // Only change damage, but later firespeed and range too. Or buy whole different gun...
    public ShootingLogic gun;

    //Need to have UI here to change values to shop at that time price
    [SerializeField]
    private int healthCost = 10;
    [SerializeField]
    private int dmgCost = 20;
    [SerializeField]
    private int sprintCost = 30;
    
    //If random from array is in our list of owned mods. Double the cost or something. Then *2, *3 and so on.
    void Start()
    {
        hp = GetComponent<PlayerHp>();
        cash = GetComponent<PointSystem>();
        tips.text = $"press 1 and {healthCost}P to get 1hp\npress 2 and {dmgCost}P to get 20dmg\npress 3 and {sprintCost}P to get message";
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && cash.pointCheck() >= healthCost)
        {
            hp.addHealth(1);
            healthCost += 5;
            cash.pointsChange(10);
            updateUI();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && cash.pointCheck() >= dmgCost)
        {
            dmgCost += 10;
            cash.pointsChange(15);
            updateUI();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && cash.pointCheck() >= sprintCost)
        {
            print("More speed");
            sprintCost += 100;
            cash.pointsChange(20);
            updateUI();
        }
    }

    private void updateUI()
    {
        tips.text = $"press 1 and {healthCost}points to get 1hp\npress 2 and {dmgCost}points to get 20dmg\npress 3 and {sprintCost}points to get message";
    }
}
*/