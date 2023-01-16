using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//REFACTOR. make this so we can use this easily with other enemys or make thinks easier to adapt
//Add cache value that we can have from PlayerModifiers.cs. cahceDmg = damageModifier();. called aina kun lisätää tai poistetaan jotain.
public class EnemyHp : MonoBehaviour
{   
    private PointSystem pointSystem;
    private MoneySystem moneySystem;
    private AudioSource[] sounds;
    private EnemyAI ai;
    private FireEnemy fai;
    private RagodollControl rControl;
    [Header("Reward and hit point")]
    [SerializeField]
    private int maxHp = 110;
    public int cacheModHp = 0; //Get this from upgradeSystem gameObject or TAG upgrade. Might not need this. Just maxhp + method 
    private int cHp;    //current hit point amount
    [SerializeField]
    private int killReward = 4; //later add modifiers here or Pointsystem
    public bool living = true;
    //Pitää korjata refactorinä tää spaghetti
    void Start()
    {
        //Uncomment, change to tag system
        pointSystem = GameObject.FindWithTag("Gamelogic").GetComponent<PointSystem>();
        moneySystem = GameObject.FindWithTag("Gamelogic").GetComponent<MoneySystem>();
        rControl = GetComponent<RagodollControl>();
        sounds = GetComponents<AudioSource>();
        //if something something then Ai else fire
        ai = GetComponent<EnemyAI>();
        fai = GetComponent<FireEnemy>();

        var rigidboies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigid in rigidboies)
        {
            HitboxLocations hitbox = rigid.gameObject.AddComponent<HitboxLocations>();
            hitbox.hp = this;
            if(ai){hitbox.enemy = ai;}
            if(fai){hitbox.fenemy = fai;}
        }

        sounds[1].PlayDelayed(Random.Range(0f, 1f));
        cHp = maxHp + cacheModHp;   //cahceModHp => enemyBuffs("Health") or something there.

        RoundSystem.zombieSpawned();
    }
    void Update()
    {
        
    }

    //Might use this in all
    public void otherSourceDamage(int damage)
    {
        if(living)
        {
            cHp -= damage;
            if(cHp <= 0)
            {
                healtZero();
            }
        }
        EnemyAI.combatMode = true;
        FireEnemy.combatMode = true;
    }

    //add force to throw up little bit.
    private void healtZero()
    {
        RoundSystem.zombieKilled();
        //Mahdollista et pitää tehdä joku tarkastus näille jos väärin järjestys
        sounds[0].Play();
        sounds[1].Stop();
        if(ai != null)
        {
            ai.killAgent();
        }
        if(fai != null)
        {
            fai.killAgent();
        }
        pointSystem.addPoints(killReward);
        moneySystem.addMoney(killReward);
        living = false;
        rControl.activeRagdoll();
        Destroy(gameObject, 10f);
    }
    //later add maybe hook kill that throws enemy side
}
