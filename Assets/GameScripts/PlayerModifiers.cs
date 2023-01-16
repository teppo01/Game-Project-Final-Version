using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Tag de_buff
//We would like to have here modifiers saved for level to level if we need to load different scenes/levels and can smooth transist
//If perfomance tanks down we could to saved value or something
public class PlayerModifiers : MonoBehaviour
{
    [Header("Pelissa saatavat upgradet(Array)")]
    public List<Modifiers> buffs;
    private List<Modifiers> inUseBuffs = new List<Modifiers>();
    private List<Modifiers> overTimes = new List<Modifiers>();

    //if(overTimes){on = true}
    //private float perS;
    //private float tickRate = 5;
    //Private gameObject/Transform player; // Use Tag to find them so we can always use plyaer
    private void Awake()
    {
        inUseBuffs = SaveBuffs.playerBuffs;
    }

    void Start()
    {

        for(int i = 0; i < inUseBuffs.Count; i ++)
        {
            print("in use " + inUseBuffs[i].name);
        }
    }

    //We could here check for virtual method that is use and make them happen here
    void Update()
    {
        //if(overTimes)foreach(var overEffect in overTimeEffects){overEffect.effect()}
    }

    //Voisi olla jokin random valikointi jos sellainen on vai miten se tehdää sitten

    public void addModifier(string upgrade)
    {
        SaveBuffs.playerBuffs.Add(buffs.Find(up => up.name == upgrade)); //find ottaa function/methodin
    }

    public int playerBasicModi(string type, int currentValue)
    {
        float modis = 0 + currentValue;
        foreach(var mod in inUseBuffs)
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
  
    //Found way to add cool shit
    public int extra(string type, int value)
    {
        float modis = 0;
        foreach(var mod in inUseBuffs)
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
}
