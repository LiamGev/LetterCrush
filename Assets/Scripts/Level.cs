using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    // To make more level types, add to this Enum and create new Scripts that reference this class and add into those classes as needed
    public enum LevelType
    {
        Normal,
    }

    public GridManager grid;
    public HUD hud;


    public int scoreStar;
    public int scoreSecondStar;
    public int scoreThirdStar;

    protected int currentScore;

    protected LevelType type;

    public LevelType Type
    {
        get { return type; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hud.setScore(currentScore);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentScore >= scoreThirdStar)
        {
            GameWin();
        }
        
    }

    public virtual void GameWin()
    {
        hud.OnGameWin();
        grid.GameOver();
    }

    public virtual void OnMove()
    {
        Debug.Log("you moved");
    }

    public virtual void OnPieceClear(GamePiece piece)
    {
        currentScore += piece.score;
        hud.setScore(currentScore);
    }
}
