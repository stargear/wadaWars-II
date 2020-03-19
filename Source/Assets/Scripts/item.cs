using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour {


    public int type = 0;    //0 is bazooka, 1 is waterpistol, 2 is aqua shotgun
    public string name;
    public GameObject projectile;
    public float projectileVelocity = 5f;
    public float rpm;       //this is the rate of fire of your weapon!
    public float rof;       //this is the rate of fire of your weapon!
    public bool charging = false;
    public GameObject muzzle;
    public Transform aimTarget;
    public float charge = 0;
    public GameObject ConeParent;
    public GameObject Cone;

    // Water steam with particle system
    public GameObject waterBeam;
    public int colliderForce = 100;
    public float maxCharge = 3.0f;
    private float timeoutTimer;
    private PlayerController playerController;


    public ParticleSystem waterBeamPS;
    public ParticleSystem waterBeamDoubleBarrelPS;


    // Cone increasing by charge
    public Vector3 coneStartScale = new Vector3(0.037f, 0.037f, 0.037f);

    private void Awake()
    {
        // Get particle system from waterBeam
        if (type !=null)
        waterBeamPS = waterBeam.GetComponentInChildren<ParticleSystem>();
        // Get player controller from parent
        playerController = GetComponentInParent<PlayerController>();
    }

    // Use this for initialization
    public void Start()
    {
        rpm = 1 / rof;


    }

    private void InvokeRepeating(string v1, double v2, double v3)
    {
        throw new NotImplementedException();
    }

    IEnumerator fire() 
        //counter for ROF
    {
        print("i shoot");
        GameObject GO = Instantiate(projectile, transform.position, transform.rotation);
        GO.transform.LookAt(aimTarget);
        bullet myBullet = GO.GetComponent<bullet>();
        myBullet.speed += charge / 2;
        charge = 0;
        yield return new WaitForSeconds(rpm);
    }

    IEnumerator fireWaterBeam()
    {
       

        // Set max charge
        if (charge > maxCharge)
            charge = maxCharge;

        // Calculate/Set particle system values
        var mainPS = waterBeamPS.main;
        mainPS.startLifetime = charge;
        mainPS.startSpeed = charge * 2;
        mainPS.startSize = charge / 2;
        var collisionPS = waterBeamPS.collision;
        collisionPS.colliderForce = colliderForce;
        float beamDuration = charge / 2;
        /*Debug.Log("startLifetime: " + charge);
        Debug.Log("startSpeed: " + charge * 2);
        Debug.Log("startSize: " + charge / 2);
        Debug.Log("colliderForce: " + colliderForce);
        Debug.Log("beamDuration: " + charge / 2);*/

        //if this is shotgun, shoot two times!
        if (type == 2)
        {
            mainPS = waterBeamPS.main;
            var secondPS = waterBeamDoubleBarrelPS.main;
            secondPS.startLifetime = charge;
            secondPS.startSpeed = charge * 3;
            secondPS.startSize = charge / 2;
            var collision2PS = waterBeamDoubleBarrelPS.collision;
            collision2PS.colliderForce = colliderForce;
            beamDuration = charge / 2;
            mainPS.startLifetime = charge;
            mainPS.startSpeed = charge * 3;
            mainPS.startSize = charge / 2;




        }


        // Reset charge & disable cone
        charging = false;
        Cone.SetActive(false);
            playerController.CanRotate = false;
            waterBeam.SetActive(true);

       
            // Activate water beam
            waterBeamPS.Play();

            // Duration of water beam
            yield return new WaitForSeconds(beamDuration);

            // Disable water beam
            waterBeamPS.Stop();
            waterBeam.SetActive(false);
            playerController.CanRotate = true;

        

      
        charge = 0;

        // Set timeout
        timeoutTimer = beamDuration * 4;

            // Player can move again
            playerController.IsCharging = false;
                        yield return new WaitForSeconds(beamDuration);

        

    }

    // Update is called once per frame
    void Update () 
    {
        projectileVelocity *= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ChargeShoot();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            FireShoot();
        }
        else
        {
            //Cone.SetActive(false);
        }

        if (charging)
        {

            charge += Time.deltaTime;
            // Max charge reached, shoot water beam
            if (charge >= maxCharge)
                FireShoot();

            // Scale cone by charge
            ConeParent.transform.localScale = coneStartScale * charge;
        }

        // Calculate timeout
        if (timeoutTimer >= 0.0f)
            timeoutTimer -= Time.deltaTime;

    }

    public void ChargeShoot()
    {
        if (timeoutTimer > 0.0f)
        {
            // Cannot shoot
            // Maybe play some sound
        }
        else
        {
            // Charge shoot
            playerController.IsCharging = true;
            charging = true;
            Cone.SetActive(true);
        }
    }

    public void FireShoot()
    {
        StartCoroutine(fireWaterBeam());
    }
}
