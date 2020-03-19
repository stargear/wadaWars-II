using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    public List<Transform> spawnPoints = new List<Transform>();

    public bool isGameMenu = false;
    public bool toggleMenu = false;

    public GameObject menu_A;
    public GameObject menu_B;

    public GameObject PlayerPrefab;
    public InputActionAsset PlayerInputActionAsset;
    public GameObject[] hats;
    public GameObject[] guns;

    [Header("Players UI")]
    public List<GameObject> playerMenus = new List<GameObject>();
    // ToDo: Do it better?
    public List<GameObject> playerJoinTexts = new List<GameObject>();
    public List<GameObject> playerReadyTexts = new List<GameObject>();
    public List<GameObject> playerCharacters = new List<GameObject>();
    public List<GameObject> playerSelectionPanels = new List<GameObject>();
    public List<GameObject> playerEventSystems = new List<GameObject>();
    private List<bool> playersReady = new List<bool>();
    private List<int> playersWatergun = new List<int>();
    private List<int> playersHat = new List<int>();
    //private List<int> playersPenguin = new List<int>();
    public List<GameObject> players = new List<GameObject>();
    private CameraMultiTarget multiTargetCam;

    [Header("Game Settings")]
    public int pointsToWin = 3;
    public float waitTimeBetweenRounds = 5;
    // Standard Game speed


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Scene is loaded -> activate players
        if (SceneManager.GetActiveScene().name == "CharacterSelectionScene")
        {
            ActivatePlayers();
        }

        // Level is started, spawn players with settings
        if (SceneManager.GetActiveScene().name == "GameScene" || SceneManager.GetActiveScene().name == "TestMap")
        {
            // Get and set multi target camera
            multiTargetCam = GameObject.Find("Main Camera").GetComponent<CameraMultiTarget>();

            SpawnPlayers();
        }
        else
        {
            // ToDo: Find better place for this.
            // Reset playerPoints in PlayerPrefsX
            int[] playerPoints = new int[Gamepad.all.Count];
            PlayerPrefsX.SetIntArray("PlayerPoints", playerPoints);
        }
    }
    private void ActivatePlayers()
    {
        // Get amount of controller
        var connectedControllers = Gamepad.all.Count;

        for (int i = 0; i < connectedControllers; i++)
        {
            // Activate player in character selection
            playerJoinTexts[i].SetActive(false);
            playerCharacters[i].SetActive(true);
            playerSelectionPanels[i].SetActive(true);
            playerEventSystems[i].SetActive(true);

            // Initialize variables for character selection
            playersReady.Add(false);
            playersHat.Add(0);
            playersWatergun.Add(0);
            //playersPenguin.Add(0);
        }
    }

    public void OnPlayerReady(int playerIndex)
    {
        // Player with playerIndex is ready
        // Deactivate selection panel
        playerSelectionPanels[playerIndex].SetActive(false);
        playerReadyTexts[playerIndex].SetActive(true);

        // Set player ready
        playersReady[playerIndex] = true;

        // Check if all players are ready
        CheckPlayerReady();
    }

    private void CheckPlayerReady()
    {
        // Check if all players are ready
        var allPlayersReady = true;
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (!playersReady[i])
                allPlayersReady = false;
        }

        // Start game if all players are ready and at least 2 players are connected
        // DEBUG: Switch between this two options to test it with only one player
        //if (allPlayersReady) // Debug only
        if (allPlayersReady && Gamepad.all.Count > 1) // Needs at least 2 players
            {
            loadMultiPlayer();
        }

    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void toggleMenuOn()
    {
        toggleMenu = !toggleMenu;
    }

    public void ChangeHat(int playerIndex, int newValue)
    {
        // Set and switch hat
        playersHat[playerIndex] = newValue;
        switchHat(playerIndex);
    }
    public void ChangeWatergun(int playerIndex, int newValue)
    {
        // Set and switch water gun
        playersWatergun[playerIndex] = newValue;
        switchGun(playerIndex);
    }

    //*********************************************************************************************************************************************

    //switch out items on slider change

    public void switchHat(int playerIndex)
    {
        //not more hats than there are!
        if (playersHat[playerIndex] < 0)
        {
            playersHat[playerIndex] = hats.Length;
        }
        else if (playersHat[playerIndex] > hats.Length)
        {
            playersHat[playerIndex] = 0;
        }

        // Get player controller
        PlayerController player = playerCharacters[playerIndex].GetComponent<PlayerController>();
        // Set hat
        player.switchHat(playersHat[playerIndex]);

    }

    public void switchGun(int playerIndex)
    {
        // Get player controller
        PlayerController player = playerCharacters[playerIndex].GetComponent<PlayerController>();
        // Set hat
        player.switchGun(playersWatergun[playerIndex]);
    }

    //********************************************************************************************************************************************

    public void loadMultiPlayer()
    {

        // All levels will be the same level - we simply instantiate another
        // Level prefab before the start of the game - this will be done
        // with playerprefs get (ingame) and set (menu)

        // Set PlayerPrefs for all players
        PlayerPrefsX.SetIntArray("Hats", playersHat.ToArray());
        PlayerPrefsX.SetIntArray("Guns", playersWatergun.ToArray());
        //PlayerPrefsX.SetIntArray("Penguins", playersPenguin.ToArray());



        //when the player loads in the game scene, we load the prefabs by first getting
        //the playerpref ints and thus equipping the player with the items!



        LoadScene("GameScene");
        //LoadScene("TestMap");

    }

    public void loadTestLevel()
    {
        PlayerPrefsX.SetIntArray("Hats", playersHat.ToArray());
        PlayerPrefsX.SetIntArray("Guns", playersWatergun.ToArray());
        //PlayerPrefsX.SetIntArray("Penguins", playersPenguin.ToArray());

        LoadScene("GameScene");

    }

    // Game quit botton
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    private void SpawnPlayers()
    {
        // Get amount of player
        int playerAmount = Gamepad.all.Count;

        // Instantiate players
        for (int i = 0; i < playerAmount; i++)
        {
            // Instantiate prefab with PlayerInput class (so controller can be attached directly)
            GameObject newPlayer = PlayerInput.Instantiate(PlayerPrefab, i, null, -1, Gamepad.all[i]).gameObject;
            newPlayer.transform.position = spawnPoints[i].transform.position;

            // Add settings from playerPrefsX and lvl for respawn
            newPlayer.GetComponent<PlayerController>().SetPrefs(i);
            newPlayer.GetComponent<PlayerController>().lvl = this;

            // Add player to array
            players.Add(newPlayer);
        }

        // Add player to multiTargetCamera
        multiTargetCam.SetTargets(players.ToArray());

        // Create points array as playerPrefs
        if (PlayerPrefsX.GetIntArray("PlayerPoints").Length == 0)
        {
            int[] playerPoints = new int[playerAmount];
            PlayerPrefsX.SetIntArray("PlayerPoints", playerPoints);
        }
    }

    public void respawn(GameObject player)
    {
        // Reset velocity
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Respawn player on random spawn point
        int mySpawnPosition = Random.Range(0, 3);
        player.transform.rotation = Quaternion.identity;
        player.transform.position = spawnPoints[mySpawnPosition].position;
    }

    public void CheckRemainingPlayers(int playerIndex)
    {
        // Remove player from player list and update multiTargetCam
        players.Remove(players[playerIndex]);
        multiTargetCam.SetTargets(players.ToArray());

        // Get remainingPlayers
        int remainingPlayers = 0;
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().life > 0)
            {
                remainingPlayers++;
            }
        }

        // End round if only one player remaining
        if (remainingPlayers <= 1)
        {
            // Remaining player get a point
            players[0].GetComponent<PlayerController>().PlayerWinsRound();
        }
    }

    public IEnumerator EndRound()
    {
        // ToDo: Show overview canvas

        // Shows Overview
        Overview.instance.OverviewTable.SetActive(true);

        // Wait
        yield return new WaitForSeconds(waitTimeBetweenRounds);

        // Load next round
        LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {

        //is this the game or the menu?
        if (isGameMenu == true)
        {
            //if we press a button we show character creation
            //menu and the map selection (for some modi)
            if (toggleMenu == true)
            {
                menu_B.SetActive(true);
                menu_A.SetActive(false);



                //here in character selection we determine which hat you show on the player
                //by pressing a button.. So use the switch case to switch out hats, weapons etc..

            }
            else
            {
                menu_B.SetActive(false);
                menu_A.SetActive(true);
            }

        }
    }

    public void SpeedModeOn()
    {
        Time.timeScale = 2f;
    }
}