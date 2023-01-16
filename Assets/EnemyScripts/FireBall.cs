using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //public/private EnemyMOdit ja start damage?
    private float lifeTime = 6;
    public float flyingSpeed = 10;
    public int fireballDamage = 2;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position = transform.position + transform.forward * flyingSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collid)
    {
        if(collid.gameObject.tag == "Player")
        {
            print("Fireball hit player");
            var play = collid.gameObject.GetComponent<PlayerHp>();
            if(play)
            {
                play.decreeseHealth(fireballDamage);
            }
            Destroy(this.gameObject);
        }
        //destory object if hitted groun objects and maybe later obsatcle layer too if needed
        if(collid.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
    }
}
