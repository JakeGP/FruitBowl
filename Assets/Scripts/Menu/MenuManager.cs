using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameManager gameManager;

    void Awake()
    {        
        // Store game manager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();;
        DontDestroyOnLoad(gameManager);
    }

    // Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Loads a scene
    public void LoadScene(string name)
    {        
        SceneManager.LoadScene(name);
    }

    // Load a new game
    public void LoadGame()
    {
        StartCoroutine(CoLoadGame());
    }

    IEnumerator CoLoadGame()
    {
        yield return null;

        // Begin async loading of game
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("QuickGame");

        // Wait for it to complete
        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {   
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
