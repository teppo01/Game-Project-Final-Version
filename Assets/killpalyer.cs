using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killpalyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider cool)
    {
        if(cool.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<PlayerHp>().decreeseHealth(1000);
        }
    }
}
