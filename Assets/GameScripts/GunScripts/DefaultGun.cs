using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "New Gun")]
public class DefaultGun : ScriptableObject
{
    [Header("Gun settings")]
    public new string name;
    public int damage;
    public int range;
    public int rof;
    public int magSize;
    public int reloadTime;  //animation time too.

    public int modDamage = 0;
    //public float spray;
    private LayerMask maskit;
    //Mahdollisesti luoja joku objekkti joka säästää kaikki tarvittat audiot ja effektit. Tai lyödä valmis ase kättee
    [Header("Effects and objects that is connected to this.")]
    // public audio clip name or what we should use;
    public float flyingPower;   //Power that we use for rigidbodys

    // jos mahdollisesti tulee enemmän effektejä eri aseille kuten haulikko mutta en usko
    public GameObject bulletImpact;
    public GameObject hitEffect;

    public void shoot(Transform shooterPoint)
    {
        maskit = 1 << 10;
        maskit |= 1 << 8;
        maskit |= 1 << 0;
        RaycastHit hit;
        Ray ray = new Ray(shooterPoint.position, shooterPoint.forward);
        bool result = Physics.Raycast(ray, out hit, range, maskit);

        if(result)
        {
            HitboxLocations target = hit.transform.GetComponent<HitboxLocations>();
            if (target) 
            {
                GameObject hitImpact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                target.hitLimb(damage + modDamage);
                Destroy(hitImpact, 1f);
            }
            else
            {
                GameObject impact = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }
        }
        if(hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * flyingPower);
        }
    }
}
