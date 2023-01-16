using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Get all spawnpoints
//Add fixed amount of enemies
//Static value of alive enemies go zero, or list of alive enemies go zero or time goes zero spawn new enemy wave.
//Add more speed for enemies and enemies that last wave.
public class RoundSystem : MonoBehaviour
{
    List<GameObject> enemys = new List<GameObject>();
    public static int AmountOfZombies; //Every spawn rises this and every death decrease this
    private EnemyModifiers modit;    //Check if there is some interesting ways to add this here with debuff

    [Header("Spawnning settings")]
    [SerializeField]
    private int maxAnountofZombies = 100;
    public int army = 0;    //Horde size when normal. size rises by one every call
    [SerializeField]
    private float betweenWaves = 15; //How much we add before next.
    [Header("Escalation settings")]
    public bool escalation; //after player gets object done, game change harder.
    public int fixedSpawnTime; //Spawning time
    public int fixedAmount;    //Horde size during escalation time. Fixed amount
    //add method to rise to death amount and make waves come faster
    //Maybe limit check would be good so we would not tank our fps to 1
    //add way to check if theyr are combat or not.
    private float nextWave; //Time.time + betweenWaves. 
    void Start()
    {
        modit = GameObject.FindWithTag("Gamelogic").GetComponent<EnemyModifiers>();
        //BerweenWaves += modit.useMods(x, y, z);
        nextWave = Time.time + betweenWaves;
        var spawns = GetComponentsInChildren<SpawnEnemies>();
        foreach(var x in spawns)
        {
            enemys.Add(x.gameObject);
            x.spawnArmry(army);
        }
    }
    //Check why prints next way + + +. And not just nextWave is =10, 9, 8 and so on
    //asked to create timer when to start and when to turn spawning up
    //Abuse mahdolisuus tällä hetkellä
    void Update()
    {
        if(Time.time >= nextWave && !escalation && AmountOfZombies < maxAnountofZombies)
        {
            print(nextWave);
            spawnNewWave(army);
            nextWave = Time.time + betweenWaves;
            return;
        }
        if(Time.time >= nextWave && escalation && AmountOfZombies < maxAnountofZombies)
        {
            fixedSpawn(fixedAmount);
            nextWave = Time.time + fixedSpawnTime;
        }
    }
    //mahdollisesti korjaa vähän tätä balancea horden spawnaus nopeudessa
    private void spawnNewWave(int x)
    {
        foreach(var y in enemys)
        {
            var n = y.GetComponent<SpawnEnemies>();
            n.spawnArmry(x);
        }
        if(betweenWaves >= 25)
        {
            betweenWaves -= 1;
        }
        else{betweenWaves = 25;}
        army ++;
    }

    private void fixedSpawn(int x)
    {
        foreach(var y in enemys)
        {
            var n = y.GetComponent<SpawnEnemies>();
            n.spawnArmry(x);
        }
    }

    public static void zombieSpawned()
    {
        AmountOfZombies ++;
    }
    public static void zombieKilled()
    {
        AmountOfZombies --;
    }
}
