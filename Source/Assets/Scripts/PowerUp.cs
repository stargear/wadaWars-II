using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerTypes
{
    Speed, // move Speed
    Jump, // Jump Force 
    Scale, // get Bigger
    Gun, // Gun Power
    Life // add life 
}


public class PowerUp : MonoBehaviour
{
    public PowerTypes powerTypes;


    //public int powerUpType = 0;
    public MeshRenderer rend;
    public Material mat1;
    public Material mat2;
    public Material mat3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    private void Start()
    {
        powerTypes = (PowerTypes)Random.Range(0, System.Enum.GetValues(typeof(PowerTypes)).Length);


        rend = GetComponent<MeshRenderer>();
        rend.material.color = Color.green;

        switch (powerTypes)
        {
            case PowerTypes.Speed:
                rend = GetComponent<MeshRenderer>();
                rend.material = mat1;
                break;

            case PowerTypes.Jump:
                rend = GetComponent<MeshRenderer>();
                rend.material = mat2;
                break;

            case PowerTypes.Scale:
                rend = GetComponent<MeshRenderer>();
                rend.material = mat3;
                break;

            case PowerTypes.Gun:
                rend = GetComponent<MeshRenderer>();
                rend.material = mat2;
                break;

            case PowerTypes.Life:
                rend = GetComponent<MeshRenderer>();
                rend.material = mat1;
                break;

            default:
                break;
        }
    }


    IEnumerator Pickup(Collider player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        //gameObject.SetActive(false); // Statt collider & meschrenderer


        switch (powerTypes)
        {
            case PowerTypes.Speed:
                controller.moveSpeed *= PowerUpManager.instance.mult;
                // Activated Time for Powerups
                yield return new WaitForSeconds(PowerUpManager.instance.duration);
                controller.moveSpeed /= PowerUpManager.instance.mult;

                break;
            case PowerTypes.Jump:
                controller.jumpSpeed *= PowerUpManager.instance.mult;
                // Activated Time for Powerups
                yield return new WaitForSeconds(PowerUpManager.instance.duration);
                controller.jumpSpeed /= PowerUpManager.instance.mult;

                break;
            case PowerTypes.Scale:
                player.transform.localScale *= PowerUpManager.instance.mult;
                // Activated Time for Powerups
                yield return new WaitForSeconds(PowerUpManager.instance.duration);
                player.transform.localScale /= PowerUpManager.instance.mult;


                break;
            case PowerTypes.Gun:
                controller.waterGun.GetComponent<item>().colliderForce = (int)(controller.waterGun.GetComponent<item>().colliderForce * PowerUpManager.instance.mult);
                controller.waterGun.GetComponent<item>().maxCharge /= PowerUpManager.instance.mult;
                // Activated Time for Powerups
                yield return new WaitForSeconds(PowerUpManager.instance.duration);
                controller.waterGun.GetComponent<item>().colliderForce = (int)(controller.waterGun.GetComponent<item>().colliderForce / PowerUpManager.instance.mult);
                controller.waterGun.GetComponent<item>().maxCharge *= PowerUpManager.instance.mult;

                break;
            case PowerTypes.Life:
                controller.life++;
                // Activated Time for Powerups (+1 Life has got no time!)

                break;
            default:
                break;
        }

        Destroy(gameObject);

    }
}
