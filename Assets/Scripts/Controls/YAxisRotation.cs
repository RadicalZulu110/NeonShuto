using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisRotation : MonoBehaviour
{
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;
    public Quaternion newRotation;
    public float movmentTime;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        //rotates the camera uisng middle mouse button 
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.right * (difference.y / 5f));

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movmentTime);
    }
}
