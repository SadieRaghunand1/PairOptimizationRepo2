using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private Image[] healthImgs;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI checkText;
    [SerializeField] private TextMeshProUGUI nextChkText;
    
    private void Start()
    {
        SetCheckPtText(0, 3);
        SetScoreText(0);
    }

    //Control the score text UI
    public void SetScoreText(int score)
    {
        scoreText.text = "Score = " + score.ToString();
    }

    public TextMeshProUGUI GetScoreText()
    {
        return scoreText;
    }

    //Remove and add health indicators in UI as player gains and looses health
    public void SetHealthImgs(bool lose)
    {
        //Lose health
        if(lose)
        {
            for (int i = healthImgs.Length - 1; i >= 0; i--) //out of bounds error
            {
                //Try if any images are available to set inactive, if not catch
                try
                {
                    if (healthImgs[i] != null)
                    {
                        if(healthImgs[i].enabled)
                        {
                            healthImgs[i].enabled = false;
                            return;
                        }
                        
                    }
                    else
                    {
                        Debug.Log("Failed to access, " + i);
                    }
                }
                catch (NullReferenceException ex)

                {
                    Debug.LogWarning("Failed to access, " + i + " " + ex.Message);
                }
                

            }
        }
        //Gain health
        else
        {
            for (int i = 0; i < healthImgs.Length; i++)
            {
                if (!healthImgs[i].enabled)
                {
                    //Re-enable images when health gained
                    healthImgs[i].enabled = true;
                    return;
                }

            }
        }
        
    }

    //Set echackpoint text
    public void SetCheckPtText(int check, int next)
    {
        checkText.text = "Checkpoints = " + check;
        nextChkText.text = "Next Checkpoint: " + next;
        StartCoroutine(SetChkPtColor());

    }

    IEnumerator SetChkPtColor()
    {
        checkText.color = Color.cyan;
        yield return new WaitForSeconds(3);
        checkText.color = Color.white;
    }


}
