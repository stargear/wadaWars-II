using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRandomScenes : MonoBehaviour
{
    // Creating a RandomMapGenerator
    public void LoadRandomScene()
    {
        // By using the Random.Range method to load Random Scenes
        int randomMapSelection = Random.Range(2, 3); // Scene randomly selected here between x & y // Scene has to be added manualy Build Settings!
        SceneManager.LoadScene(randomMapSelection);
        Debug.Log("Random Scene loaded " + randomMapSelection);
    }
}
