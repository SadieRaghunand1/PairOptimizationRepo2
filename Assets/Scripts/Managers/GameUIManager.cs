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
    public void SetScoreText(int score)
    {
        scoreText.text = "Score = " + score.ToString();
    }

    public TextMeshProUGUI GetScoreText()
    {
        return scoreText;
    }

    public void SetHealthImgs(bool lose)
    {
        if(lose)
        {
            for (int i = healthImgs.Length - 1; i >= 0; i--) //out of bounds error
            {
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
        else
        {
            for (int i = 0; i < healthImgs.Length; i++)
            {
                if (!healthImgs[i].enabled)
                {
                    healthImgs[i].enabled = true;
                    return;
                }

            }
        }
        
    }

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
