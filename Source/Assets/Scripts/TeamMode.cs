using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TeamMode : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject teamButton;
    public bool areFourPlayers;

    void Start()
    {
        var connectedControllers = Gamepad.all.Count;

        if (connectedControllers == 4)
        {
            areFourPlayers = true;
        }

    }

    void Update()
    {
        teamButton.SetActive(areFourPlayers);
    }

}

