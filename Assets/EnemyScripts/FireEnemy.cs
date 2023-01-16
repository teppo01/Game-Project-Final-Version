using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//HUOMIO OMALLA VASTUULLA KATSOT KOODIA! JUMALA ON AINUT KUKA TIETÄÄ MITÄ TAPAHTUU
public class FireEnemy : MonoBehaviour
{

    [SerializeField]
    private GameObject fireball;    //Object that we create and shoot
    private Transform player;
    private EnemyHp life;
    //we might want to reduce destination calls later or in future. Stopping distance should be 1
    private NavMeshAgent agent;
    [SerializeField]
    private Transform headPosition;   //Can be used to set poisition for parent. After animation or during.
    private bool doingJ;
    private Vector3 endPoint;
    private Vector3 oldDestination;
    private float transferSpeed = 1;
    private float animationTime;
    private float distanceDivTime;
    private float jumpDistance;
    [SerializeField]
    private Transform wallpos;

    //Animaton
    private Animator anime;
    private int walkHash;
    private int runHash;
    private int attackHash;
    private int jumpHasj;
    private int climbHash;
    private bool wasClimb;

    [SerializeField]
    private Transform rayPosition;
    // [SerializeField]
    // private Vector3[] goodPlaces;    //was meant to be used for better place finder. Not in use

    [SerializeField]
    private float fireRange = 40;
    [SerializeField]
    private float reloadTime = 3;
    private float shootCd;
    private bool canShoot = false;
    private float roatationSpeed = 70;  //Käänytää nopeasti pelaajaa päin jotta voidaan ampua
    private int fuckThisPlace = 0;  //Kun tämä menee neljää me etsitää uusi paikka ampua
    private Vector3 fireAngle;  //Kulma

    private float stunTime;
    private bool isStunned = false;

