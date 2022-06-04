using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Positioning")]
    public Vector2 cameraOffset = new Vector2(10f, 14f);
    public float lookAtOffset = 2f;

    [Header("Move Controls")]
    public float inOutSpeed = 5f;
    public float lateralSpeed = 5f;
    public float rotateSpeed = 45f;

    [Header("Rotation X")]
    public float maxRotationX = 25f;
    public float minRotationX = 335f;

    [Header("Move Bounds")]
    public Vector2 minBounds, maxBounds, verticalBounds;

    [Header("Zoom controls")]
    public float zoomSpeed = 4f;
    public float nearZoomLimit = 2f;
    public float farZoomLimit = 16f;
    public float startingZoom = 5f;

    ZoomStrategy zoomStrategy;
    Vector3 frameMove;
    float frameRotateX, frameRotateY;
    float frameZoom;
    Camera cam;
    float rotationX, rotationY;
    

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
        //zoomStrategy = new OrtographZoomStrategy(cam, startingZoom);
        zoomStrategy = new PerspectiveZoomStrategy(cam, cameraOffset, startingZoom);
        cam.transform.LookAt(transform.position + Vector3.up * lookAtOffset);
        
    }

    private void OnEnable()
    {
        KeybordInputManager.OnMoveInput += UpdateFrameMove;
        //KeybordInputManager.OnRotateInput += UpdateFrameRotate;
        KeybordInputManager.OnZoomInput += UpdateFrameZoom;
        MouseInputManager.OnMoveInput += UpdateFrameMove;
        MouseInputManager.OnRotateInput += UpdateFrameRotate;
        MouseInputManager.OnZoomInput += UpdateFrameZoom;
    }

    private void OnDisable()
    {
        KeybordInputManager.OnMoveInput -= UpdateFrameMove;
        //KeybordInputManager.OnRotateInput -= UpdateFrameRotate;
        KeybordInputManager.OnZoomInput -= UpdateFrameZoom;
        MouseInputManager.OnMoveInput -= UpdateFrameMove;
        MouseInputManager.OnRotateInput -= UpdateFrameRotate;
        MouseInputManager.OnZoomInput -= UpdateFrameZoom;
    }

    private void UpdateFrameZoom(float zoomAmount)
    {
        frameZoom += zoomAmount;
    }

    private void UpdateFrameRotate(float rotateAmountX, float rotateAmountY)
    {
        frameRotateX += rotateAmountX;
        frameRotateY += rotateAmountY;
    }

    private void UpdateFrameMove(Vector3 moveVector)
    {
        frameMove += moveVector;
    }

    private void LateUpdate()
    {
        if (frameMove != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMove.x * lateralSpeed, frameMove.y, frameMove.z * inOutSpeed);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.unscaledDeltaTime;
            LockPositionInBounds();
            frameMove = Vector3.zero;
        }
        
        if(frameRotateX != 0f)
        {
            transform.Rotate(Vector3.up, frameRotateX * Time.unscaledDeltaTime * rotateSpeed);
            rotationY = transform.localEulerAngles.y;
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            frameRotateX = 0f;
        }
        else if(frameRotateY != 0f)
        {
            if(checkAngleX(transform.eulerAngles.x))
            {
                transform.Rotate(Vector3.right, frameRotateY * Time.unscaledDeltaTime * rotateSpeed);
                rotationX = transform.localEulerAngles.x;
                transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            }
            else
            {
                if(transform.localEulerAngles.x >= maxRotationX+10 )
                    transform.rotation = Quaternion.Euler(minRotationX, rotationY, 0);
                else if(transform.localEulerAngles.x-360 <= minRotationX)
                    transform.rotation = Quaternion.Euler(maxRotationX, rotationY, 0);
            }
            Debug.Log(transform.eulerAngles.x);
            
            frameRotateY = 0f;
        }
        else
        {
           
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        if (frameZoom < 0f)
        {
            zoomStrategy.ZoomIn(cam, Time.unscaledDeltaTime * Mathf.Abs(frameZoom) * zoomSpeed, nearZoomLimit);
            frameZoom = 0f;
        }else if(frameZoom > 0f)
        {
            zoomStrategy.ZoomOut(cam, Time.unscaledDeltaTime * frameZoom * zoomSpeed, farZoomLimit);
            frameZoom = 0f;
        }
    }

    private void LockPositionInBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
                                        Mathf.Clamp(transform.position.y, verticalBounds.x, verticalBounds.y),
                                        Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y));
    }

    private bool checkAngleX(float angle)
    {
        if ((angle >= 0 && angle <= maxRotationX) || (angle >= minRotationX && angle <= 360))
            return true;
        else
            return false;
    }

    
}
