using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public bool hasGamepad = false;
    public Rigidbody rBody;
    public float moveSpeed = 2f;
    public float jumpSpeed = 22f;
    public float rotateSpeed = 40f;
    public LevelManager lvl;
    public bool CanMove; // For CountDownController
    public bool grounded = true;
    public Quaternion defaultRotation;
    public float damp = 0.2f;
    public GameObject waterGun;
    public GameObject Circle;
    public bool IsCharging = false;
    public bool CanRotate = true;
    private item waterGunItem;
    //public item waterGun;
    public Animator anim;
    // Gamepad controls
    //PlayerControls controls;
    Vector2 inputMovement;

    public GameObject[] guns;
    public GameObject[] hats;
    public int gunInt = 0;
    public int hatInt = 0;

    public int life = 3;
    //public int points = 0; -> use PlayerPrefsX.GetIntArray("PlayerPoints") instead.
    public int index = 0;
    public GameObject playerColor;
    public List<Material> playerColorMaterials = new List<Material>();
    public GameObject playerUI;
    private GameObject UI_name;
    private GameObject UI_life;
    private GameObject heartIcon;
    private GameObject backdrop;
    private GameObject chargeImage;


    private Text uiName;
    private Text uiLife;
    private Image uiHeart;
    private Image uiBD;
    private Image uiCharge;
    public  int powerUpIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        //  rBody = GetComponent<Rigidbody>();
        defaultRotation = transform.rotation;
        //lvl = GameObject.Find("Level").GetComponent<LevelManager>();

        CanMove = false;
    }

    public void SetPrefs(int playerIndex)
    {
        // Get items from playerPrefsX
        switchHat(PlayerPrefsX.GetIntArray("Hats")[playerIndex]);
        switchGun(PlayerPrefsX.GetIntArray("Guns")[playerIndex]);
        //switchPenguin(PlayerPrefsX.GetIntArray("Penguins")[playerIndex]);

        // Set index and activate UI
        index = playerIndex;
        ActivateUI();
    }

    public void switchGun(int newGunInt)
    {
        //not more hats than there are!
        gunInt = newGunInt;

        switch (gunInt)
        {
            case 0:
                for (int i = 0; i < guns.Length; i++)
                {
                    guns[i].SetActive(false);
                }
                guns[0].SetActive(true);
                waterGunItem = guns[0].GetComponent<item>();
                break;
            case 1:
                for (int i = 0; i < guns.Length; i++)
                {
                    guns[i].SetActive(false);
                }
                guns[1].SetActive(true);
                waterGunItem = guns[1].GetComponent<item>();
                break;
            case 2:
                for (int i = 0; i < guns.Length; i++)
                {
                    guns[i].SetActive(false);
                }
                guns[2].SetActive(true);
                waterGunItem = guns[2].GetComponent<item>();
                break;

            default:
                for (int i = 0; i < guns.Length; i++)
                {
                    guns[i].SetActive(false);
                }
                guns[1].SetActive(true);
                waterGunItem = guns[1].GetComponent<item>();


                break;
        }
    }



    public void switchHat(int newHatInt)
    {
        hatInt = newHatInt;

        //not more hats than there are!
        if (hatInt < 0) { hatInt = hats.Length; } else if (hatInt > hats.Length) { hatInt = 0; }


        switch (hatInt)
        {
            case 0:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[0].SetActive(true);
                break;
            case 1:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[1].SetActive(true);
                break;
            case 2:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[2].SetActive(true);
                break;
            case 3:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[3].SetActive(true);
                break;
            case 4:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[4].SetActive(true);
                break;
            case 5:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[5].SetActive(true);
                break;
            case 6:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[6].SetActive(true);
                break;
            case 7:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[7].SetActive(true);
                break;
            case 8:
                for (int i = 0; i < hats.Length; i++)
                {
                    hats[i].SetActive(false);
                }
                hats[8].SetActive(true);
                break;
            default:
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {

        //checked if player flipped over
        if (Vector3.Dot(transform.up, Vector3.down) > 0.5)
        {
            //Debug.Log("flipped over xD");
        }
        if (Vector3.Dot(transform.right, Vector3.right) > 0.7)
        {
            //Debug.Log("flipped over xD");
            //     rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            //    rBody.isKinematic = true;
            //     transform.rotation = defaultRotation;
            //   transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * damp);
            //  rBody.isKinematic = false;

        }

        //subtract life from the UI
        uiLife.text = "" + life.ToString();
        //display the charge from the item
        uiCharge.fillAmount =  waterGunItem.charge/waterGunItem.maxCharge;



        if (hasGamepad == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }

            // movement here
            if (Input.GetKey(KeyCode.A))
            {
                //move towards the left
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
                transform.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));

            }
            else if (Input.GetKey(KeyCode.D))
            {
                //move towards the right
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
                transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.W))
            {
                //How do I start from here??
                transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //walk backwards
                transform.Translate(Vector3.forward * -Time.deltaTime * Input.GetAxis("Vertical") * -1 * moveSpeed);

            }

        }
        else
        {
            // New Input System
            if (inputMovement != Vector2.zero && grounded && CanMove)
            {
                // Movement

                Vector3 movement = new Vector3(inputMovement.x, 0f, inputMovement.y) * Time.deltaTime * moveSpeed;

                if (IsCharging && CanRotate)
                {
                    // Rotation is limited while charging
                    transform.rotation = Quaternion.LookRotation(movement * Time.deltaTime);
                    anim.SetBool("walk", false);
                    anim.SetBool("jump", false);
                }
                else if (CanRotate)
                {
                    // Don't rotate while shooting

                    transform.rotation = Quaternion.LookRotation(movement * Time.deltaTime);


                }

                // Don't move while charging
                if (!IsCharging)
                {
                    anim.SetFloat("speed", 0.3f);
                    anim.SetBool("walk", true);
                    transform.Translate(movement, Space.World);
                }

                else
                {
                    anim.SetFloat("speed", 0.0f);
                    anim.SetBool("walk", false);
                }

            }
            else if (!grounded) // Player is in air
            {
                // ToDo: Discuss in group: Is it fun? Or should we remove it?
                // Only move forward while in air
                transform.Translate(transform.forward * Time.deltaTime * moveSpeed, Space.World);
                anim.SetBool("walk", false);

            }


        }

    }

    private IEnumerator spawnTimer(float waitTime)
    {
        PlayerInput input = gameObject.GetComponent<PlayerInput>();

        // Lose a life
        life--;




        // Check if enough life is available
        if (life <= 0)
        {
            // End for this player
            PlayerDies();
            yield return null;
        }

        if (index == 0) { PlayerPrefs.SetInt("player_1_life",life); }
        else if (index == 1) { PlayerPrefs.SetInt("player_2_life", life); }
        else if (index == 2) { PlayerPrefs.SetInt("player_3_life", life); }
        else if (index == 3) { PlayerPrefs.SetInt("player_4_life", life); }


        // Deactivate input
        /*input.DeactivateInput();
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;*/

        // ToDo: Show death animation or something like this

        yield return new WaitForSeconds(waitTime);

        // Acivate input
        /*input.ActivateInput();
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;*/

        // Respawn
        lvl.respawn(gameObject);

    }

    private void Jump()
    {
        //i jump up!
        if (grounded)
        {
            anim.SetBool("jump", true);
            rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            grounded = false;
        }
    }

    private void ActivateUI()
    {
        // Default player color
        //var playerIndexColor = new Color(25, 69, 243); // blue

        switch (index)
        {

            case 0:
                //this is player 0
                playerUI = GameObject.Find("playerUI_1");
                heartIcon = GameObject.Find("Image1");
                backdrop = GameObject.Find("BackDrop1");
                chargeImage = GameObject.Find("charge1");
                UI_name = GameObject.Find("Text1");
                UI_life = GameObject.Find("lifeText1");
                break;
            case 1:
                //this is player 1
                playerUI = GameObject.Find("playerUI_2");
                heartIcon = GameObject.Find("Image2");
                backdrop = GameObject.Find("BackDrop2");
                chargeImage = GameObject.Find("charge2");
                UI_name = GameObject.Find("Text2");
                UI_life = GameObject.Find("lifeText2");
                //playerIndexColor = new Color(255, 35, 38); // red
                break;
            case 2:
                //this is player 2
                playerUI = GameObject.Find("playerUI_3");
                heartIcon = GameObject.Find("Image3");
                backdrop = GameObject.Find("BackDrop3");
                chargeImage = GameObject.Find("charge3");
                UI_name = GameObject.Find("Text3");
                UI_life = GameObject.Find("lifeText3");
                //playerIndexColor = new Color(47, 188, 3); // green
                break;
            case 3:
                //this is player 3
                playerUI = GameObject.Find("playerUI_4");
                heartIcon = GameObject.Find("Image4");
                backdrop = GameObject.Find("BackDrop4");
                chargeImage = GameObject.Find("charge4");
                UI_name = GameObject.Find("Text4");
                UI_life = GameObject.Find("lifeText4");
                //playerIndexColor = new Color(216, 217, 48); // yellow
                break;
        }
        //we cannot GameObject.Find() inactive objects so we need to activate the ui elements    :/
        //now we need to assign the UI elements of these objects


        uiHeart = heartIcon.GetComponent<Image>();
        uiBD = backdrop.GetComponent<Image>();
        uiCharge = chargeImage.GetComponent<Image>();
        uiLife = UI_life.GetComponent<Text>();
        uiName = UI_name.GetComponent<Text>();

        uiHeart.enabled = true;
        uiBD.enabled = true;
        uiCharge.enabled = true;
        uiLife.enabled = true;
        uiName.enabled = true;

        // Set color for player
        //playerColor.GetComponent<Renderer>().material.color = playerIndexColor;

        // Set material to player
        playerColor.GetComponent<Renderer>().material = playerColorMaterials[index];
    }

    private void PlayerDies()
    {
        // Set UI text
        uiLife.text = "dead :(";

        // Check if only one player is remaining
        LevelManager.Instance.CheckRemainingPlayers(index);

        // Disable gameObject
        gameObject.SetActive(false);
    }

    public void PlayerWinsRound()
    {
        var playerPoints = PlayerPrefsX.GetIntArray("PlayerPoints");
        playerPoints[index]++;

        PlayerPrefsX.SetIntArray("PlayerPoints", playerPoints);


        if (index == 0) { PlayerPrefs.SetInt("player_1_points", playerPoints[index]); }
        else if (index == 1) { PlayerPrefs.SetInt("player_2_points", playerPoints[index]); }
        else if (index == 2) { PlayerPrefs.SetInt("player_3_points", playerPoints[index]); }
        else if (index == 3) { PlayerPrefs.SetInt("player_4_points", playerPoints[index]); }


        if (playerPoints[index] >= LevelManager.Instance.pointsToWin)
        {
            // Player wins whole game
            // Load TableOfPoints scene
            //LevelManager.Instance.LoadScene("TableOfPoints");
            LevelManager.Instance.LoadScene("TableOfPoints");
        } 
        else
        {
            // End round and start next
            StartCoroutine(LevelManager.Instance.EndRound());

        }
    }

    void OnTriggerEnter(Collider other)
    {
      //  powerUpIndex++;



        if (other.gameObject.layer == 4) //if you hit water
        {
            //display on canvas that you lost a life!

            //display a restart timer on canvas... 3,2,1, go!!


            StartCoroutine(spawnTimer(2f));

            //after time, the player is respawned somewhere..
        }


    
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Landed on ground or object
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9) //if you hit terrain or object
        {
            // this is grounded!
            grounded = true;
            anim.SetBool("jump", false);

        }
    }



    private void OnMove(InputValue input)
    {
        if (input.Get<Vector2>().sqrMagnitude >= 0.1)
        {
            inputMovement = input.Get<Vector2>();
            anim.SetFloat("speed", 0.3f);
            anim.SetBool("walk", true);
        }
        else
        {
            inputMovement = Vector2.zero;
            anim.SetFloat("speed", 0.0f);
            anim.SetBool("walk", false);
        }
    }

    private void OnJump()
    {
        Jump();
    }

    private void OnShootPressed()
    {
        waterGunItem.ChargeShoot();
    }

    private void OnShootReleased()
    {
        waterGunItem.FireShoot();
    }
}