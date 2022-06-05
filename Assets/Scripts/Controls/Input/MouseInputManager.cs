using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseInputManager : InputManager
{
    Vector2Int screen;
    Vector2 mousePositionOnRotateStart;

    // Events
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;
    public static event ZoomInputHandler OnZoomInput;
    private bool isOnPanel;

    private void Awake()
    {
        screen = new Vector2Int(Screen.width, Screen.height);
        //isOnPanel = false;
    }

    private void Update()
    {
        Vector3 mp = Input.mousePosition;
        bool mouseValid = (mp.y <= screen.y * 1.05f && mp.y >= screen.y * -0.05f &&
                            mp.x <= screen.x * 1.05f && mp.x >= screen.x * -0.05f); // The mouse must to be near to the window

        //if (!mouseValid) return;

        // Movement Near to the borders
        Debug.Log(isOnPanel);
        if (!isOnPanel)
        {
            if (mp.y > screen.y * 0.95)
            {
                OnMoveInput?.Invoke(Vector3.forward);
            }
            else if (mp.y < screen.y * 0.05)
            {
                OnMoveInput?.Invoke(-Vector3.forward);
            }

            if (mp.x > screen.x * 0.95)
            {
                OnMoveInput?.Invoke(Vector3.right);
            }
            else if (mp.x < screen.x * 0.05)
            {
                OnMoveInput?.Invoke(-Vector3.right);
            }
        }

        // Rotation
        if (Input.GetMouseButtonDown(1))
        {
            mousePositionOnRotateStart.x = mp.x;
            mousePositionOnRotateStart.y = mp.y;
        }else if (Input.GetMouseButton(1))
        {
            // Rotate to right and left
            if(mp.x < mousePositionOnRotateStart.x-100)
            {
                OnRotateInput?.Invoke(-1f, 0);
            }
            else if(mp.x > mousePositionOnRotateStart.x+100)
            {
                OnRotateInput?.Invoke(1f, 0);
            }
            else
            {
                // Rotate up and down
                if (mp.y < mousePositionOnRotateStart.y - 100)
                {
                    OnRotateInput?.Invoke(0, -1f);
                }
                else if (mp.y > mousePositionOnRotateStart.y + 100)
                {
                    OnRotateInput?.Invoke(0, 1f);
                }
            }

            
        }

        // Zoom
        if(Input.mouseScrollDelta.y > 0)
        {
            OnZoomInput?.Invoke(-3f);
        }else if(Input.mouseScrollDelta.y < 0)
        {
            OnZoomInput?.Invoke(3f);
        }
    }
    /*
    public void SetOnPanel(bool p)
    {
        isOnPanel = p;
    }
    
    private void OnMouseEnter(bool p)
    {
        isOnPanel = p;
    }
    */

    public void derp (bool p)
    {
        isOnPanel = p;
    }
}
