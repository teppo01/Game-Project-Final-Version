using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookProjectile : MonoBehaviour
{   
    //This all is for animate hook flying and tell when HookLogic start to do it function.
    [SerializeField]
    private Transform shootingPoint;
    [SerializeField]
    private HookLogic hookLogic;
    [SerializeField]
    private float hookSpeedToTarget = 40;
    private int reachedDest = 0;
    private Vector3 myTarget;
    //Need to find at Awake/Start state to find all logic and add here. Maybe Tag is needed at these points when need to find.
    void Awake()
    {
        hookLogic = GameObject.FindWithTag("Player").GetComponent<HookLogic>(); 
        shootingPoint = GameObject.Find("ShootingPoint").transform;
        myTarget = hookLogic.targetLocation;
    }
    //huge if enemy is target logic and if missing is logic... Slerp would make nice animation
    //Now below would be missed shot. if(targetIsHit){move there. stop there. stop roation and wait clean call.}
    void Update()
    {
        var step = hookSpeedToTarget * Time.deltaTime;
        Vector3 relativePos = myTarget - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos);
        if(!hookLogic.reachTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, myTarget, step);
        }
        else
        {
            myTarget = shootingPoint.position;
            transform.position = Vector3.MoveTowards(transform.position, myTarget, step);
        }
        //Right now need to find way for logic that we get from HookLogic that do we come back or not
        //maybe freeze rotation and movement if logic is hit right think, when pulling towards enemy or object
        //Enemy and get clean command from hooklogic. Clean hookmodel when all things is done or we have been pulled.
        if(Vector3.Distance(transform.position, myTarget) < 0.5)
        {   
            //Might need to be something else. Because what if enemy dodge? Can enemy dodge or are we forgiving?
            //reachDest ++ only if it was not targetIsEnemy. Otherwise we would maybe stop hook and pull player and clean.
            hookLogic.reachTarget = true;
            reachedDest ++;
        }
        //Hope this wont bite back
        if(reachedDest == 2)
        {
            clean();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Could make it it stops early and wont move through. But need to be fast to not take attention.
        //We need to add enemy AI stop if here is set true like stun.
        if(collider.gameObject.layer == 10)
        {
            print("Got to my end point or hit enemies on my way");
            myTarget = collider.transform.position;
            hookLogic.targetLocation = collider.transform.position;
        }
    }
    //we could hide object maybe and re actived when needed.
    private void clean()
    {
        Destroy(gameObject);
    }
}
