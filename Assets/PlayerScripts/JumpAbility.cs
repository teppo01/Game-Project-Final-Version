using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Get player movement maybe later here so we have same settings with one settings
public class JumpAbility : MonoBehaviour
{
    [Header("Double jump settings")]
    private CharacterController player;
    public Transform grounChecker;
    public int jumpCount = 2;
    private bool isG;
    private Vector3 vel;
    //Ei voinut jostain sysytä käyttää intiä mut hyvä näin
    public LayerMask mask;
    [Header("Dash setting")]
    public float dashLenght = 20;
    public float cdDash;
    private float timeDash;
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {   
        //Refactor so we take values from player gameObject
        isG = Physics.CheckSphere(grounChecker.position, 0.5f, mask);

        if(isG && vel.y < 0)
        {
            jumpCount = 2;
            vel.y = -1;    
        }
        if(Input.GetKeyDown(KeyCode.Space) && canJump(jumpCount))
        {
            //first high we wish to jump * 2 * gravity
            vel.y += Mathf.Sqrt(3 * -2f * -17);
            jumpCount -= 1;
        }
        vel.y += -17 * Time.deltaTime;
        player.Move(vel * Time.deltaTime);


    }
    
    private bool canJump(int canJump)
    {
        if(canJump > 0){return true;}
        else{return false;}
    }
}
