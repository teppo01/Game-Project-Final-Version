using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject objective;    //MOdel for gameobject
    public Animator elevator;     //Animation for Right door
    private int objectiveAmount;     //How many is there
    public int objectivesDone;      //HOw many is done, should be private and with method ++
    private bool objectsD = false;
    // Might add controller for time settings.
    private GameObject ending;   //get ending collidet object and enable it.
    private BoxCollider startingArea;

    private GameObject objectiveDoneText;
    private TMPro.TextMeshProUGUI objectiveCountText;
    
    //Add start we crate all objectives to childs. This could be part of gamelogic or own thing. But be carful if gameobject has childs.
    void Start()
    {
        EnemyAI.combatMode = false;
        FireEnemy.combatMode = false;

        ending = GameObject.FindGameObjectWithTag("Finish");
        ending.SetActive(false);
        objectiveDoneText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("DoneText").gameObject;
        objectiveCountText = GameObject.FindGameObjectWithTag("UICANVAS").transform.Find("ObjectiveCount").GetComponent<TMPro.TextMeshProUGUI>();

        objectiveAmount = transform.childCount;
        objectiveCountText.text = objectiveAmount.ToString();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.layer = 6;
            GameObject lb = Instantiate(objective, child.position, Quaternion.LookRotation(child.gameObject.transform.forward));    //look if u could add this gameobject to child
            lb.transform.parent = child;

            CapsuleCollider sc = child.gameObject.AddComponent<CapsuleCollider>();
            sc.isTrigger = true;
            sc.center = new Vector3(0,0,2);
            sc.radius = 2.5f;   //2f-3f
            sc.height = 6;
            var x = child.gameObject.AddComponent<BoxFixing>();
            x.m = this;
            
            GameObject light = new GameObject("light");
            light.transform.parent = child;
            light.transform.localPosition = new Vector3(0f,1.6f,0.5f);

            Light lightSettings = light.AddComponent<Light>();
            lightSettings.color = Color.red;
            lightSettings.intensity = 2;
            lightSettings.range = 4;

        }
        openDoors();
    }

    void Update()
    {
        if(objectivesDone >= objectiveAmount && !objectsD)
        {
            openDoors();
            ending.SetActive(true);
            FindObjectOfType<AudioManager>().Play("ElevatorsOpen");
            objectsD = true;
            objectiveDoneText.SetActive(true);
        }
        //Here we could go ape shit       
    }

    public void closeDoors()
    {
        elevator.SetTrigger("close");
    }
    public void openDoors()
    {
        elevator.SetTrigger("open");
    }
    private void random(int amount)
    {

    }
    public void objectiveSuccess()
    {
        objectivesDone += 1;
        objectiveCountText.text = (objectiveAmount - objectivesDone).ToString();
    }

    // At the start player is inside this game object so after leaving or dealing damage to zombies the combat begins and surviving. This could be in own gameobject that will be disable after its done.
    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player" && EnemyAI.combatMode == false)
        {
            EnemyAI.combatMode = true;
            FireEnemy.combatMode = true;
            FindObjectOfType<AudioManager>().goingCombat();
            closeDoors();
        }
    }
}
