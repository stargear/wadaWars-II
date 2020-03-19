using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructable : MonoBehaviour
{
    public GameObject fracturedVersion;
    public GameObject CountdownText;
    public float timeTillDestroy;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // Set timer
        timer = timeTillDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0.0f)
        {
            // Destroy/Shatter platform (swap model)
            Instantiate(fracturedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;

            // Show warning on last 5 seconds
            if (timer <= 5.5f)
            {
                // Activate Countdown timer
                CountdownText.SetActive(true);
                CountdownText.GetComponent<Text>().text = Mathf.Round(timer).ToString();

                // ToDo: Play Countdown sound
            }
        }
    }
}
