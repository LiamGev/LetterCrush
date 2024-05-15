using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    private GamePiece piece;
    private IEnumerator moveCoroutine;
    

    private void Awake()
    {
        piece = GetComponent<GamePiece>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int newX, int newY, float time)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = MoveCoroutine(newX, newY, time);
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(int newX, int newY, float time)
    {
        piece.X = newX;
        piece.Y = newY;


        Vector3 startpos = transform.position;
        Vector3 endpos = piece.gridRef.GetWorldPosition(newX, newY);
       
        for(float t = 0; t <= 1 * time; t += Time.deltaTime)
        {
            piece.transform.position = Vector3.Lerp(startpos, endpos, t / time);
            yield return 0;
        }

        piece.transform.position = endpos;
    }
}
