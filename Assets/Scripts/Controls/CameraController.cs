using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float movmentSpeed;
    public float movmentTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovmentInput();
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        //using scroll wheel to zoom in and out 
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        //using right mouse click on world to pan
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
        //rotates the camera uisng middle mouse button 
        if(Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        
        if(Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            newRotation *= Quaternion.Euler(Vector3.right * (difference.y / 5f));
        }
        /*
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.right * (difference.y / 5f));
        }
        */
    }

    void HandleMovmentInput()
    {
        //pans Camera using ASDW or Arrow keys 
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movmentSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movmentSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movmentSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movmentSpeed);
        }
        //Rotates Camera using Q and E
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        //makes the movment smoother
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movmentTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movmentTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movmentTime);
    }

}
