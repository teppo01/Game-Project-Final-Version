using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Later make weapons and add them here and remove weapons fields and attribudes from here.
public class ShootingLogic : MonoBehaviour
{
    public Camera playercamaera;
    public DefaultGun theGUN;
    private PlayerModifiers modit;   //tag system where its upgrades
    private AudioSource gunAudio;
    public AudioClip shootingSound, reloadSound;
    private float nextTimeToFire = 0f;
    public ParticleSystem muzzleFlash;

    private float currAmmo;
    private bool reloading = false;
    private Animator gunAnimator;
    private TMPro.TextMeshProUGUI ammoText;

    void Start()
    {
        modit = GameObject.FindWithTag("Gamelogic").GetComponent<PlayerModifiers>();
        gunAnimator = GameObject.FindWithTag("Weapon").GetComponent<Animator>();
        ammoText = GameObject.FindWithTag("UICANVAS").transform.Find("AmmoCount").GetComponent<TMPro.TextMeshProUGUI>();

        //SETUP MODIS
        theGUN.damage = modit.playerBasicModi("DAMAGE", theGUN.damage);
        theGUN.rof = modit.playerBasicModi("ROF", theGUN.rof);
        theGUN.magSize = modit.playerBasicModi("MAG", theGUN.magSize);
        theGUN.reloadTime = modit.playerBasicModi("RELOAD", theGUN.reloadTime);


        gunAudio = GetComponent<AudioSource>();
        currAmmo = theGUN.magSize;
        UpdateAmmoText();
    }

    //Change maybe fixedupdate for physics maybe
    //Asioita voisi muokata niin että ylös tulisi aseClass ja käyttäisimme niiden statseja ja juttuja täälä
    void Update()
    {
        // return blocks shooting
        if (reloading) return;

        if (currAmmo <= 0)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.R) && !(currAmmo == theGUN.magSize) && !GamePause.paused)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !GamePause.paused){
            currAmmo--;
            UpdateAmmoText();

            nextTimeToFire = Time.time + 1f/theGUN.rof;
            muzzleFlash.Play();
            gunAudio.clip = shootingSound;
            gunAudio.Play();
            theGUN.shoot(playercamaera.transform);
        }   
    }

    IEnumerator Reload()
    {
        reloading = true;
        gunAudio.clip = reloadSound;
        gunAudio.Play();
        gunAnimator.SetBool("reloadAnimation", true);
        yield return new WaitForSeconds(theGUN.reloadTime);
        currAmmo = theGUN.magSize;
        gunAnimator.SetBool("reloadAnimation", false);
        UpdateAmmoText();
        reloading = false;
        gunAudio.Stop();
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currAmmo.ToString(); ;
    }
}
