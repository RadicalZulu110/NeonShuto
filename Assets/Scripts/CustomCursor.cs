using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    private GameObject grid;

    private void Awake()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = grid.transform.position.y;
        mousePosition.z = mousePosition.z + 10;*/
        transform.position = cursorPointingGround();
    }

    // Return the point in the ground plane when the cursor is pointing
    private Vector3 cursorPointingGround()
    {
        Plane plane = new Plane(Vector3.up, 0f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;

        if (plane.Raycast(ray, out distanceToPlane))
        {
            return ray.GetPoint(distanceToPlane);
        }
        else
        {
            Debug.Log("NO");
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        }
    }
}
