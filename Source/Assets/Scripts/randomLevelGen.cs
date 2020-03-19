using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomLevelGen : MonoBehaviour
{
    public int levelInt;
    public GameObject levelOne;
    public GameObject levelTwo;
    //add more levels!

    // Start is called before the first frame update
    void Start()
    {
        levelInt = Random.Range(0,3);


        if (levelInt <= 1)
        {
            levelOne.SetActive(true);
        }
        else
        {
            levelTwo.SetActive(true);
        }
    }

}
