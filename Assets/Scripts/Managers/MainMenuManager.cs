using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    GameManager gm;

    private void Start()
    {
        gm = FindAnyObjectByType<GameManager>();
        
    }

    public void LoadGame()
    {

        Debug.Log("Game opened");
        SceneManager.LoadScene(1);
    }

    public void SetHighScoreText(GameManager gmCall)
    {
        Debug.Log(gmCall.GetHighScore());
        highScoreText.text = "High Score: " + gmCall.GetHighScore();
    }
}