    [Header("Roaming")]
    public static bool combatMode = false;
    public float roamRange = 10;
    public float waitTime = 10f;
    public float walkingSpeed = 4.5f;   //walking speed
    public float runSpeed = 8f;   //isnt used. But might in end point this will get buffs inside
    private bool niceSpot = false;  //boolean when we have spot or not
    private float canMoveTime;  //cd before we move

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        life = GetComponent<EnemyHp>();
        anime = GetComponent<Animator>();   //Change string to performance mode
        walkHash = Animator.StringToHash("Walk Target");
        runHash = Animator.StringToHash("Run");
        attackHash = Animator.StringToHash("Attack");
        climbHash = Animator.StringToHash("Climb");
        shootCd = Time.time + reloadTime;   //shooting cooldown
        canMoveTime = waitTime + Time.time; //Afk time
    }

    void Update()
    {
        if(life.living)
        {
            if(agent.isOnOffMeshLink || doingJ)
            {
                dropJump();
                return;
            }
            if(combatMode && !doingJ)
            {
                anime.SetBool(walkHash, false);
                agent.speed = runSpeed;
                combat();
                return;
            }
            if(!combatMode && !doingJ)
            {
                anime.SetBool(runHash, false);
                afkRoaming();
                return;
            }
        }
    }

    private void shoot()
    {
        //rayPosition.forward väärä. Pitää laske itse
        Vector3 angel = player.position - rayPosition.position;
        RaycastHit hit;
        Ray ray = new Ray(rayPosition.position, angel);
        //add later player mask if only sees something
        bool validTarget = Physics.Raycast(ray, out hit, fireRange);
        //Need && layer == player layer
        if(validTarget)
        {
            //Change player later
            if(hit.transform.gameObject.tag == "Player")
            {
                shootCd = Time.time + reloadTime;
                fireAngle = angel;
                canShoot = true;
            }
            else
            {
                print(fuckThisPlace);
                shootCd = Time.time + 2;
                fuckThisPlace ++;
                placeToShoot();
            }
        }
    }

    //WE might need to but timer here for one second becuause, navmesh agent aint free
    private void placeToShoot()
    {   
        if(Vector3.Distance(player.position, transform.position) > fireRange || fuckThisPlace == 3)
        {
            //It would be good if we have few spots and shoos the better one or closes one
            Vector3 point;
            if(RandomPoint(player.position, fireRange, out point))
            {
                // agent.SetDestination(new Vector3(0,0,0));
                agent.SetDestination(point);
                fuckThisPlace = 0;
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        //we could add if player is too close and closeTime is high to move away from player
    }

    //https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
    //out x tuottaa function/methodin jälkee x jotain arvoja
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    //Afk routines when player is not in combat
    private void afkRoaming()
    {
        //add all the && isWalking or something else to required.
        if(agent.remainingDistance < agent.stoppingDistance && !niceSpot)
        {
            anime.SetBool(walkHash, false);
            canMoveTime = Time.time + waitTime;
            niceSpot = true;
        }
        Vector3 point;
        if(Time.time > canMoveTime && RandomPoint(transform.position, roamRange, out point) && niceSpot)
        {
            agent.SetDestination(point);
            anime.SetBool(walkHash, true);
            niceSpot = false;
        }
    }
    //When Player is in combat
    private void combat()
    {
        if(!isStunned)
        {   
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                Vector3 direcetion = (player.position - transform.position).normalized;
                Quaternion playerLocation = Quaternion.LookRotation(direcetion);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, playerLocation, roatationSpeed * Time.deltaTime);
                if(Time.time >= shootCd)
                {
                    shoot();
                    if(canShoot)
                    {
                        anime.SetTrigger(attackHash);
                        Instantiate(fireball, rayPosition.position, Quaternion.LookRotation(fireAngle)); //Later pool this
                        canShoot = false;
                    }
                }
                else{placeToShoot();}
            }
            //voi olla että pitää muoka omaksi jutuksi miten toimii
            else
            {
                anime.SetBool(runHash, true);
                //agent.SetDestination(player.position);
            }
        }
        else if(Time.time >= stunTime)
        {
            isStunned = false;
        }
        else
        {
            //Play trigger for stun?
            agent.SetDestination(transform.position);
        }       
    }

 public void dropJump()
    {
        if(!doingJ)
        {   
            //Need to shoot ray from head to see where is wall and rotate towards it. Range 3f
            doingJ = true;
            oldDestination = agent.destination;
            endPoint = agent.currentOffMeshLinkData.endPos;
            agent.enabled = false;

            if(endPoint.y > transform.position.y)
            {
                wasClimb = true;
                rotateTowardsWall();
                moveTorwardWall();
                anime.SetTrigger(climbHash);    //tarkista että alkaa välittömästi
                transferSpeed = 3.3f;
                animationTime = Time.time + transferSpeed;
                distanceDivTime = endPoint.y/transferSpeed;
            }
            else
            {
                anime.SetTrigger("Jump");   //tarkista että alkaa välittömästi
                transferSpeed = 1f;
                animationTime = Time.time + transferSpeed;
                jumpDistance = Vector3.Distance(transform.position, endPoint);
                distanceDivTime = jumpDistance/transferSpeed;
                print("Jump");
            }
        }
        // else if(Vector3.Distance(transform.position, endPoint) < 0.1f)
        else if(Time.time > animationTime)
        {
            if(wasClimb)
            {
                anime.SetTrigger("DoneClimb");  //enable
                wasClimb = false;
            }
            else
            {
                anime.SetTrigger("Landing");    //enable
            }   
            print("Done");
            transform.position = endPoint;
            doingJ = false;
            agent.enabled = true;
            agent.SetDestination(oldDestination);
        }
        //teleport when courutine is done?
        //Need a lerp or slerp
        else
        {   
            //Voisi olla if climb me mennää vaan ylös kunnes done ja pieni tp? jos me hypätään niin sitten oikein
            //Ei käytetä toisen asteen yhtälöä tehdäksemme kaunista hyppyä
            //Alempi on kiipeäminen. Voidaan tarkistaa wasClimb
            if(wasClimb)
            {
                var x = endPoint - transform.position;
                transform.position = transform.position + Vector3.up * distanceDivTime * Time.deltaTime;
            }
            else
            {
                var x = endPoint - transform.position;
                transform.position = transform.position + x * distanceDivTime * Time.deltaTime;
            }
        }
    }
    //Rotate during climb
    private void rotateTowardsWall()
    {
        RaycastHit wall;
        Ray ray = new Ray(headPosition.position, headPosition.forward);
        //might add layer to ground
        if(Physics.Raycast(ray, out wall, 8f))
        {
            print("Hit wall with ray");
            transform.rotation = Quaternion.LookRotation(-wall.normal);
        }
    }
    //move closer towards wall
    private void moveTorwardWall()
    {
        transform.position = wallpos.position;
    }

    public void stunEnemy(float time)
    {
        isStunned = true;
        stunTime = Time.time + time;
    }

    //Public or void idk...
    public void killAgent()
    {
        agent.enabled = false;
    }
}
