using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{

    public int numberStone, numberCrystals;
    public GameObject stonePrefab, crystalPrefab;
    public GameObject gridGO;

    Grid grid;


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
    private void createStones(int numberStones)
    {
        for(int i = 0; i < numberStones; i++)
        {
            GameObject node;

            do      // Get the first free node 
            {
                int x = Random.Range(0, grid.getSizeX());
                int z = Random.Range(0, grid.getSizeY());
                Vector3 pos = new Vector3(x, 0, z);
                node = grid.getTile(x, z);
            } while (node.GetComponent<Node>().isOcupied());

            Instantiate(stonePrefab, new Vector3(node.transform.position.x, 0, node.transform.position.z), Quaternion.identity);
            node.GetComponent<Node>().setOcupied(true);
            node.GetComponent<Node>().setStone(true);
            grid.setAdyacentStone(node.GetComponent<Node>());
        }
    }

    // Spawn crystal chunks the number given
    private void createCrystals(int numberCrystal)
    {
        for (int i = 0; i < numberCrystal; i++)
        {
            GameObject node;

            do      // Get the first free node 
            {
                int x = Random.Range(0, grid.getSizeX());
                int z = Random.Range(0, grid.getSizeY());
                Vector3 pos = new Vector3(x, 0, z);
                node = grid.getTile(x, z);
            } while (node.GetComponent<Node>().isOcupied());

            Instantiate(crystalPrefab, new Vector3(node.transform.position.x, 0, node.transform.position.z), Quaternion.identity);
            node.GetComponent<Node>().setOcupied(true);
            node.GetComponent<Node>().setCrystal(true);
            grid.setAdyacentCrystal(node.GetComponent<Node>());
        }
    }
}
