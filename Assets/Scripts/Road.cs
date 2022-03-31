using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{

    public Mesh currentMesh, straightMesh, TJunctionMesh, cornerMesh, crossMesh;
    public Material currentMaterial, straightMaterial, TJunctionMaterial, cornerMaterial, crossMaterial;
    public Grid grid;

    private Node actualTile;
    public bool left, right, up, down;
    private int roads;
    private Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        // Get the actual tile where is the road
        actualTile = grid.getTile(this.gameObject.transform.position).GetComponent<Node>();
        left = false; right = false; up = false; down = false;
        roads = 0;
        currentRotation = this.gameObject.transform.rotation;
        currentMesh = this.gameObject.GetComponent<MeshFilter>().mesh;
        currentMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        updateJunction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateJunction()
    {
        roads = 0;
        this.gameObject.transform.rotation = currentRotation;
        left = false; right = false; up = false; down = false;

        // Check left node
        if (grid.getTile(actualTile.getPosX() - 1, actualTile.getPosY()).GetComponent<Node>().isRoad())
        {
            left = true;
            roads++;
        }

        // Check right node
        if (grid.getTile(actualTile.getPosX() + 1, actualTile.getPosY()).GetComponent<Node>().isRoad())
        {
            right = true;
            roads++;
        }

        // Check up node
        if (grid.getTile(actualTile.getPosX(), actualTile.getPosY() + 1).GetComponent<Node>().isRoad())
        {
            up = true;
            roads++;
        }

        // Check down node
        if (grid.getTile(actualTile.getPosX(), actualTile.getPosY() - 1).GetComponent<Node>().isRoad())
        {
            down = true;
            roads++;
        }


        // Check which road we need, begin with the most complex junction
        if (roads == 4)  // Cross
        {
            this.gameObject.transform.rotation = Quaternion.identity;
            if (this.gameObject.GetComponent<MeshFilter>().mesh != crossMesh)
            {
                this.gameObject.GetComponent<MeshFilter>().mesh = crossMesh;
                this.gameObject.GetComponent<MeshRenderer>().material = crossMaterial;
            }
        }
        else if (roads == 3)    // T junction
        {
            this.gameObject.transform.rotation = Quaternion.identity;
            if (this.gameObject.GetComponent<MeshFilter>().mesh != TJunctionMesh)
            {
                this.gameObject.GetComponent<MeshFilter>().mesh = TJunctionMesh;
                this.gameObject.GetComponent<MeshRenderer>().material = TJunctionMaterial;
            }

            // Rotate if it is necessary
            if (down && left && up)
                this.gameObject.transform.Rotate(0, 90, 0);
            else if (left && up && right)
                this.gameObject.transform.Rotate(0, 180, 0);
            else if (up && right && down)
                this.gameObject.transform.Rotate(0, 270, 0);
        }
        else if(roads == 2)
        {
            if(!((right && left) || (up && down)))   // If it is not a straight line
            {
                this.gameObject.transform.rotation = Quaternion.identity;
                if (this.gameObject.GetComponent<MeshFilter>().mesh != cornerMesh)
                {
                    this.gameObject.GetComponent<MeshFilter>().mesh = cornerMesh;
                    this.gameObject.GetComponent<MeshRenderer>().material = cornerMaterial;
                }

                // Rotate if it is necessary
                if (left && up)
                    this.gameObject.transform.Rotate(0, 90, 0);
                else if (up && right)
                    this.gameObject.transform.Rotate(0, 180, 0);
                else if (right && down)
                    this.gameObject.transform.Rotate(0, 270, 0);
            }
            else
            {
                this.gameObject.transform.rotation = currentRotation;
                this.gameObject.GetComponent<MeshFilter>().mesh = currentMesh;
                this.gameObject.GetComponent<MeshRenderer>().material = currentMaterial;
            }
        }
        else if(roads == 1)
        {
            /*this.gameObject.transform.rotation = currentRotation;
            if (this.gameObject.GetComponent<MeshFilter>().mesh != straightMesh)
            {
                this.gameObject.GetComponent<MeshFilter>().mesh = straightMesh;
                this.gameObject.GetComponent<MeshRenderer>().material = straightMaterial;
            }

            if(up || down)
            {
                this.gameObject.transform.Rotate(0, 90, 0);
            }*/
        }
        else if(roads == 0)
        {
            this.gameObject.transform.rotation = currentRotation;
            if (this.gameObject.GetComponent<MeshFilter>().mesh != straightMesh)
            {
                this.gameObject.GetComponent<MeshFilter>().mesh = straightMesh;
                this.gameObject.GetComponent<MeshRenderer>().material = straightMaterial;
            }
        }
    }
}
