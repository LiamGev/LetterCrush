using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public int score;

    private int x;
    private int y;
    
    public int X
    {
        get { return x; }
        set { if (IsMoveAble()) { x = value; } }
    }

    public int Y
    {
        get { return y; }
        set { if (IsMoveAble()){ y = value; } }
    }

    private GridManager.PieceType type;
    
    public GridManager.PieceType Type
    {
        get { return type; }
    }

    private GridManager grid;
    
    public GridManager gridRef
    {
        get { return grid; }
    }

    private PieceController moveablePiece;
    
    public PieceController MoveablePiece
    {
        get { return moveablePiece; }
    }

    private LetterPiece letterComponent;

    public LetterPiece LetterComponent
    {
        get { return letterComponent; }
    }

    private ClearPiece clearPiece;

    public ClearPiece ClearPiece
    {
        get { return clearPiece; }
    }

    private void Awake()
    {
        moveablePiece = GetComponent<PieceController>();
        letterComponent = GetComponent<LetterPiece>();
        clearPiece = GetComponent<ClearPiece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _x, int _y, GridManager _grid, GridManager.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    private void OnMouseEnter()
    {
        grid.Enter(this);
    }

    private void OnMouseDown()
    {
        grid.Press(this);
    }

    private void OnMouseUp()
    {
        grid.Release();
    }

    public bool IsMoveAble()
    {
        return moveablePiece != null;
    }

    public bool IsLetter()
    {
        return letterComponent != null;
    }

    public bool IsClearable()
    {
        return clearPiece != null;
    }
}
