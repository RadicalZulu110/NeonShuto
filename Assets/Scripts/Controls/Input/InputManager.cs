using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    public delegate void MoveInputHandler(Vector3 moveVector);
    public delegate void RotateInputHandler(float rotateAmountX, float rotateAmountY);
    public delegate void ZoomInputHandler(float zoomAmount);
}
