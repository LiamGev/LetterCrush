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
        // Check if there are any touches
        if (Input.touchCount > 0)
        {
            // Get the first touch
            Touch touch = Input.GetTouch(0);

            // Convert touch position to ray
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            // Check if the ray hits any collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is this object
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
        // Mimics OnMouseEnter
        if (gamePiece != null)
        {
            grid.Enter(gamePiece);
        }
    }

    void OnTouchDown()
    {
        // Mimics OnMouseDown
        if (gamePiece != null)
        {
            grid.Press(gamePiece);
        }
    }

    void OnTouchUp()
    {
        // Mimics OnMouseUp
        grid.Release();
    }
}
