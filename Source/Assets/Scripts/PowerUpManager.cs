using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    public float mult = 1.4f;
    public float duration = 5f;
    //public float spawnIn = 5f;

    public float powerSpawner = 10f;

    // Timer, which repeaats for powerups
    public float repeater = 5f;
    // Timer, that + repeater 
    public float countDowner = 5f;

    // For power spawn positions 
    private int i;

    // Spawn position can added in Unity
    public GameObject[] spawnPoints;
    //public GameObject effect;
    // Powerup Prefab
    public GameObject powerupPrefab;

    public static PowerUpManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //SpawnPowerUps();
        StartCoroutine(Wait());
    }

    IEnumerator Wait() 
    {
        yield return new WaitForSeconds(powerSpawner);

        Debug.Log(spawnPoints.Length);
        // Powerups will randomly spawned in different positions
        for (int i = 0; i < spawnPoints.Length; i++)
        {

            Instantiate(powerupPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);

            // Hier is the effect added
            //Instantiate(effect, transform.position, transform.rotation);

            //Debug.Log(spawnPoints[i].transform.position);


            
        }
    }

    private void Update()
    {
        // Repeater - Delta Time
        repeater -= Time.deltaTime;

        // If 0 it will spawn powerups
        if (repeater <= 0)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Instantiate(powerupPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            }

            // If 0, it repeats the process
            repeater += countDowner;
        }
    }

    //void SpawnPowerUps()
    //{
    //    Debug.Log(spawnPoints.Length);
    //    // Powerups will randomly spawned in different positions
    //    for (int i = 0; i < spawnPoints.Length; i++)
    //    {
    //
    //        Instantiate(powerupPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
    //
    //        // Hier is the effect added
    //        //Instantiate(effect, transform.position, transform.rotation);
    //
    //        //Debug.Log(spawnPoints[i].transform.position);
    //
    //    }
    //}
}
