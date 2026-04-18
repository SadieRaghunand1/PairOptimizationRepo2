using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool original;
    public int currentScore;
    public int highestScore;
    private int checkpointsReached;

    //In game scene
    GameUIManager uIManager;

    //In main menu
    MainMenuManager mainMenuManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        CheckForExistingManager();
        //SceneManager.sceneLoaded += LoadInNewScene;
        StartCoroutine(DelayLoad());
    }

    //Load next
    void LoadInNewScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            mainMenuManager = FindAnyObjectByType<MainMenuManager>();
            mainMenuManager.SetHighScoreText(this);
            currentScore = 0;
        }   
        else if (scene.buildIndex == 1)
        {
            uIManager = FindAnyObjectByType<GameUIManager>();
        }
            
        
    }

    //Destroy this manager if loading back into the main menu and this is not the original game manager
    void CheckForExistingManager()
    {
        if (FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length > 1 && !original)
        {
            Destroy(this.gameObject);
        }
        else
        {
            original = true;
        }
    }

    //Reset high score if this round's is higher
    public void EndGame()
    {
        if(currentScore > highestScore)
        {
            highestScore = currentScore;
            checkpointsReached = 0;
        }
        //load main menu
        SceneManager.LoadScene(0);
    }

    //Increase score
    public void IncPts(int pts)
    {
        currentScore += pts;
        uIManager.SetScoreText(currentScore);
    }


    //Getters and setters
    public int GetHighScore()
    {
        return highestScore;
    }

    public void SetCheckPoint(int next)
    {
        checkpointsReached++;
        uIManager.SetCheckPtText(checkpointsReached, next);
    }

    //Delay loading in new scene
    IEnumerator DelayLoad()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.sceneLoaded += LoadInNewScene;
        //LoadInNewScene();
    }
}
