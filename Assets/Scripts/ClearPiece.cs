using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPiece : MonoBehaviour
{

    public AnimationClip clearAnimation;

    private bool isCleared = false;

    public bool IsCleared {  get { return isCleared; } }

    protected GamePiece piece;

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

    public void Clear()
    {
        piece.gridRef.level.OnPieceClear(piece);
        isCleared = true;
        StartCoroutine(ClearCoroutine());   
    }

    private IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play(clearAnimation.name);
            
            yield return new WaitForSeconds(clearAnimation.length);

            Destroy(gameObject);
        }
    }
}
