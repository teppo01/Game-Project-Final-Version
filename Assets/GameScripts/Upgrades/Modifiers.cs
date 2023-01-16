using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nopea tapa tehdä valmiita dataobjetta joita sitten käyttää objeteissa tai lukea muualla käytössä
//Tarkista voidaanko lisätä jotain methodeja
[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifier")]
public class Modifiers : ScriptableObject
{
    [Header("Upgrade stats/effects")]
    public new string name;
    public string effectDesc;
    //Typee should be {"Damage", "Health", "Mobility", "Extra", "OverTime"}, but no idea how to make dropdown menu for this.
    public string type;
    //How much it might cost or what it's value
    public int cost;
    //Float maybe better. For upgrade with int, we should chage first int before adding.
    public float floatModValue;
    public int flatModValue;
    //If we have more fuckery effect that we dont want to take space from somewhere else we could add here
    //public virtual int effect(int value){return value;} //we can do many things inside here
}
