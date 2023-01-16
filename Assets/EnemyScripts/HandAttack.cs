using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour
{
    public int zombieDamage = 2;
    //Add damage that value here. We will get it from parent
    public void zombieMeleeCheck()
    {
        Collider[] hitCollided = Physics.OverlapSphere(transform.position, 2f, 12);
        foreach(var hitted in hitCollided)
        {
            print(hitted.gameObject.name);
        }
    }
    private void OnTriggerEnter(Collider player)
    {
        if(player.transform.tag == "Player")
        {
            print("Hit player with animation");
            var play = player.gameObject.GetComponent<PlayerHp>();
            if(play)
            {
                play.decreeseHealth(zombieDamage);
            }
        }
    }
}
