using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPiece : MonoBehaviour
{
    public enum LetterType
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
    }

    [System.Serializable]
    public struct LetterSprite
    {
        public LetterType letter;
        public Sprite sprite;
    };

    public LetterSprite[] letterSprites;

    private LetterType letter;

    public LetterType Letter
    {
        get { return letter; }
        set { SetLetter(value); }
    }

    public int NumLetters
    {
        get { return letterSprites.Length;}
    }

    private SpriteRenderer sprite;
    private Dictionary<LetterType, Sprite> letterSpriteDict;
    
    void Awake()
    {
        // Find the "Piece" object with a specific tag
        GameObject pieceObject = GameObject.FindWithTag("Piece");
        letterSpriteDict = new Dictionary<LetterType, Sprite>();

        // Check if the "Piece" object was found
        if (pieceObject != null)
        {
            // Attempt to get the SpriteRenderer component from the found "Piece" object
            SpriteRenderer pieceSpriteRenderer = pieceObject.GetComponent<SpriteRenderer>();

            // Check if the SpriteRenderer component was found
            if (pieceSpriteRenderer != null)
            {
                // Assign the SpriteRenderer component to the sprite variable
                sprite = pieceSpriteRenderer;
            }
            else
            {
                // Log an error message if the SpriteRenderer component was not found
                Debug.LogError("SpriteRenderer component not found on the object with tag 'Piece'.");
            }
        }
        else
        {
            // Log an error message if the object with tag "Piece" was not found
            Debug.LogError("Object with tag 'Piece' not found.");
        }

        for (int i = 0; i < letterSprites.Length; i++)
        {
            if (!letterSpriteDict.ContainsKey(letterSprites[i].letter))
            {
                letterSpriteDict.Add(letterSprites[i].letter, letterSprites[i].sprite);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLetter(LetterType newLetter)
    {
        letter = newLetter;

        // Get the SpriteRenderer component of this specific piece
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (letterSpriteDict.ContainsKey(newLetter))
        {
            spriteRenderer.sprite = letterSpriteDict[newLetter];
            Debug.Log("Sprite assigned for LetterType " + newLetter + ": " + spriteRenderer.sprite.name);
        }
        else
        {
            Debug.LogError("No sprite found for LetterType: " + newLetter);
        }
    }


}
