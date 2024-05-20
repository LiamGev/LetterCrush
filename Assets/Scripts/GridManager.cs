using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using static LetterPiece;

public class GridManager : MonoBehaviour
{
    private bool isCheckingMatches = false;
    private float debounceDelay =0.1f;

    public enum PieceType
    {
        A, B, COUNT, EMPTY
    }

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    }

    [System.Serializable]
    public struct PiecePosition
    {
        public PieceType type;
        public int x;
        public int y;
    }

    public int startLetter;
    public int endLetter;

    public int xDim;
    public int yDim;
    public float fillTime;

    public Level level;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    public PiecePosition[] initialPieces;
    public GameObject endScreen;

    private Dictionary<PieceType, GameObject> prefabDict;
    private GamePiece[,] pieces;

    private GamePiece pressedPiece;
    private GamePiece enteredPiece;

    public HUD hud;

    private bool gameOver = false;


    // Start is called before the first frame update
    void Awake()
    {
        prefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!prefabDict.ContainsKey(piecePrefabs[i].type))
            {
                prefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        pieces = new GamePiece[xDim, yDim];

        for (int i = 0; i < initialPieces.Length; i++)
        {
            if (initialPieces[i].x >= 0 && initialPieces[i].x < xDim && initialPieces[i].y >= 0 && initialPieces[i].y < yDim)
            {
                SpawnNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y] == null)
                {
                    SpawnNewPiece(x, y, PieceType.EMPTY);
                }
            }
        }

        StartCoroutine(Fill());
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator DebouncedMatchCheck()
    {
        isCheckingMatches = true;
        yield return new WaitForSeconds(debounceDelay);

        // Now perform the match check
        bool matchesFound = CanMakeMatchingSwap();

        // Log the result or perform any other actions
        Debug.Log("Matches found: " + matchesFound);

        isCheckingMatches = false;

        if (!matchesFound)
        {
            endScreen.SetActive(true);
            GameOver();
        }
    }

    public IEnumerator Fill()
    {
        bool needsFill = true;

        while (needsFill)
        {
            yield return new WaitForSeconds(fillTime);

            while (FillStep())
            {
                yield return new WaitForSeconds(fillTime);
            }
            
            needsFill = ClearAllMatches();

            if (!isCheckingMatches && IsBoardFull())
            {
                StartCoroutine(DebouncedMatchCheck());
            }
        }
    }

    public bool FillStep()
    {
        bool movedPiece = false;

        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                GamePiece piece = pieces[x, y];

                if (piece.IsMoveAble())
                {
                    GamePiece pieceBelow = pieces[x, y + 1];

                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MoveablePiece.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = Instantiate(prefabDict[PieceType.A], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.A);
                pieces[x, 0].MoveablePiece.Move(x, 0, fillTime);
                pieces[x, 0].LetterComponent.SetLetter((LetterPiece.LetterType)Random.Range(startLetter, endLetter)); // Set random Range to Random.Range(0, pieces[x,y].LetterComponent.NumLetters)
                movedPiece = true;
            }
        }
        return movedPiece;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f - y);
    }    
    
    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(prefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }

    public bool IsAdjacent(GamePiece pieceFirst, GamePiece pieceSecond)
    {
        return (pieceFirst.X == pieceSecond.X && Mathf.Abs(pieceFirst.Y - pieceSecond.Y) == 1) ||
               (pieceFirst.Y == pieceSecond.Y && Mathf.Abs(pieceFirst.X - pieceSecond.X) == 1);
    }


    public void Swap(GamePiece pieceFirst, GamePiece pieceSecond)
    {
        if (gameOver)
        {
            return;
        }

        if (pieceFirst.IsMoveAble() && pieceSecond.IsMoveAble())
        {
            pieces[pieceFirst.X, pieceFirst.Y] = pieceSecond;
            pieces[pieceSecond.X, pieceSecond.Y] = pieceFirst;

            if (GetMatch(pieceFirst, pieceSecond.X, pieceSecond.Y) != null || GetMatch(pieceSecond, pieceFirst.X, pieceFirst.Y) != null)
            {
                int pieceFirstX = pieceFirst.X;
                int pieceFirstY = pieceFirst.Y;

                pieceFirst.MoveablePiece.Move(pieceSecond.X, pieceSecond.Y, fillTime);
                pieceSecond.MoveablePiece.Move(pieceFirstX, pieceFirstY, fillTime);

                ClearAllMatches();
                StartCoroutine(Fill());
                level.OnMove();
            }
            else
            {
                pieces[pieceFirst.X, pieceFirst.Y] = pieceFirst;
                pieces[pieceSecond.X, pieceSecond.Y] = pieceSecond;
            }


        }
    }

    public void Press(GamePiece piece)
    {
        pressedPiece = piece;
    }

    public void Enter(GamePiece piece)
    {
        enteredPiece = piece;
    }

    public void Release()
    {
        if(IsAdjacent(pressedPiece, enteredPiece))
        {
            Swap(pressedPiece, enteredPiece);
        }
    }

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
    {
        if (piece.IsLetter())
        {
            LetterType letter = piece.LetterComponent.Letter;
            List<GamePiece> horizontal = new List<GamePiece>();
            List<GamePiece> vertical = new List<GamePiece>();
            List<GamePiece> matching = new List<GamePiece>();

            // Horizontal
            horizontal.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int xOffset = 1; xOffset < xDim; xOffset++)
                {
                    int x = (dir == 0) ? newX - xOffset : newX + xOffset;

                    if (x < 0 || x >= xDim)
                    {
                        break;
                    }

                    if (pieces[x, newY].IsLetter() && pieces[x, newY].LetterComponent.Letter == letter)
                    {
                        horizontal.Add(pieces[x, newY]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (horizontal.Count >= 3)
            {
                matching.AddRange(horizontal);
            }

            if (horizontal.Count >= 3)
            {
                foreach (var horizontalPiece in horizontal)
                {
                    for (int dir = -1; dir <= 1; dir += 2)
                    {
                        List<GamePiece> tempVertical = new List<GamePiece>();

                        for (int yOffset = 1; yOffset < yDim; yOffset++)
                        {
                            int y = horizontalPiece.Y + dir * yOffset;

                            if (y < 0 || y >= yDim)
                            {
                                break;
                            }

                            if (pieces[horizontalPiece.X, y].IsLetter() && pieces[horizontalPiece.X, y].LetterComponent.Letter == letter)
                            {
                                tempVertical.Add(pieces[horizontalPiece.X, y]);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (tempVertical.Count >= 2)
                        {
                            vertical.Add(horizontalPiece); // Add the original horizontal piece if a vertical match is found
                            vertical.AddRange(tempVertical);

                            bool sameLetter = vertical.All(p => p.LetterComponent.Letter == letter);

                            if (sameLetter)
                            {
                                matching.AddRange(vertical);
                                break; // Break to avoid redundant checks
                            }
                            else
                            {
                                vertical.Clear(); // Clear if letters don't match
                            }
                        }
                    }
                }
            }

            // Vertical
            horizontal.Clear();
            vertical.Clear();
            vertical.Add(piece);

            for (int dir = 0; dir <= 1; dir++)
            {
                for (int yOffset = 1; yOffset < yDim; yOffset++)
                {
                    int y = (dir == 0) ? newY - yOffset : newY + yOffset;

                    if (y < 0 || y >= yDim)
                    {
                        break;
                    }

                    if (pieces[newX, y].IsLetter() && pieces[newX, y].LetterComponent.Letter == letter)
                    {
                        vertical.Add(pieces[newX, y]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (vertical.Count >= 3)
            {
                matching.AddRange(vertical);
            }

            if (vertical.Count >= 3)
            {
                foreach (var verticalPiece in vertical)
                {
                    for (int dir = -1; dir <= 1; dir += 2)
                    {
                        List<GamePiece> tempHorizontal = new List<GamePiece>();

                        for (int xOffset = 1; xOffset < xDim; xOffset++)
                        {
                            int x = verticalPiece.X + dir * xOffset;

                            if (x < 0 || x >= xDim)
                            {
                                break;
                            }

                            if (pieces[x, verticalPiece.Y].IsLetter() && pieces[x, verticalPiece.Y].LetterComponent.Letter == letter)
                            {
                                tempHorizontal.Add(pieces[x, verticalPiece.Y]);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (tempHorizontal.Count >= 2)
                        {
                            horizontal.Add(verticalPiece); // Add the original vertical piece if a horizontal match is found
                            horizontal.AddRange(tempHorizontal);

                            bool sameLetter = horizontal.All(p => p.LetterComponent.Letter == letter);

                            if (sameLetter)
                            {
                                matching.AddRange(horizontal);
                                break; // Break to avoid redundant checks
                            }
                            else
                            {
                                horizontal.Clear(); // Clear if letters don't match
                            }
                        }
                    }
                }
            }

            if (matching.Count >= 3)
            {
                return matching.Distinct().ToList();
            }
        }
        return null;
    }


    public bool ClearAllMatches()
    {
        bool needsFill = false;

        for (int y = 0; y < yDim; y++)
        {
            for (int x = 0; x < xDim; x++)
            {
                if (pieces[x, y].IsClearable())
                {
                    List<GamePiece> match = GetMatch(pieces[x, y], x, y);

                    if (match != null && match.Count >= 3)
                    {
                        foreach (GamePiece piece in match)
                        {
                            if (ClearPiece(piece.X, piece.Y))
                            {
                                needsFill = true;
                            }
                        }
                    }
                }
            }
        }
        return needsFill;
    }


    public bool ClearPiece(int x, int y)
    {
        if (pieces[x,y].IsClearable() && !pieces[x, y].ClearPiece.IsCleared)
        {
            pieces[x, y].ClearPiece.Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);

            return true;
        }
        return false;
    }

    public bool CheckForMatches(int x, int y)
    {
        if (pieces[x, y] == null)
        {
            return false;
        }

        GamePiece piece = pieces[x, y];
        if (!piece.IsLetter())
        {
            // No match possible if the piece is not a letter
            return false;
        }

        return CheckLineForMatches(x, y, 1, 0) || CheckLineForMatches(x, y, 0, 1);
    }

    public bool CheckLineForMatches(int startX, int startY, int dx, int dy)
    {
        LetterType letter = pieces[startX, startY].LetterComponent.Letter;
        int matchCount = 1;

        // Check in the positive direction
        matchCount += CountMatchesInDirection(startX, startY, dx, dy, letter);

        // Check in the negative direction
        matchCount += CountMatchesInDirection(startX, startY, -dx, -dy, letter);

        // Include the starting piece in the count
        return matchCount >= 3;
    }

    public int CountMatchesInDirection(int startX, int startY, int dx, int dy, LetterType letter)
    {
        int matchCount = 0;
        int currentX = startX + dx;
        int currentY = startY + dy;

        while (currentX >= 0 && currentX < xDim && currentY >= 0 && currentY < yDim)
        {
            GamePiece currentPiece = pieces[currentX, currentY];
            if (!currentPiece.IsLetter() || currentPiece.LetterComponent.Letter != letter)
            {
                break; // Stop counting if the piece is not a letter or doesn't match the given letter
            }
            matchCount++;
            currentX += dx;
            currentY += dy;
        }

        return matchCount;
    }


    public bool CanMakeMatchingSwap()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                // Check swapping with the right neighbor
                if (x < xDim - 1)
                {
                    SwapPieces(x, y, x + 1, y);

                    if (CheckForMatches(x, y) || CheckForMatches(x + 1, y))
                    {
                        UndoSwap(x, y, x + 1, y); // Undo the swap
                        Debug.Log($"Found matching swap at ({x}, {y}) and ({x + 1}, {y})");
                        return true;
                    }
                    UndoSwap(x, y, x + 1, y); // Undo the swap
                }

                // Check swapping with the bottom neighbor
                if (y < yDim - 1)
                {
                    SwapPieces(x, y, x, y + 1);

                    if (CheckForMatches(x, y) || CheckForMatches(x, y + 1))
                    {
                        UndoSwap(x, y, x, y + 1); // Undo the swap
                        Debug.Log($"Found matching swap at ({x}, {y}) and ({x}, {y + 1})");
                        return true;
                    }
                    UndoSwap(x, y, x, y + 1); // Undo the swap
                }
            }
        }

        // No matching swap found
        return false;
    }

    public void SwapPieces(int x1, int y1, int x2, int y2)
    {
        // Swap the pieces at positions (x1, y1) and (x2, y2)
        GamePiece temp = pieces[x1, y1];
        pieces[x1, y1] = pieces[x2, y2];
        pieces[x2, y2] = temp;
    }

    public void UndoSwap(int x1, int y1, int x2, int y2)
    {
        // Undo the swap by swapping the pieces back
        SwapPieces(x1, y1, x2, y2);
    }

    bool IsBoardFull()
    {
        // Check if every grid cell is occupied by a piece
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (pieces[x, y].Type == PieceType.EMPTY)
                {
                    // Found an empty cell, so the board is not full
                    return false;
                }
            }
        }
        // No empty cells found, so the board is full
        return true;
    }

    public void GameOver()
    {
        hud.OnGameWin();
    }

}
