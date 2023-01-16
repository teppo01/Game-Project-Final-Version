using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    //Oletamme et tämä on pelaajassa kiini, jos ei ole niin pitäisi luoda TAG pelaajalle ja ottaa sen transformi.
    //Voisi olla ampuminenkin et voidaa ampua itemme jonnekin ja pitää olla valid ground mask sijainti
    private CharacterController player;
    private Vector3 tpLocation;
    private bool locationSet = false;
    private float tpCD = 10;
    private float tpTime;
    
    void Start()
    {
        player = GetComponent<CharacterController>();
        tpTime = Time.time;
        tpLocation = new Vector3(0,0,0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Time.time >= tpTime)
        {
            if(locationSet)
            {
                tpTime = Time.time + tpCD;
                tpThere();
                locationSet = false;
            }
            else
            {
                safeMyLocation(transform.position);
                locationSet = true;
            }
        }
    }

    private void safeMyLocation(Vector3 position)
    {
        tpLocation = new Vector3(position.x, position.y, position.z);
        print($"Location set to {tpLocation}.");
    }

    private void tpThere()
    {
        //Might need to check with phyci that there is place to tp and if not maybe tp little bit up.
        print($"Teleportin to {tpLocation}");
        player.enabled = false;
        transform.position = tpLocation;
        player.enabled = true;
    }
}
