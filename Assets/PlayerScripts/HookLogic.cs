using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLogic : MonoBehaviour
{
    public Transform shootingPoint;
    [SerializeField]
    private GameObject hookModel;
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private Transform cameraRay;
   private HitboxLocations hitHp;
    //COOLDOWN
    [SerializeField]
    private float hookRange = 20f;
    private float onCd;
    [SerializeField]
    private float cooldownTime = 5;
    private bool coolingDown = false;

    //Hook logic
    [SerializeField]
    private float pullSpeed = 10;
    [HideInInspector]
    public Vector3 hookRouteFound;
    [HideInInspector]
    public Vector3 targetLocation;
    [HideInInspector]
    public bool reachTarget = false;
    private bool targetIsEnemy = false;
    private bool canDealDmg = false;
    //private GameObject = instantiate -> gameobject.clean() when distance closed
    private TMPro.TextMeshProUGUI cooldownText;
    void Start()
    {
        player = GetComponent<CharacterController>();
        onCd = Time.time;
        cooldownText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("HookCDindicator").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        cooldownText.text = "0.0";
        //change reachTarget -> shootTarget and other make reachTarget true do something else.
    }

    void Update()
    {
        if (coolingDown)
        {
            CalculateTextCD();
        }
        //Add cd system.
        if(Input.GetKeyDown(KeyCode.F) && Time.time >= onCd)
        {
            onCd = Time.time + cooldownTime;
            coolingDown = true;
            hookRoute();
            reachTarget = false;
            Instantiate(hookModel, shootingPoint.position, Quaternion.LookRotation(hookRouteFound));
            //we could have Gameobject deleteWhenDone. And when distance small delete hook object.
        }
        // Bug that it move direction but not update one
        if(reachTarget && targetIsEnemy)
        {
            // Better would be add pullVelocity for playerMove. But this is fine for now 11.11.22
            Vector3 pullMoving = targetLocation - transform.position;
            player.Move(pullMoving * pullSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetLocation) < 2)
            {
                targetIsEnemy = false;
                if(canDealDmg)
                {
                    hitHp.hitLimb(50);
                    canDealDmg = false;
                }
            }
        }
    }

    private void hookRoute()
    {
        RaycastHit hit;
        LayerMask mask = 1 << 8;
        mask |= 1 << 0;
        mask |= 1 << 10;
        //Maskit tarvitaan ja myös siirtää pikkasen eteenpäin.
        Ray ray = new Ray(cameraRay.position, cameraRay.forward);
        bool result = Physics.Raycast(ray, out hit, hookRange, mask);

        if(result)
        {
            if(hit.transform.gameObject.layer == 10)
            {

                hitHp = hit.transform.GetComponent<HitboxLocations>();
                if(hitHp)
                {
                    hitHp.stunLimb(3);
                }
                canDealDmg = true;
            }
            targetLocation = hit.point;
            hookRouteFound = targetLocation - shootingPoint.position;
            targetIsEnemy = true;
            //Could add we hit something and that would be used where targetisenemy is used.
            //TargetIsEnemy would be used then to check attack call for animation or something else than movement.
        }
        //If we dont hit anything
        else
        {
            targetLocation = ray.GetPoint(hookRange);
            hookRouteFound = targetLocation - shootingPoint.position;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 dir = transform.TransformDirection(Vector3.forward) * hookRange;
        Gizmos.DrawRay(transform.position, dir);
    }

    private void CalculateTextCD()
    {
        cooldownText.text = Mathf.Abs((Time.time - onCd)).ToString("0.0");
        if (Time.time - onCd >= 0.0)
        {
            coolingDown = false;
        }
    }
}
