using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;
    
    //Maybe we need to open another thread for this... no idea how :D
    //Migh need to just spawn next to each other or inside
    public void spawnArmry(int armySize)
    {
        for(int i =0; i < armySize; i++)
        {
            GameObject clone = Instantiate(enemy, new Vector3(transform.position.x + i * 0.1f, transform.position.y, transform.position.z + i*0.1f), enemy.transform.rotation);
        }
    }
}
