using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//We could here check all buffs and assign them to enemies cache values or something.
public class EnemyModifiers : MonoBehaviour
{
    [Header("Enemy modifiers")]
    public List<EnemyMod> eMods;
    //private bool setOn;
    private List<EnemyMod> enemyBuffsInUse = new List<EnemyMod>();
    private List<EnemyMod> overTimeEEffects = new List<EnemyMod>();
    //Need to create function/method that checks all things and we could call that when needed or at start
    void Start()
    {
        // for(int i = 0; i < eMods.Count; i++)
        // {
        //     print(eMods[i].name);
        // }
        addDebuff("test");
    }

    void Update()
    {
        //if(overTimes)foreach(var overEffect in overTimeEffects){overEffect.effect()}
    }
    public void addDebuff(string choice)
    {
        enemyBuffsInUse.Add(eMods.Find(up => up.name == choice));
    }
    //MIght need check different modi for Extra and OverTime
    public int eModiType(string type)
    {
        float modis = 0;
        foreach(var mod in enemyBuffsInUse)
        {
            if(mod.type == type)
            {
                modis += mod.flatModValue;
                if(mod.floatModValue > 0)
                {
                    modis *= mod.floatModValue;
                }
            }
        }
        return Convert.ToInt32(modis);
    }

    public void clearAllDebuffs()
    {
        enemyBuffsInUse.Clear();
    }
}
