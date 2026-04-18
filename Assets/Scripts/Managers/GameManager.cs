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


    public void EndGame()
    {
        if(currentScore > highestScore)
        {
            highestScore = currentScore;
            checkpointsReached = 0;
        }
        SceneManager.LoadScene(0);
    }

    public void IncPts(int pts)
    {
        currentScore += pts;
        uIManager.SetScoreText(currentScore);
    }

    public int GetHighScore()
    {
        return highestScore;
    }

    public void SetCheckPoint(int next)
    {
        checkpointsReached++;
        uIManager.SetCheckPtText(checkpointsReached, next);
    }

    IEnumerator DelayLoad()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.sceneLoaded += LoadInNewScene;
        //LoadInNewScene();
    }
}
