using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountDownGameStart : MonoBehaviour
{
    public static CountDownGameStart instance;

    public int countdownTime;
    public Text countdownDisplay;

    public GameObject HUDContainer;
    public GameObject countDownController;

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

    // Countdown timer for the time that the game begins
    // Players got time for looking around and getting ready
    private void Start()
    {

        // ToDo: Freeze all players
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;

        }
        //player.CanMove = false;

        countdownDisplay.text = "GO!";

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
        HUDContainer.SetActive(false);

        // Unfreeze all players
        foreach (GameObject player in LevelManager.Instance.players)
        {
            player.GetComponent<PlayerController>().CanMove = true;
        }

    }
}
