using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Buildings : MonoBehaviour
{
    private GameObject buildingToPlace, roadToPlace, initialToPlace, farmToPlace, batteryToPlace, crystalMineToPlace, stoneMineToPlace;
    public CustomCursor customCursor;
    public Grid grid;
    public GameObject[,] tiles;
    public Camera camera;
    public GameObject initialShadow, roadShadow, buildingShadow, farmShadow, batteryShadow, stoneMineShadow, crystalMineShadow;
    private Material roadShadowMaterial, buildingShadowMaterial, farmShadowMaterial, batteryShadowMaterial, stoneMineShadowMaterial, crystalMineShadowMaterial;
    public AudioSource buildingPlaceSound, buildingRotateSound, deleteBuildingSound;
    public ParticleSystem buildingPlaceParticles;

    GameObject nearNode, lastNearActiveNode, firstNodeRoad, lastNodeRoad;
    bool isDeleting;
    public GameManager gameManager;//need to change naming convention for this to be somthing else rather than gameManager
    public BuildingCost buildingCost;
    private GameObject selectedObjectToDelete;
    private Material[] originalMaterial;
    public Material[] deletingMaterial;
    private Vector3 buildPos;
    private BuildingCost buildingShadowScript, initialShadowScript;
    private bool firstRoadPlaced, initialPlaced;
    private GameObject[] roads;
    public float divisbleReturn;

    public NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        isDeleting = false;
        buildingShadowScript = buildingShadow.GetComponent<BuildingCost>();
        initialShadowScript = initialShadow.GetComponent<BuildingCost>();
        firstRoadPlaced = false;
        initialPlaced = false;
        getShadowMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        roads = GameObject.FindGameObjectsWithTag("Road");

        if(grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();

        if (tiles == null)
            tiles = grid.getGrid();

        // Hero shadow
        if (initialShadow.activeInHierarchy)
        {
            nearNode = getNearestActiveNode(customCursor.gameObject);
            lastNearActiveNode = nearNode;
            if (grid.areNodesFree(initialShadowScript.getGridWidth(), initialShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                buildPos = buildCentered(grid.getNodes(initialShadowScript.getGridWidth(), initialShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
                initialShadow.transform.position = new Vector3(buildPos.x, 0.3f, buildPos.z);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    rotateAroundY(initialShadow, 90);
                    buildingRotateSound.Play();
                    initialShadowScript.changeWH();
                }
            }
            
        }

        //Road Shadow
        if (roadShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy)
            {
                roadShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                roadShadow.GetComponentInChildren<Renderer>().material = roadShadowMaterial;
            }

            roadShadow.transform.position = new Vector3(nearNode.transform.position.x, 0.1f, nearNode.transform.position.z);
            if (Input.GetKeyDown(KeyCode.R) && !firstRoadPlaced)
            {
                rotateAroundY(roadShadow, 90);
                buildingRotateSound.Play();
            }
        }

        //Building Shadow
        if (buildingShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);
            
            if (!nearNode.activeInHierarchy || !grid.areNodesFree(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                buildingShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                buildingShadow.GetComponentInChildren<Renderer>().material = buildingShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            buildingShadow.transform.position = new Vector3(buildPos.x, 1.7f, buildPos.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(buildingShadow, 90);
                buildingRotateSound.Play();
            }
            
        }

        //farm shadow
        if (farmShadow.activeInHierarchy )
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                farmShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                farmShadow.GetComponentInChildren<Renderer>().material = farmShadowMaterial;
                lastNearActiveNode = nearNode;
            }

           
            buildPos = buildCentered(grid.getNodes(farmShadow.GetComponent<BuildingCost>().getGridWidth(), farmShadow.GetComponent<BuildingCost>().getGridHeight(), nearNode.GetComponent<Node>()));
            farmShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(farmShadow, 90);
                buildingRotateSound.Play();
            }
          
        }

        //battery shadow
        if (batteryShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                batteryShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                batteryShadow.GetComponentInChildren<Renderer>().material = batteryShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(batteryShadow.GetComponent<BuildingCost>().getGridWidth(), batteryShadow.GetComponent<BuildingCost>().getGridHeight(), nearNode.GetComponent<Node>()));
            batteryShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(batteryShadow, 90);
                buildingRotateSound.Play();
            }
            
        }

        //stonemine shadow
        if (stoneMineShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                stoneMineShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                stoneMineShadow.GetComponentInChildren<Renderer>().material = stoneMineShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(stoneMineShadow.GetComponent<BuildingCost>().getGridWidth(), stoneMineShadow.GetComponent<BuildingCost>().getGridHeight(), nearNode.GetComponent<Node>()));
            stoneMineShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(stoneMineShadow, 90);
                buildingRotateSound.Play();
            }
            
        }

        //crystalmine shadow
        if (crystalMineShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(buildingShadowScript.getGridWidth(), buildingShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                crystalMineShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                crystalMineShadow.GetComponentInChildren<Renderer>().material = crystalMineShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(crystalMineShadow.GetComponent<BuildingCost>().getGridWidth(), crystalMineShadow.GetComponent<BuildingCost>().getGridHeight(), nearNode.GetComponent<Node>()));
            crystalMineShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(crystalMineShadow, 90);
                buildingRotateSound.Play();
            }
            
        }

        // Cancel construction with escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            reSetValues();
        }

        // Create buildings
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            // Create initial building
            if (initialToPlace != null)
            {
                //nearNode = getNearestActiveNode(customCursor.gameObject);

                createBuilding(initialToPlace, initialShadow);
                List<GameObject> nodes = grid.getNodes(initialShadow.GetComponent<BuildingCost>().getGridWidth(), initialShadow.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>());
                for(int i=0; i<nodes.Count; i++)
                {
                    nodes[i].GetComponent<Node>().setInitial(true);
                }
                initialPlaced = true;
                initialToPlace = null;
                grid.checkTilesRoads();
            }

            // Create house
            if (buildingToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(buildingToPlace, buildingShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            //Create farm
            if (farmToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(farmToPlace, farmShadow);
                gameManager.SetNoFarms(gameManager.GetNoFarms() + 1);
            }

            //Create battery
            if (batteryToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(batteryToPlace, batteryShadow);
                gameManager.SetNoBatterys(gameManager.GetNoBatterys() + 1);
            }

            //Create StoneMine
            if (stoneMineToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(stoneMineToPlace, stoneMineShadow);
                gameManager.SetNoStoneMines(gameManager.GetNoStoneMines() + 1);
            }

            //Create CrystalMine
            if (crystalMineToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(crystalMineToPlace, crystalMineShadow);
                gameManager.SetNoCrystalMines(gameManager.GetNoCrystalMines() + 1);
            }
        }


        // Create road
        if (Input.GetKeyDown(KeyCode.Mouse0) && roadToPlace != null && !Input.GetKey(KeyCode.LeftShift)) // Create single road
        {
            nearNode = getNearestActiveNode(customCursor.gameObject);

            Instantiate(roadToPlace, new Vector3(nearNode.transform.position.x, 0, nearNode.transform.position.z), roadShadow.transform.rotation);
            buildingPlaceSound.Play();
            buildingPlaceParticles.transform.position = new Vector3(nearNode.transform.position.x, 0, nearNode.transform.position.z);
            buildingPlaceParticles.Play();
            nearNode.GetComponent<Node>().setOcupied(true);
            List<GameObject> nodes = grid.getNodes(roadToPlace.GetComponent<BuildingCost>().getGridWidth(), roadToPlace.GetComponent<BuildingCost>().getGridHeight(), nearNode.GetComponent<Node>());
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].GetComponent<Node>().setRoad(true);
            }
            gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            roadToPlace = null;
            customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            grid.setTilesActive(false);
            grid.checkTilesRoads();
            roadShadow.SetActive(false);
            updateRoadsJunction();
            surface.BuildNavMesh();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && roadToPlace != null && firstRoadPlaced && Input.GetKey(KeyCode.LeftShift)) // Last road of the line
        {
            nearNode = getNearestActiveNode(customCursor.gameObject);

            Instantiate(roadToPlace, new Vector3(nearNode.transform.position.x, 0, nearNode.transform.position.z), roadShadow.transform.rotation);
            lastNodeRoad = nearNode;
            buildLineRoads(firstNodeRoad.GetComponent<Node>(), lastNodeRoad.GetComponent<Node>());
            gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            buildingPlaceSound.Play();
            roadToPlace = null;
            customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            nearNode.GetComponent<Node>().setOcupied(true);
            nearNode.GetComponent<Node>().setRoad(true);
            grid.setTilesActive(false);
            grid.checkTilesRoads();
            roadShadow.SetActive(false);
            firstRoadPlaced = false;
            grid.setTilesActive(false);
            updateRoadsJunction();
            surface.BuildNavMesh();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && roadToPlace != null && Input.GetKey(KeyCode.LeftShift)) // First road of the line
        {
            nearNode = getNearestActiveNode(customCursor.gameObject);

            Instantiate(roadToPlace, new Vector3(nearNode.transform.position.x, 0, nearNode.transform.position.z), roadShadow.transform.rotation);
            buildingPlaceSound.Play();
            buildingPlaceParticles.transform.position = new Vector3(nearNode.transform.position.x, 0, nearNode.transform.position.z);
            buildingPlaceParticles.Play();
            nearNode.GetComponent<Node>().setOcupied(true);
            nearNode.GetComponent<Node>().setRoad(true);
            gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            //roadToPlace = null;
            //customCursorRoad.gameObject.SetActive(false);
            //Cursor.visible = true;
            //grid.setTilesActive(false);
            //grid.checkTilesRoads();
            //roadShadow.SetActive(false);
            firstRoadPlaced = true;
            firstNodeRoad = nearNode;
            grid.setTilesLineRoadVisible(firstNodeRoad.GetComponent<Node>());
            updateRoadsJunction();
        }

        

        

        // Delete building or road
        if(selectedObjectToDelete != null)
        {
            selectedObjectToDelete.GetComponentInChildren<Renderer>().materials = originalMaterial;
            selectedObjectToDelete = null;
        }

        if (isDeleting) // If Im deleting
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);     // Throw a ray in the mouse position
            if (Physics.Raycast(ray, out RaycastHit hitInfo))           // If we get a hit with that ray
            {
                if (hitInfo.collider.gameObject != null && (hitInfo.collider.gameObject.tag == "PopulationBuilding" ||      // We see if the gameobject hitted
                                                            hitInfo.collider.tag == "Road" || 
                                                            hitInfo.collider.tag == "ResourceBuilding"))                        // is a good one
                {
                    selectedObjectToDelete = hitInfo.collider.gameObject;
                    originalMaterial = selectedObjectToDelete.GetComponentInChildren<Renderer>().materials;
                    selectedObjectToDelete.GetComponentInChildren<Renderer>().materials = deletingMaterial;

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (selectedObjectToDelete.tag == "PopulationBuilding")
                        {
                            PopulationBuilding buildingScript = hitInfo.collider.gameObject.GetComponent<PopulationBuilding>();
                            gameManager.SetNoBuilding(gameManager.GetNoBuildings() - 1);
                            gameManager.AddPop(-buildingScript.GetPopulation());
                            gameManager.AddGold(-buildingScript.GetGoldIncrease());
                            gameManager.gold += (int)(buildingScript.GoldCost * divisbleReturn);
                            gameManager.food += (int)(buildingScript.FoodCost * divisbleReturn);
                            gameManager.energy += (int)(buildingScript.EnergyCost * divisbleReturn);
                            gameManager.crystal += (int)(buildingScript.CrystalCost * divisbleReturn);
                            gameManager.stone += (int)(buildingScript.StoneCost * divisbleReturn);
                            grid.setNodesUnoccupied(buildingScript.getGridWidth(), buildingScript.getGridHeight(), grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>());
                        }
                        else if(selectedObjectToDelete.tag == "ResourceBuilding")
                        {
                            ProductionBuilding buildingScript = hitInfo.collider.gameObject.GetComponent<ProductionBuilding>();
                            gameManager.AddFood(-buildingScript.GetFoodIncrease());
                            gameManager.AddEnergy(-buildingScript.GetEnergyIncrease());
                            gameManager.AddStone(-buildingScript.GetStoneIncrease());
                            gameManager.AddCrystal(-buildingScript.GetCrystalIncrease());
                            gameManager.gold += (int)(buildingScript.GoldCost * divisbleReturn);
                            gameManager.food += (int)(buildingScript.FoodCost * divisbleReturn);
                            gameManager.energy += (int)(buildingScript.EnergyCost * divisbleReturn);
                            gameManager.crystal += (int)(buildingScript.CrystalCost * divisbleReturn);
                            gameManager.stone += (int)(buildingScript.StoneCost * divisbleReturn);
                            grid.setNodesUnoccupied(buildingScript.getGridWidth(), buildingScript.getGridHeight(), grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>());
                        }
                        else
                        {
                            grid.setNodesUnoccupied(selectedObjectToDelete.GetComponent<BuildingCost>().getGridWidth(), selectedObjectToDelete.GetComponent<BuildingCost>().getGridHeight(), grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>());
                            grid.checkTilesRoads();
                            updateRoadsJunction();
                        }
                        
                        grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>().setOcupied(false);
                        Destroy(selectedObjectToDelete);
                        deleteBuildingSound.Play();
                        selectedObjectToDelete = null;
                    }
                }
            }
        }

    }

    /********************************************************************************************************************************/

    // Private functions
    // Return the gameobject node active in the world nearest to the gameobject given
    private GameObject getNearestActiveNode(GameObject gObject)
    {
        GameObject res = null;
        float distanceNode = float.MaxValue;
        float dist = 0;
        
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (!tiles[i, j].GetComponent<Node>().isOcupied() && tiles[i, j].activeInHierarchy)
                {
                    dist = Vector3.Distance(tiles[i, j].transform.position, gObject.transform.position);
                    if (dist < distanceNode)
                    {
                        distanceNode = dist;
                        res = tiles[i, j];
                    }
                }
            }
        }

        return res;
    }

    // Return the gameobject node, active or not, nearest to the gameobject given
    private GameObject getNearestNode(GameObject gObject)
    {
        GameObject res = null;
        float distanceNode = float.MaxValue;
        float dist = 0;

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                //if (!tiles[i, j].GetComponent<Node>().isOcupied())
                //{
                    dist = Vector3.Distance(tiles[i, j].transform.position, gObject.transform.position);
                    if (dist < distanceNode)
                    {
                        //Debug.Log(dist);
                        distanceNode = dist;
                        res = tiles[i, j];
                    }
                //}
            }
        }

        return res;
    }

    // Rotate a gameobject around the axis y
    private void rotateAroundY(GameObject go, float degrees)
    {
        go.transform.Rotate(0, degrees, 0);
    }

    // Build the building centered between the nodes
    private Vector3 buildCentered(List<GameObject> nodes)
    {
        float x = 0, z = 0;

        for(int i=0; i< nodes.Count; i++)
        {
            x += nodes[i].GetComponent<Node>().transform.position.x;
            z += nodes[i].GetComponent<Node>().transform.position.z;
        }

        x /= nodes.Count;
        z /= nodes.Count;

        Vector3 res = new Vector3(x, 0, z);
        return res;
    }

    // Build a line of roads betweem the first node and the last node
    private void buildLineRoads(Node firstNode, Node lastNode)
    {
        // First we have to know in which direction we are building
        float lineX = firstNode.transform.position.x - lastNode.transform.position.x;
        float lineZ = firstNode.transform.position.z - lastNode.transform.position.z;

        if (lineX < 0)   // Build to the right
        {
            for (int i = firstNode.getPosX() + 1; i < lastNode.getPosX(); i++)
            {
                GameObject tileToBuild = grid.getTile(i, firstNode.getPosY());
                Instantiate(roadToPlace, tileToBuild.transform.position, roadShadow.transform.rotation);
                buildingPlaceParticles.transform.position = tileToBuild.transform.position;
                buildingPlaceParticles.Play();
                tileToBuild.GetComponent<Node>().setOcupied(true);
                tileToBuild.GetComponent<Node>().setRoad(true);
                gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            }
        }
        else if (lineX > 0) // Build to the left
        {
            for (int i = lastNode.getPosX() + 1; i < firstNode.getPosX(); i++)
            {
                GameObject tileToBuild = grid.getTile(i, firstNode.getPosY());
                Instantiate(roadToPlace, tileToBuild.transform.position, roadShadow.transform.rotation);
                buildingPlaceParticles.transform.position = tileToBuild.transform.position;
                buildingPlaceParticles.Play();
                tileToBuild.GetComponent<Node>().setOcupied(true);
                tileToBuild.GetComponent<Node>().setRoad(true);
                gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            }
        }
        else if (lineZ < 0) // Build up
        {
            for (int i = firstNode.getPosY() + 1; i < lastNode.getPosY(); i++)
            {
                GameObject tileToBuild = grid.getTile(firstNode.getPosX(), i);
                Instantiate(roadToPlace, tileToBuild.transform.position, roadShadow.transform.rotation);
                buildingPlaceParticles.transform.position = tileToBuild.transform.position;
                buildingPlaceParticles.Play();
                tileToBuild.GetComponent<Node>().setOcupied(true);
                tileToBuild.GetComponent<Node>().setRoad(true);
                gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            }
        }
        else if (lineZ > 0) // Build down
        {
            for (int i = lastNode.getPosY() + 1; i < firstNode.getPosY(); i++)
            {
                GameObject tileToBuild = grid.getTile(firstNode.getPosX(), i);
                Instantiate(roadToPlace, tileToBuild.transform.position, roadShadow.transform.rotation);
                buildingPlaceParticles.transform.position = tileToBuild.transform.position;
                buildingPlaceParticles.Play();
                tileToBuild.GetComponent<Node>().setOcupied(true);
                tileToBuild.GetComponent<Node>().setRoad(true);
                gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
            }
        }
    }

    // Update roads to check junctions
    private void updateRoadsJunction()
    {
        for(int i=0; i < roads.Length; i++)
        {
            roads[i].GetComponent<Road>().updateJunction();
        }
    }

    // Reset all the values to the originals
    private void reSetValues()
    {
        buildingToPlace = null;
        farmToPlace = null;
        batteryToPlace = null;
        stoneMineToPlace = null;
        crystalMineToPlace = null;
        customCursor.gameObject.SetActive(false);
        Cursor.visible = true;
        grid.setTilesActive(false);
        roadToPlace = null;
        initialToPlace = null;
        initialShadow.SetActive(false);
        roadShadow.SetActive(false);
        buildingShadow.SetActive(false);
        farmShadow.SetActive(false);
        batteryShadow.SetActive(false);
        stoneMineShadow.SetActive(false);
        crystalMineShadow.SetActive(false);
        isDeleting = false;
    }

    // Get all the materials of the shadow buildings
    private void getShadowMaterials()
    {
        buildingShadowMaterial = buildingShadow.GetComponentInChildren<Renderer>().material;
        roadShadowMaterial = roadShadow.GetComponentInChildren<Renderer>().material;
        farmShadowMaterial = farmShadow.GetComponentInChildren<Renderer>().material;
        batteryShadowMaterial = batteryShadow.GetComponentInChildren<Renderer>().material;
        stoneMineShadowMaterial = stoneMineShadow.GetComponentInChildren<Renderer>().material;
        crystalMineShadowMaterial = crystalMineShadow   .GetComponentInChildren<Renderer>().material;
    }

    /* Instantiate a building in the nodes necessary
     * 
     * building: the building to instantiate
     * shadowScript: the script of the shadow object, to get the width and height
     */
    private void createBuilding(GameObject building, GameObject shadow)
    {
        if (lastNearActiveNode.activeInHierarchy && grid.areNodesFree(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()))
        {
            building.GetComponent<BuildingCost>().setWH(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight());
            buildPos = buildCentered(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));
            grid.setNodesOccupied(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>());
            Instantiate(building, new Vector3(buildPos.x, 0f, buildPos.z), shadow.transform.rotation);

            buildingPlaceSound.Play();
            buildingPlaceParticles.transform.position = new Vector3(lastNearActiveNode.transform.position.x, 0, lastNearActiveNode.transform.position.z);
            buildingPlaceParticles.Play();

            gameManager.BuyBuilding(building.GetComponent<BuildingCost>());

            // If the sifht is down, continue 
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Cursor.visible = true;
                grid.setTilesActive(false);
                shadow.SetActive(false);
                buildingToPlace = null;
                farmToPlace = null;
                batteryToPlace = null;
                stoneMineToPlace = null;
                crystalMineToPlace = null;
            }
        }
    }

    /********************************************************************************************************************************/

    // Button event to create a building
    public void createBuilding(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            buildingToPlace = building;
            isDeleting = false;
            buildingShadow.SetActive(true);
        }
    }

    //button event to create a farm
    public void createFarm(GameObject farm)
    {
        if (initialPlaced)
        {
            if (gameManager.GetGold() - farm.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - farm.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - farm.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - farm.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - farm.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            farmToPlace = farm;
            isDeleting = false;
            farmShadow.SetActive(true);
        }
        
    }

    //button event to create a battery
    public void createBattery(GameObject battery)
    {
        if (initialPlaced)
        {
            if (gameManager.GetGold() - battery.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - battery.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - battery.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - battery.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - battery.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            batteryToPlace = battery;
            isDeleting = false;
            batteryShadow.SetActive(true);
        }
    }

    //button event to create a StoneMine
    public void createStoneMine(GameObject StoneMine)
    {
        if (initialPlaced && grid.isStoneAvailable())
        {
            if (gameManager.GetGold() - StoneMine.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - StoneMine.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - StoneMine.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - StoneMine.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - StoneMine.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesStoneActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            stoneMineToPlace = StoneMine;
            isDeleting = false;
            stoneMineShadow.SetActive(true);
        }
    }

    //button event to create a CrystalMine
    public void createCrystalMine(GameObject CrystalMine)
    {
        if (initialPlaced && grid.isCrystalAvailable())
        {
            if (gameManager.GetGold() - CrystalMine.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - CrystalMine.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - CrystalMine.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - CrystalMine.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - CrystalMine.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesCrystalActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            crystalMineToPlace = CrystalMine;
            isDeleting = false;
            crystalMineShadow.SetActive(true);
        }
    }

    // Button event to create a road
    public void createRoad(GameObject road)
    {
        if (initialPlaced)
        {
            if (gameManager.GetGold() - road.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetFood() - road.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetEnergy() - road.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetStone() - road.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetCrystal() - road.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesAdyacentRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            roadToPlace = road;
            isDeleting = false;
            roadShadow.SetActive(true);
        }
    }

    // Button event to create initial building
    public void createInitial(GameObject building)
    {
        if (!initialPlaced)
        {
            grid.setTilesActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            initialToPlace = building;
            isDeleting = false;
            initialShadow.SetActive(true);
        }
    }

    // Button event to delete a building or a road
    public void deleteBuilding()
    {
        if (isDeleting)
            isDeleting = false;
        else
            isDeleting = true;
    }
}