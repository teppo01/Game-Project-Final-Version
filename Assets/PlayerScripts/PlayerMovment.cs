using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Linux user check that you don't use modernet mono on omnisharp.
//CharacterController antaa meille hyvän toimivan scriptin alkuu.
public class PlayerMovment : MonoBehaviour
{
    public CharacterController playerController;
    private PlayerHp playerHp;

    public Transform groundChecker;
    private bool grounded;
    [SerializeField]
    private float groundDistance = 0.5f;
    public LayerMask groundMask;
    public float gravity = -17;
    
    private float movement_h;
    private float movement_v;
    
    private Vector3 velocity;
    private Vector3 movement;

    [SerializeField]
    private int speed;
    [Header("Dashing settings")]
    [SerializeField]
    private float dashSpeed = 20;
    [SerializeField]
    private float dashCD = 5;
    private float dashTimer;
    private float dashTime = 0.2f; //Time.time
    private float startTime;
    [SerializeField]
    private float jumpHeight = 2.5f;

    private TMPro.TextMeshProUGUI cooldownText;
    private bool coolingDown = false;
    
    //We could check from game logic that can we use what and enable those abilities.
    //private int jumpCount = 2; if ground jump count -= 1; and space bar check that we have jump left. Jump is hard to make feel good
    void Start()
    {
        // best practice with prefabs is to find gamelogic to prevent overrides (i think)
        playerHp = GameObject.FindGameObjectWithTag("Gamelogic").GetComponent<PlayerHp>();
        cooldownText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("dashCDindicator").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        cooldownText.text = "0.0";
    }

    void Update()
    {
        if (coolingDown)
        {
            CalculateTextCD();
        }
        //we might add that if we have mobility in use we use that rather
        grounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        movement_h = Input.GetAxis("Horizontal");
        movement_v = Input.GetAxis("Vertical");
        movement = new Vector3(movement_h, 0f, movement_v);
        //movement.Normalize(); bug, we move faster when going right and forward. But normalising make weird things

        Vector3 realMovement = movement.z * transform.forward + movement.x * transform.right;
        //lerp would be nice
        if(startTime > Time.time)
        {
            playerController.Move(realMovement* Time.deltaTime * dashSpeed);
        }
        else{
            playerController.Move(realMovement * speed * Time.deltaTime);
        }
        //cd needs to be added and add
        if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time > dashTimer)
        {
            coolingDown = true;
            dashTimer = Time.time + dashCD;
            startTime = Time.time + dashTime;
        }

        if(Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    //One object need Collider.OnTrigger and other needs Rigidbody/collider
    //Maski käyttöö et oma tavarat ei tee vahinkoa tai poistaa colliderit
    private void OnTriggerEnter(Collider gameobj)
    {
        //zombie layer
        // if(gameobj.gameObject.layer == 10)
        // {
        //     playerHp.decreeseHealth(1);
        //     //add force or velocity away from hit later move this to enemy hit
        // }
        //fireball layer
        if(gameobj.gameObject.layer == 11)
        {
            playerHp.decreeseHealth(1);
        }
    }
    private void CalculateTextCD()
    {
        cooldownText.text = Mathf.Abs((Time.time - dashTimer)).ToString("0.0");
        if (Time.time - dashTimer >= 0.0)
        {
            coolingDown = false;
        }
    }
}
