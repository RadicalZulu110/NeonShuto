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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = grid.transform.position.y;
        mousePosition.z = mousePosition.z + 10;
        transform.position = mousePosition;
    }
}
