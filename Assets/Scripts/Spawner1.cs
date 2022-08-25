using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{

    public int numberStone, numberCrystals;
    public GameObject stonePrefab, crystalPrefab;
    public GameObject gridGO;

    Grid grid;
    Vector3 prefabPos;

    // Start is called before the first frame update
    void Start()
    {
        grid = gridGO.GetComponent<Grid>();

        createStones(numberStone);
        createCrystals(numberCrystals);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn stone chunks the number given
    public void createStones(int numberStones)
    {
        for(int i = 0; i < numberStones; i++)
        {
            GameObject node;

            do      // Get the first free node 
            {
                int x = Random.Range(0, grid.getSizeX()-10);
                int z = Random.Range(0, grid.getSizeY()-10);
                Vector3 pos = new Vector3(x, 0, z);
                node = grid.getTile(x, z);
            } while (node.GetComponent<Node>().isOcupied());

            prefabPos = buildCentered(grid.getNodes(stonePrefab.GetComponent<BuildingCost>().getGridWidth(), stonePrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
            grid.setNodesOccupied(stonePrefab.GetComponent<BuildingCost>().getGridWidth(), stonePrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>());
            stonePrefab.GetComponent<BuildingCost>().setNodes(grid.getNodes(stonePrefab.GetComponent<BuildingCost>().getGridWidth(), stonePrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
            Instantiate(stonePrefab, new Vector3(prefabPos.x, 0f, prefabPos.z), Quaternion.identity);
            grid.setNodesStone(grid.getNodes(stonePrefab.GetComponent<BuildingCost>().getGridWidth(), stonePrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
        }
    }

    // Spawn crystal chunks the number given
    public void createCrystals(int numberCrystal)
    {
        for (int i = 0; i < numberCrystal; i++)
        {
            GameObject node;

            do      // Get the first free node 
            {
                int x = Random.Range(0, grid.getSizeX()-10);
                int z = Random.Range(0, grid.getSizeY()-10);
                Vector3 pos = new Vector3(x, 0, z);
                node = grid.getTile(x, z);
            } while (node.GetComponent<Node>().isOcupied());

            prefabPos = buildCentered(grid.getNodes(crystalPrefab.GetComponent<BuildingCost>().getGridWidth(), crystalPrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
            grid.setNodesOccupied(crystalPrefab.GetComponent<BuildingCost>().getGridWidth(), crystalPrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>());
            stonePrefab.GetComponent<BuildingCost>().setNodes(grid.getNodes(crystalPrefab.GetComponent<BuildingCost>().getGridWidth(), crystalPrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
            Instantiate(crystalPrefab, new Vector3(prefabPos.x, 0f, prefabPos.z), Quaternion.identity);
            grid.setNodesCrystal(grid.getNodes(crystalPrefab.GetComponent<BuildingCost>().getGridWidth(), crystalPrefab.GetComponent<BuildingCost>().getGridHeight(), node.GetComponent<Node>()));
        }
    }




    private Vector3 buildCentered(List<GameObject> nodes)
    {
        float x = 0, z = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            x += nodes[i].GetComponent<Node>().transform.position.x;
            z += nodes[i].GetComponent<Node>().transform.position.z;
        }

        x /= nodes.Count;
        z /= nodes.Count;

        Vector3 res = new Vector3(x, 0, z);
        return res;
    }
}
