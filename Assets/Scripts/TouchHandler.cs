using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private GamePiece gamePiece;
    private GridManager grid;
    private bool isTouching = false;

    void Start()
    {
        // Get the GamePiece component attached to the same GameObject
        gamePiece = GetComponent<GamePiece>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        OnTouchDown();
                    }
                    else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        OnTouchEnter();
                        isTouching = true;
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        OnTouchUp();
                        isTouching = false;
                    }
                }
            }
            else
            {
                if (isTouching && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    OnTouchUp();
                    isTouching = false;
                }
            }
        }
    }

    void OnTouchEnter()
    {
        if (gamePiece != null)
        {
            grid.Enter(gamePiece);
        }
    }

    void OnTouchDown()
    {
        if (gamePiece != null)
        {
            grid.Press(gamePiece);
        }
    }

    void OnTouchUp()
    {
        grid.Release();
    }
}
