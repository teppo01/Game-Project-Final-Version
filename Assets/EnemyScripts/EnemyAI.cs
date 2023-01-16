using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//HUOMI. KOODI ON PASKAA ALHAALLA KIITOS ANIMAATIO KOODI JOKA PITÄISI VARMAA OLLA OMA! OMALLA RISKILLÄ LUKEKAA
public class EnemyAI : MonoBehaviour
{
    private Transform player;   //Change to tag later
    //public/private enemymodit mods;
    private EnemyHp life;
    //add maybe time to reduce player destation calls. stoping distance shjould be one
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

    public Transform hand;   //Name child hand to find it
    public int zombieDamage = 2;

    //These are for guns
    private float stunTime;
    private bool isStunned = false;

    [Header("Roaming")]
    public static bool combatMode = false;
    public float roamRange = 3;
    public float waitTime = 10f;
    public float walkingSpeed = 4.5f;
    public float runSpeed = 7.5f;   //isnt used. But might in end point this will get buffs inside
    private bool niceSpot = false;
    private float canMoveTime;

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
        canMoveTime = waitTime + Time.time;
    }

    //Need to check if alive
    void Update()
    {   
        if(life.living)
        {
            if(!isStunned)
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
    }
    //We seek random place from our afk position.
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
        if(agent.remainingDistance < agent.stoppingDistance || Vector3.Distance(player.position, transform.position) < 1.5f)
        {
            if(Vector3.Distance(transform.position, player.position) < 2f)
            {
                agent.enabled = false;
                rotateToAttakPlayer();
                attackPlayer();
            }
            agent.enabled = true;
            agent.SetDestination(player.position);
        }
        else
        {
            anime.SetBool(runHash, true);
            agent.SetDestination(player.position);
        }
    }
    //Create attack void for melee that stop ai like train with transform.position.
    private void attackPlayer()
    {
        anime.SetBool(runHash, false);
        agent.enabled = false;
        transform.position = transform.position;    //add maybe tranform.forward to little bit forward with attack
        //enable Hit arm and diable after done
        anime.SetTrigger(attackHash);//wait here?
        agent.enabled = true;
    }

    //It would be nice if there was easy math for animation time / task distance to play animation right time and speed
    public void dropJump()
    {
        if(!doingJ)
        {   
            //Need to shoot ray from head to see where is wall and rotate towards it. Range 3f
            doingJ = true;
            oldDestination = agent.destination;
            endPoint = agent.currentOffMeshLinkData.endPos;
            agent.enabled = false;

            if(endPoint.y > transform.position.y + 1)
            {
                wasClimb = true;
                rotateTowardsWall();
                moveTorwardWall();
                anime.SetTrigger(climbHash);    //tarkista että alkaa välittömästi
                var timeForAnimation = anime.GetCurrentAnimatorClipInfo(0); //Ei saatu oikeaa pituutta, hard coded pituus
                transferSpeed = timeForAnimation.Length;             //Tarvitaan takistus animaatio nopeus
                // animationTime = Time.time + transferSpeed;
                animationTime = Time.time + 3.8f;

                float fDistance = Vector3.Distance(transform.position, endPoint);
                // distanceDivTime = fDistance/transferSpeed;
                distanceDivTime = fDistance/4f;
            }
            else
            {
                anime.SetTrigger("Jump");   //tarkista että alkaa välittömästi
                animationTime = Time.time + transferSpeed;
                jumpDistance = Vector3.Distance(transform.position, endPoint);
                distanceDivTime = jumpDistance/transferSpeed;
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
            if(wasClimb && transform.position.y < endPoint.y)
            {
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
    //REFACTOR, maybe try to find if u can see rotation of the navmeshlink. If we can see rotate how it is. If not plis add mask
    private void rotateTowardsWall()
    {
        RaycastHit wall;
        Ray ray = new Ray(headPosition.position, headPosition.forward);
        LayerMask mas = 1 << 8;
        mas |= 1 << 0;
        if(Physics.Raycast(ray, out wall, 8f, mas))
        {
            transform.rotation = Quaternion.LookRotation(-wall.normal);
        }
    }
    //move closer towards wall, this is fucked
    private void moveTorwardWall()
    {

        transform.position = wallpos.position;
    }
    //Rotate towards player
    private void rotateToAttakPlayer()
    {
        Vector3 targetD = player.position - transform.position;
        Quaternion x = Quaternion.LookRotation(targetD);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, x, 180*Time.deltaTime);
    }
    
    //Add damage that value here. We will get it from parent
    public void zombieMeleeCheck()
    {
        LayerMask mas = 1 << 12;

        Collider[] hitCollided = Physics.OverlapSphere(hand.position, 1.75f, mas);
        foreach(var hitted in hitCollided)
        {
            PlayerHp hp = GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<PlayerHp>();
            hp.decreeseHealth(zombieDamage);
        }
    }
    //Move more smoother
    private void betterMovement()
     {

     }
    //Method for guns and abilities to stun them
    public void stunEnemy(float time)
    {
        isStunned = true;
        stunTime = Time.time + time;
    }

    //For EnemyHp class to kill disable ai.
    public void killAgent()
    {
        agent.enabled = false;
    }
}
