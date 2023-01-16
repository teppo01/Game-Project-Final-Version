using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemymod", menuName = "EnemyMod")]
public class EnemyMod : ScriptableObject
{
    [Header("Enemy Modifier")]
    public new string name;
    public string effectDesc;
    //Typee should be {"Damage", "Health", "Mobility", "Extra", "Over Time"}, but no idea how to make dropdown menu for this.
    public string type;
    //How much reward you get.
    public int reward;
    //Float maybe better. For upgrade with int, we should chage first int before adding.
    public float floatModValue;
    //for flat values
    public int flatModValue;
    //If we have more fuckery effect that we dont want to take space from somewhere else we could add here
    //public virtual int effect(int value){return value;} //we can do many things inside here
}
