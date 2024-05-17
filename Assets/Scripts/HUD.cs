using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public Level level;
    public GameOver gameOver;

    public TextMeshProUGUI ScoreText;
    public UnityEngine.UI.Image[] Stars;

    private int starId = 0;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< Stars.Length; i++)
        {
            if(i == starId)
            {
                Stars[i].enabled = true;
            }
            else
            {
                Stars[i].enabled = false;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setScore(int score)
    {
        ScoreText.text = "Score: " + score.ToString();

        int visibleStar = 0;

        if(score >= level.scoreStar && score < level.scoreSecondStar)
        {
            visibleStar = 1;
        } else if(score >= level.scoreSecondStar && score < level.scoreThirdStar)
        {
            visibleStar = 2;
        }else if(score >= level.scoreThirdStar)
        {
            visibleStar = 3;
        }

        for( int i = 0; i < Stars.Length;i++)
        {
            if(i == visibleStar)
            {
                Stars[i].enabled = true;
            }
            else
            {
                Stars[i].enabled = false;
            }
        }

        starId = visibleStar;
    }

    public void OnGameWin()
    {
        gameOver.ShowWin(level.currentScore, starId);
        if (starId > PlayerPrefs.GetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0)) {
            PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, starId);
        }
    }

}
