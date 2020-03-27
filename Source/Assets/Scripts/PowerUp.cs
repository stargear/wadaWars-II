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
    public Material mat4;
    public Material mat5;

    private int powerInt;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collider myCollider = this.GetComponent<Collider>();
            PlayerController controller = other.GetComponent<PlayerController>();

            if (controller.currentPowerUp == 0)
            {

            AudioSource source = this.GetComponent<AudioSource>();
            source.Play();

            myCollider.enabled = false;
            controller.powerUpIndex++;

            if (controller.index == 0) { PlayerPrefs.SetInt("player_1_PowerUps", controller.powerUpIndex); }
            else if (controller.index == 1) { PlayerPrefs.SetInt("player_2_PowerUps", controller.powerUpIndex); }
            else if (controller.index == 2) { PlayerPrefs.SetInt("player_3_PowerUps", controller.powerUpIndex); }
            else if (controller.index == 3) { PlayerPrefs.SetInt("player_4_PowerUps", controller.powerUpIndex); }

            StartCoroutine(Pickup(other));
            }

            
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
                powerInt = 1;
                rend = GetComponent<MeshRenderer>();
                rend.material = mat1;

                break;

            case PowerTypes.Jump:
                powerInt = 2;
                rend = GetComponent<MeshRenderer>();
                rend.material = mat2;
                break;

            case PowerTypes.Scale:
                powerInt = 3;
                rend = GetComponent<MeshRenderer>();
                rend.material = mat3;
                break;

            case PowerTypes.Gun:
                powerInt = 4;
                rend = GetComponent<MeshRenderer>();
                rend.material = mat4;
                break;

            case PowerTypes.Life:
                powerInt = 5;
                rend = GetComponent<MeshRenderer>();
                rend.material = mat5;
                break;

            default:
                powerInt = 5;
                break;
        }


    }

  

    IEnumerator Pickup(Collider player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        //gameObject.SetActive(false); // Statt collider & meschrenderer

        controller.changeSprite(powerInt);
            switch (powerTypes)
            {
                case PowerTypes.Speed:
                    controller.currentPowerUp = 1;
                    controller.moveSpeed *= PowerUpManager.instance.mult;
                    // Activated Time for Powerups
                    yield return new WaitForSeconds(PowerUpManager.instance.duration);
                    controller.moveSpeed /= PowerUpManager.instance.mult;
                    controller.currentPowerUp = 0;
                    break;

                case PowerTypes.Jump:
                    controller.currentPowerUp = 2;
                    controller.jumpSpeed *= PowerUpManager.instance.mult;
                    // Activated Time for Powerups
                    yield return new WaitForSeconds(PowerUpManager.instance.duration);
                    controller.jumpSpeed /= PowerUpManager.instance.mult;
                    controller.currentPowerUp = 0;
                    break;

                case PowerTypes.Scale:
                    controller.currentPowerUp = 3;
                    player.transform.localScale *= PowerUpManager.instance.mult;
                    // Activated Time for Powerups
                    yield return new WaitForSeconds(PowerUpManager.instance.duration);
                    player.transform.localScale /= PowerUpManager.instance.mult;
                    controller.currentPowerUp = 0;
                    break;

                case PowerTypes.Gun:
                    controller.currentPowerUp = 4;
                    controller.waterGun.GetComponent<item>().colliderForce = (int)(controller.waterGun.GetComponent<item>().colliderForce * PowerUpManager.instance.mult);
                    controller.waterGun.GetComponent<item>().maxCharge /= PowerUpManager.instance.mult;
                    // Activated Time for Powerups
                    yield return new WaitForSeconds(PowerUpManager.instance.duration);
                    controller.waterGun.GetComponent<item>().colliderForce = (int)(controller.waterGun.GetComponent<item>().colliderForce / PowerUpManager.instance.mult);
                    controller.waterGun.GetComponent<item>().maxCharge *= PowerUpManager.instance.mult;
                    controller.currentPowerUp = 0;
                    break;


                case PowerTypes.Life:
                    controller.life++;
                    // Activated Time for Powerups (+1 Life has got no time!)
                    controller.currentPowerUp = 0;
                    break;
                default:
                    break;
            }


        controller.changeSprite(0);

        Destroy(gameObject);

    }
}
