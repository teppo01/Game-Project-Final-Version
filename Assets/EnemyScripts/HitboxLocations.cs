using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxLocations : MonoBehaviour
{
    public EnemyHp hp;
    public EnemyAI enemy;
    public FireEnemy fenemy;

    public void hitLimb(int damage)
    {
        hp.otherSourceDamage(damage);
    }
    public void stunLimb(int stun)
    {
        enemy.stunEnemy(stun);
    }
}
