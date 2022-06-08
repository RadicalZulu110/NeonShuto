using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Buildings : MonoBehaviour
{
    private GameObject T1HouseToPlace, T2HouseToPlace, T3HouseToPlace, T1WaterToPlace, T2WaterToPlace, T3WaterToPlace, roadToPlace, initialToPlace, T1FoodToPlace, T2FoodToPlace, T3FoodToPlace, T1PowerToPlace, T2PowerToPlace, T3PowerToPlace, crystalMineToPlace, stoneMineToPlace, B_ResourceStorageToPlace, B_FoodStorageToPlace;
    public CustomCursor customCursor;
    public Grid grid;
    public GameObject[,] tiles;
    public Camera camera;
    public GameObject initialShadow, roadShadow, T1HouseShadow, T2HouseShadow, T3HouseShadow, T1WaterShadow, T2WaterShadow, T3WaterShadow, T1FoodShadow, T2FoodShadow, T3FoodShadow, T1PowerShadow, T2PowerShadow, T3PowerShadow, stoneMineShadow, crystalMineShadow, B_ResourceStorageShadow, B_FoodStoageShadow;
    private BuildingCost T1HouseShadowScript, initialShadowScript, roadShadowScript, T2HouseShadowScript, T3HouseShadowScript, T1WaterShadowScript, T2WaterShadowScript, T3WaterShadowScript, T1FoodShadowScript, T2FoodShadowScript, T3FoodShadowScript, T1PowerShadowScript, T2PowerShadowScript, T3PowerShadowScript, stoneMineShadowScript, crystalMineShadowScript, B_ResourceStorageShadowScript, B_FoodStoageShadowScript;
    private Material roadShadowMaterial, T1HouseShadowMaterial, T2HouseShadowMaterial, T3HouseShadowMaterial, T1WaterShadowMaterial, T2WaterShadowMaterial, T3WaterShadowMaterial, T1FoodShadowMaterial, T2FoodShadowMaterial, T3FoodShadowMaterial, T1PowerShadowMaterial, T2PowerShadowMaterial, T3PowerShadowMaterial, stoneMineShadowMaterial, crystalMineShadowMaterial, B_ResourceStorageShadowMaterial, B_FoodStorageShadowMaterial;
    public AudioSource buildingPlaceSound, buildingRotateSound, deleteBuildingSound;
    public ParticleSystem buildingPlaceParticles;

    GameObject nearNode, lastNearActiveNode, firstNodeRoad, lastNodeRoad;
    public bool isDeleting;
    public GameManager gameManager;//need to change naming convention for this to be somthing else rather than gameManager
    public BuildingCost buildingCost;
    private GameObject selectedObjectToDelete;
    private Material[] originalMaterial;
    public Material[] deletingMaterial;
    private Vector3 buildPos;
    
    private bool firstRoadPlaced, initialPlaced;
    private GameObject[] roads;
    public float divisbleReturn;

    public Button DeleteButton;
    public Color ActiveColor;
    public Color InactiveColor;

    private StartingConstruction heroBuilding;
    private List<StorageBuilding> storageBuildings;
    private List<FoodBuilding> foodBuildings;
    private List<StoneMiner> stoneMinerBuildings;
    private List<CrystalMiner> crystalMinerBuildings;

    public NavMeshSurface surface;
    private bool refreshNavMesh;
    public float polutionMultiplicator, waterMultiplicator;
    GameObject  actualNodeRoad = null;

    // Start is called before the first frame update
    void Start()
    {
        isDeleting = false;
        storageBuildings = new List<StorageBuilding>();
        foodBuildings = new List<FoodBuilding>();
        stoneMinerBuildings = new List<StoneMiner>();
        crystalMinerBuildings = new List<CrystalMiner>();
        firstRoadPlaced = false;
        initialPlaced = false;
        getShadowMaterials();
        GetShadowScripts();
        refreshNavMesh = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshNavMesh)
        {
            surface.BuildNavMesh();
            refreshNavMesh = false;
        }

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
                grid.MakeNodesHL(grid.getNodes(initialShadowScript.getGridWidth(), initialShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
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
            grid.MakeNodesHL(grid.getNodes(roadShadowScript.getGridWidth(), roadShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R) && !firstRoadPlaced)
            {
                rotateAroundY(roadShadow, 90);
                buildingRotateSound.Play();
            }
        }

        //T1 House Shadow
        if (T1HouseShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);
            
            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T1HouseShadowScript.getGridWidth(), T1HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T1HouseShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T1HouseShadow.GetComponentInChildren<Renderer>().material = T1HouseShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T1HouseShadowScript.getGridWidth(), T1HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T1HouseShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T1HouseShadowScript.getGridWidth(), T1HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T1HouseShadow, 90);
                buildingRotateSound.Play();
                T1HouseShadowScript.RotateBuilding();
            }
            
        }

        //T2 House Shadow
        if (T2HouseShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T2HouseShadowScript.getGridWidth(), T2HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T2HouseShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T2HouseShadow.GetComponentInChildren<Renderer>().material = T2HouseShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T2HouseShadowScript.getGridWidth(), T2HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T2HouseShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T2HouseShadowScript.getGridWidth(), T2HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T2HouseShadow, 90);
                buildingRotateSound.Play();
                T2HouseShadowScript.RotateBuilding();

            }

        }

        //T3 House Shadow
        if (T3HouseShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T3HouseShadowScript.getGridWidth(), T3HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T3HouseShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T3HouseShadow.GetComponentInChildren<Renderer>().material = T3HouseShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T3HouseShadowScript.getGridWidth(), T3HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T3HouseShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T3HouseShadowScript.getGridWidth(), T3HouseShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T3HouseShadow, 90);
                buildingRotateSound.Play();
                T3HouseShadowScript.RotateBuilding();

            }

        }

        //T1 Water Shadow
        if (T1WaterShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T1WaterShadowScript.getGridWidth(), T1WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T1WaterShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T1WaterShadow.GetComponentInChildren<Renderer>().material = T1WaterShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T1WaterShadowScript.getGridWidth(), T1WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T1WaterShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T1WaterShadowScript.getGridWidth(), T1WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T1WaterShadow, 90);
                buildingRotateSound.Play();
                T1WaterShadowScript.RotateBuilding();

            }

        }

        //T2 Water Shadow
        if (T2WaterShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T2WaterShadowScript.getGridWidth(), T2WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T2WaterShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T2WaterShadow.GetComponentInChildren<Renderer>().material = T2WaterShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T2WaterShadowScript.getGridWidth(), T2WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T2WaterShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T2WaterShadowScript.getGridWidth(), T2WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T2WaterShadow, 90);
                buildingRotateSound.Play();
                T2WaterShadowScript.RotateBuilding();

            }

        }

        //T3 Water Shadow
        if (T3WaterShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T3WaterShadowScript.getGridWidth(), T3WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T3WaterShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T3WaterShadow.GetComponentInChildren<Renderer>().material = T3WaterShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            buildPos = buildCentered(grid.getNodes(T3WaterShadowScript.getGridWidth(), T3WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T3WaterShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T3WaterShadowScript.getGridWidth(), T3WaterShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T3WaterShadow, 90);
                buildingRotateSound.Play();
                T3WaterShadowScript.RotateBuilding();

            }

        }

        //T1 Food shadow
        if (T1FoodShadow.activeInHierarchy )
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T1FoodShadowScript.getGridWidth(), T1FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(T1FoodShadowScript.getGridWidth(), T1FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T1FoodShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T1FoodShadow.GetComponentInChildren<Renderer>().material = T1FoodShadowMaterial;
                lastNearActiveNode = nearNode;
            }

           
            buildPos = buildCentered(grid.getNodes(T1FoodShadowScript.getGridWidth(), T1FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T1FoodShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T1FoodShadowScript.getGridWidth(), T1FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T1FoodShadow, 90);
                buildingRotateSound.Play();
                T1FoodShadowScript.RotateBuilding();

            }

        }

        //T2 Food shadow
        if (T2FoodShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T2FoodShadowScript.getGridWidth(), T2FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(T2FoodShadowScript.getGridWidth(), T2FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T2FoodShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T2FoodShadow.GetComponentInChildren<Renderer>().material = T2FoodShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(T2FoodShadowScript.getGridWidth(), T2FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T2FoodShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T2FoodShadowScript.getGridWidth(), T2FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T2FoodShadow, 90);
                buildingRotateSound.Play();
                T2FoodShadowScript.RotateBuilding();

            }

        }

        //T3 Food shadow
        if (T3FoodShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T3FoodShadowScript.getGridWidth(), T3FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(T3FoodShadowScript.getGridWidth(), T3FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T3FoodShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T3FoodShadow.GetComponentInChildren<Renderer>().material = T3FoodShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(T3FoodShadowScript.getGridWidth(), T3FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T3FoodShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T3FoodShadowScript.getGridWidth(), T3FoodShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T3FoodShadow, 90);
                buildingRotateSound.Play();
                T3FoodShadowScript.RotateBuilding();

            }

        }

        //T1 Power shadow
        if (T1PowerShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T1PowerShadowScript.getGridWidth(), T1PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T1PowerShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T1PowerShadow.GetComponentInChildren<Renderer>().material = T1PowerShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(T1PowerShadowScript.getGridWidth(), T1PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T1PowerShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T1PowerShadowScript.getGridWidth(), T1PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T1PowerShadow, 90);
                buildingRotateSound.Play();
                T1PowerShadowScript.RotateBuilding();

            }

        }

        //T2 Power shadow
        if (T2PowerShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T2PowerShadowScript.getGridWidth(), T2PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T2PowerShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T2PowerShadow.GetComponentInChildren<Renderer>().material = T2PowerShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(T2PowerShadowScript.getGridWidth(), T2PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T2PowerShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T2PowerShadowScript.getGridWidth(), T2PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T2PowerShadow, 90);
                buildingRotateSound.Play();
                T2PowerShadowScript.RotateBuilding();

            }

        }

        //T3 Power shadow
        if (T3PowerShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(T3PowerShadowScript.getGridWidth(), T3PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                T3PowerShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                T3PowerShadow.GetComponentInChildren<Renderer>().material = T3PowerShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(T3PowerShadowScript.getGridWidth(), T3PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            T3PowerShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(T3PowerShadowScript.getGridWidth(), T3PowerShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(T3PowerShadow, 90);
                buildingRotateSound.Play();
                T3PowerShadowScript.RotateBuilding();
            }

        }

        //Food Storage shadow
        if (B_FoodStoageShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(B_FoodStoageShadowScript.getGridWidth(), B_FoodStoageShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(B_FoodStoageShadowScript.getGridWidth(), B_FoodStoageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                B_FoodStoageShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                B_FoodStoageShadow.GetComponentInChildren<Renderer>().material = T1PowerShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(B_FoodStoageShadowScript.getGridWidth(), B_FoodStoageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            B_FoodStoageShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(B_FoodStoageShadowScript.getGridWidth(), B_FoodStoageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(B_FoodStoageShadow, 90);
                buildingRotateSound.Play();
                B_FoodStoageShadowScript.RotateBuilding();

            }

        }

        //Food Resource shadow
        if (B_ResourceStorageShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesFree(B_ResourceStorageShadowScript.getGridWidth(), B_ResourceStorageShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(B_ResourceStorageShadowScript.getGridWidth(), B_ResourceStorageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                B_ResourceStorageShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                B_ResourceStorageShadow.GetComponentInChildren<Renderer>().material = T1PowerShadowMaterial;
                lastNearActiveNode = nearNode;
            }


            buildPos = buildCentered(grid.getNodes(B_ResourceStorageShadowScript.getGridWidth(), B_ResourceStorageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            B_ResourceStorageShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(B_ResourceStorageShadowScript.getGridWidth(), B_ResourceStorageShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(B_ResourceStorageShadow, 90);
                buildingRotateSound.Play();
                B_ResourceStorageShadowScript.RotateBuilding();

            }

        }

        //stonemine shadow
        if (stoneMineShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesStone(stoneMineShadowScript.getGridWidth(), stoneMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(stoneMineShadowScript.getGridWidth(), stoneMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                stoneMineShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                stoneMineShadow.GetComponentInChildren<Renderer>().material = stoneMineShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(stoneMineShadowScript.getGridWidth(), stoneMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            stoneMineShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(stoneMineShadowScript.getGridWidth(), stoneMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(stoneMineShadow, 90);
                buildingRotateSound.Play();
                stoneMineShadowScript.RotateBuilding();

            }

        }

        //crystalmine shadow
        if (crystalMineShadow.activeInHierarchy)
        {
            nearNode = getNearestNode(customCursor.gameObject);

            if (!nearNode.activeInHierarchy || !grid.areNodesCrystal(crystalMineShadowScript.getGridWidth(), crystalMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>())
                || !grid.hasRoadAdyacent(crystalMineShadowScript.getGridWidth(), crystalMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()))
            {
                crystalMineShadow.GetComponentInChildren<Renderer>().materials = deletingMaterial;
            }
            else
            {
                crystalMineShadow.GetComponentInChildren<Renderer>().material = crystalMineShadowMaterial;
                lastNearActiveNode = nearNode;
            }

            
            buildPos = buildCentered(grid.getNodes(crystalMineShadowScript.getGridWidth(), crystalMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));
            crystalMineShadow.transform.position = new Vector3(buildPos.x, 0.1f, buildPos.z);
            grid.MakeNodesHL(grid.getNodes(crystalMineShadowScript.getGridWidth(), crystalMineShadowScript.getGridHeight(), nearNode.GetComponent<Node>()));

            if (Input.GetKeyDown(KeyCode.R))
            {
                rotateAroundY(crystalMineShadow, 90);
                buildingRotateSound.Play();
                crystalMineShadowScript.RotateBuilding();

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

            // Create T1 house
            if (T1HouseToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T1HouseToPlace, T1HouseShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            // Create T2 house
            if (T2HouseToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T2HouseToPlace, T2HouseShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            // Create T3 house
            if (T3HouseToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T3HouseToPlace, T3HouseShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            // Create T1 Water
            if (T1WaterToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T1WaterToPlace, T1WaterShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            // Create T2 Water
            if (T2WaterToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T2WaterToPlace, T2WaterShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            // Create T3 Water
            if (T3WaterToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T3WaterToPlace, T3WaterShadow);
                gameManager.SetNoBuilding(gameManager.GetNoBuildings() + 1);
            }

            //Create Food T1
            if (T1FoodToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T1FoodToPlace, T1FoodShadow);
                gameManager.SetNoFarms(gameManager.GetNoFarms() + 1);
                
            }

            //Create Food T2
            if (T2FoodToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T2FoodToPlace, T2FoodShadow);
                gameManager.SetNoFarms(gameManager.GetNoFarms() + 1);

            }

            //Create Food T3
            if (T3FoodToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T3FoodToPlace, T3FoodShadow);
                gameManager.SetNoFarms(gameManager.GetNoFarms() + 1);

            }

            //Create Power T1
            if (T1PowerToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T1PowerToPlace, T1PowerShadow);
                gameManager.SetNoBatterys(gameManager.GetNoBatterys() + 1);
            }

            //Create Power T2
            if (T2PowerToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T2PowerToPlace, T2PowerShadow);
                gameManager.SetNoBatterys(gameManager.GetNoBatterys() + 1);
            }

            //Create Power T3
            if (T3PowerToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(T3PowerToPlace, T3PowerShadow);
                gameManager.SetNoBatterys(gameManager.GetNoBatterys() + 1);
            }

            //Create Food Storage
            if (B_FoodStorageToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(B_FoodStorageToPlace, B_FoodStoageShadow);
                gameManager.SetNoFoodStorage(gameManager.GetNoFoodStorage() + 1);
            }

            //Create Resource Storage
            if (B_ResourceStorageToPlace != null)
            {
                //nearNode = getNearestNode(customCursor.gameObject);

                createBuilding(B_ResourceStorageToPlace, B_ResourceStorageShadow);
                gameManager.SetNoResourceStorage(gameManager.GetNoResourceStorage() + 1);
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
        /*if (Input.GetKeyDown(KeyCode.Mouse0) && roadToPlace != null && !Input.GetKey(KeyCode.LeftShift)) // Create single road
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
        }*/



        if(roadToPlace != null && Input.GetKey(KeyCode.Mouse0))
        {
            if(firstNodeRoad == null)
            {
                firstNodeRoad = getNearestActiveNode(customCursor.gameObject);
            }
            else
            {
                grid.setTilesLineRoadVisible(firstNodeRoad.GetComponent<Node>());
                actualNodeRoad = getNearestActiveNode(customCursor.gameObject);
                grid.MakeNodesHL(grid.GetNodesAvailableBetween(firstNodeRoad.GetComponent<Node>(), actualNodeRoad.GetComponent<Node>()));
            }
        }

        if(roadToPlace != null && (actualNodeRoad != null || firstNodeRoad != null) && !Input.GetKey(KeyCode.Mouse0))
        {
            if(actualNodeRoad == null)
            {
                buildLineRoads(firstNodeRoad.GetComponent<Node>(), firstNodeRoad.GetComponent<Node>());
            }
            else
            {
                buildLineRoads(firstNodeRoad.GetComponent<Node>(), actualNodeRoad.GetComponent<Node>());
            }
            
            firstNodeRoad = null;
            actualNodeRoad = null;
            roadToPlace = null;
            buildingPlaceSound.Play();
            customCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            grid.setTilesActive(false);
            grid.checkTilesRoads();
            roadShadow.SetActive(false);
            updateRoadsJunction();
            surface.BuildNavMesh();
            refreshNavMesh = true;
        }



        // Delete building or road
        if (selectedObjectToDelete != null)
        {
            selectedObjectToDelete.GetComponentInChildren<Renderer>().materials = originalMaterial;
            selectedObjectToDelete = null;
        }

        if (isDeleting) // If Im deleting
        {
            ColorBlock cb = DeleteButton.colors;
            cb.normalColor = ActiveColor;
            cb.selectedColor = ActiveColor;
            DeleteButton.colors = cb;
            // set button color here 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);     // Throw a ray in the mouse position
            if (Physics.Raycast(ray, out RaycastHit hitInfoAux))           // If we get a hit with that ray
            {
                RaycastHit[] hits = Physics.RaycastAll(ray);            // Get all the hits
                Debug.DrawLine(Input.mousePosition, hitInfoAux.collider.transform.position, Color.yellow, 1, true);

                foreach(RaycastHit hitInfo in hits)
                {
                    if (hitInfo.collider.gameObject != null && hitInfo.collider is BoxCollider &&
                                                            (hitInfo.collider.gameObject.tag == "PopulationBuilding" ||      // We see if the gameobject hitted
                                                            hitInfo.collider.tag == "Road" ||
                                                            hitInfo.collider.tag == "ResourceBuilding" ||
                                                            hitInfo.collider.tag == "StorageBuilding"))                        // is a good one
                    {
                        Debug.DrawLine(Input.mousePosition, hitInfo.collider.transform.position, Color.green, 1, true);
                        selectedObjectToDelete = hitInfo.collider.gameObject;
                        originalMaterial = selectedObjectToDelete.GetComponentInChildren<Renderer>().materials;
                        selectedObjectToDelete.GetComponentInChildren<Renderer>().materials = deletingMaterial;

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (selectedObjectToDelete.tag == "PopulationBuilding")
                            {
                                PopulationBuilding buildingScript = selectedObjectToDelete.GetComponent<PopulationBuilding>();
                                gameManager.SetNoBuilding(gameManager.GetNoBuildings() - 1);
                                gameManager.AddPop(-buildingScript.GetPopulation());
                                gameManager.AddGold(-buildingScript.GetGoldIncrease());
                                gameManager.TotalGold += (int)(buildingScript.GoldCost * divisbleReturn);
                                gameManager.TotalFood += (int)(buildingScript.FoodCost * divisbleReturn);
                                gameManager.TotalEnergy += (int)(buildingScript.EnergyCost * divisbleReturn);
                                gameManager.TotalCrystal += (int)(buildingScript.CrystalCost * divisbleReturn);
                                gameManager.TotalStone += (int)(buildingScript.StoneCost * divisbleReturn);
                                grid.setNodesUnoccupied(buildingScript.getNodes());
                            }
                            else if (selectedObjectToDelete.tag == "ResourceBuilding")
                            {
                                ProductionBuilding buildingScript = selectedObjectToDelete.GetComponent<ProductionBuilding>();

                                if (selectedObjectToDelete.GetComponent<FoodBuilding>())
                                {
                                    gameManager.deleteFarm(selectedObjectToDelete);
                                    foodBuildings.Remove(selectedObjectToDelete.GetComponent<FoodBuilding>());
                                }
                                else if (selectedObjectToDelete.GetComponent<StoneMiner>())
                                {
                                    gameManager.deleteStoneMiner(selectedObjectToDelete);
                                    stoneMinerBuildings.Remove(selectedObjectToDelete.GetComponent<StoneMiner>());
                                }
                                else if (selectedObjectToDelete.GetComponent<CrystalMiner>())
                                {
                                    crystalMinerBuildings.Remove(selectedObjectToDelete.GetComponent<CrystalMiner>());
                                }

                                
                                //gameManager.AddFood(-buildingScript.GetFoodIncrease());
                                gameManager.foodCapacity -= (buildingScript.GetPersonalFoodCapacity());//selectedObjectToDelete.GetComponent<FoodBuilding>().PersonalFoodCapacity;
                                gameManager.foodStored -= (buildingScript.GetCurrentFoodStored());//selectedObjectToDelete.GetComponent<FoodBuilding>().currentFoodStored;
                                gameManager.AddEnergy(-buildingScript.GetEnergyIncrease());
                                //gameManager.AddStone(-buildingScript.GetStoneIncrease());
                                gameManager.stoneCapacity -= (buildingScript.GetPersonalStoneCapacity());//selectedObjectToDelete.GetComponent<MinerBuilding>().PersonalStoneCapacity;
                                gameManager.stoneStored -= (buildingScript.GetCurrentStoneStored());//selectedObjectToDelete.GetComponent<MinerBuilding>().currentStoneStored;
                                                                                                    //gameManager.AddCrystal(-buildingScript.GetCrystalIncrease());
                                gameManager.crystalCapacity -= (buildingScript.GetPersonalCrystalCapacity());//selectedObjectToDelete.GetComponent<MinerBuilding>().PersonalCrystalCapacity;
                                gameManager.crystalStored -= (buildingScript.GetCurrentCrystalStored());//selectedObjectToDelete.GetComponent<MinerBuilding>().currentCrystalStored;
                                gameManager.TotalGold += (int)(buildingScript.GoldCost * divisbleReturn);
                                gameManager.TotalFood += (int)(buildingScript.FoodCost * divisbleReturn);
                                gameManager.TotalEnergy += (int)(buildingScript.EnergyCost * divisbleReturn);
                                gameManager.TotalCrystal += (int)(buildingScript.CrystalCost * divisbleReturn);
                                gameManager.TotalStone += (int)(buildingScript.StoneCost * divisbleReturn);
                                grid.setNodesUnoccupied(buildingScript.getNodes());

                                // Truck associated with that production building
                                if (buildingScript.getTruckRecollecting())
                                {
                                    buildingScript.getTruckRecollecting().GetComponent<Truck>().MakeAvailable();
                                }
                            }
                            else if (selectedObjectToDelete.tag == "StorageBuilding")
                            {
                                storageBuildings.Remove(selectedObjectToDelete.GetComponent<StorageBuilding>());

                                if (selectedObjectToDelete.GetComponent<FoodStorageBuilding>())
                                {
                                    gameManager.foodCapacity -= selectedObjectToDelete.GetComponent<FoodStorageBuilding>().GetMaxFood();
                                    gameManager.DeleteFoodStorageBuilding(selectedObjectToDelete.GetComponent<FoodStorageBuilding>());
                                    grid.setNodesUnoccupied(selectedObjectToDelete.GetComponent<FoodStorageBuilding>().getNodes());
                                }
                                else if (selectedObjectToDelete.GetComponent<ResourceStorageBuilding>())
                                {
                                    gameManager.stoneCapacity -= selectedObjectToDelete.GetComponent<ResourceStorageBuilding>().GetMaxStone();
                                    gameManager.crystalCapacity -= selectedObjectToDelete.GetComponent<ResourceStorageBuilding>().GetMaxCrystal();
                                    gameManager.DeleteResourceStorageBuilding(selectedObjectToDelete.GetComponent<ResourceStorageBuilding>());
                                    grid.setNodesUnoccupied(selectedObjectToDelete.GetComponent<ResourceStorageBuilding>().getNodes());
                                }

                                
                            }
                            else
                            {
                                //grid.setNodesUnoccupied(selectedObjectToDelete.GetComponent<BuildingCost>().getGridWidth(), selectedObjectToDelete.GetComponent<BuildingCost>().getGridHeight(), grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>());
                                grid.ResetNodes(grid.getNodes(selectedObjectToDelete.GetComponent<BuildingCost>().getGridWidth(), selectedObjectToDelete.GetComponent<BuildingCost>().getGridHeight(), grid.getTile(selectedObjectToDelete.transform.position).GetComponent<Node>()));
                                grid.checkTilesRoads();
                                updateRoadsJunction();
                                heroBuilding.RemoveRoad(selectedObjectToDelete);
                                for (int i = 0; i < storageBuildings.Count; i++)
                                {
                                    storageBuildings[i].RemoveRoad(selectedObjectToDelete);
                                }

                                for (int i = 0; i < foodBuildings.Count; i++)
                                {
                                    foodBuildings[i].RemoveRoad(selectedObjectToDelete);
                                }

                                for (int i = 0; i < stoneMinerBuildings.Count; i++)
                                {
                                    stoneMinerBuildings[i].RemoveRoad(selectedObjectToDelete);
                                }

                                for (int i = 0; i < crystalMinerBuildings.Count; i++)
                                {
                                    crystalMinerBuildings[i].RemoveRoad(selectedObjectToDelete);
                                }

                                refreshNavMesh = true;
                            }

                            gameManager.DeleteBuilding(selectedObjectToDelete);
                            Destroy(selectedObjectToDelete);
                            deleteBuildingSound.Play();
                            selectedObjectToDelete = null;
                            
                        }
                        
                        break;
                    }
                }
            }
        }
        else
        {
            ColorBlock cb = DeleteButton.colors;
            cb.normalColor = InactiveColor;
            cb.selectedColor = InactiveColor;
            DeleteButton.colors = cb;
        }

    }

    /********************************************************************************************************************************/

    // Public functions
    // Delete the gameobject given
    public void DeleteBuilding(GameObject objectToDelete)
    {
        if (objectToDelete.tag == "PopulationBuilding")
        {
            PopulationBuilding buildingScript = objectToDelete.GetComponent<PopulationBuilding>();
            gameManager.SetNoBuilding(gameManager.GetNoBuildings() - 1);
            gameManager.AddPop(-buildingScript.GetPopulation());
            gameManager.AddGold(-buildingScript.GetGoldIncrease());
            gameManager.TotalGold += (int)(buildingScript.GoldCost * divisbleReturn);
            gameManager.TotalFood += (int)(buildingScript.FoodCost * divisbleReturn);
            gameManager.TotalEnergy += (int)(buildingScript.EnergyCost * divisbleReturn);
            gameManager.TotalCrystal += (int)(buildingScript.CrystalCost * divisbleReturn);
            gameManager.TotalStone += (int)(buildingScript.StoneCost * divisbleReturn);
            grid.setNodesUnoccupied(buildingScript.getNodes());
        }
        else if (objectToDelete.tag == "ResourceBuilding")
        {
            ProductionBuilding buildingScript = objectToDelete.GetComponent<ProductionBuilding>();

            if (objectToDelete.GetComponent<FoodBuilding>())
            {
                gameManager.deleteFarm(objectToDelete);
            }
            else if (objectToDelete.GetComponent<StoneMiner>())
            {
                gameManager.deleteStoneMiner(objectToDelete);
            }
            else if (objectToDelete.GetComponent<CrystalMiner>())
            {
                gameManager.deleteCrystalMiner(objectToDelete);
            }


            //gameManager.AddFood(-buildingScript.GetFoodIncrease());
            gameManager.foodCapacity -= (buildingScript.GetPersonalFoodCapacity());//selectedObjectToDelete.GetComponent<FoodBuilding>().PersonalFoodCapacity;
            gameManager.foodStored -= (buildingScript.GetCurrentFoodStored());//selectedObjectToDelete.GetComponent<FoodBuilding>().currentFoodStored;
            gameManager.AddEnergy(-buildingScript.GetEnergyIncrease());
            //gameManager.AddStone(-buildingScript.GetStoneIncrease());
            gameManager.stoneCapacity -= (buildingScript.GetPersonalStoneCapacity());//selectedObjectToDelete.GetComponent<MinerBuilding>().PersonalStoneCapacity;
            gameManager.stoneStored -= (buildingScript.GetCurrentStoneStored());//selectedObjectToDelete.GetComponent<MinerBuilding>().currentStoneStored;
                                                                                //gameManager.AddCrystal(-buildingScript.GetCrystalIncrease());
            gameManager.crystalCapacity -= (buildingScript.GetPersonalCrystalCapacity());//selectedObjectToDelete.GetComponent<MinerBuilding>().PersonalCrystalCapacity;
            gameManager.crystalStored -= (buildingScript.GetCurrentCrystalStored());//selectedObjectToDelete.GetComponent<MinerBuilding>().currentCrystalStored;
            grid.setNodesUnoccupied(buildingScript.getNodes());

            // Truck associated with that production building
            if (buildingScript.getTruckRecollecting())
            {
                buildingScript.getTruckRecollecting().GetComponent<Truck>().MakeAvailable();
            }
        }
        else if (objectToDelete.tag == "StorageBuilding")
        {
            storageBuildings.Remove(objectToDelete.GetComponent<StorageBuilding>());

            if (objectToDelete.GetComponent<FoodStorageBuilding>())
            {
                gameManager.foodCapacity -= objectToDelete.GetComponent<FoodStorageBuilding>().GetMaxFood();
                gameManager.DeleteFoodStorageBuilding(objectToDelete.GetComponent<FoodStorageBuilding>());
                grid.setNodesUnoccupied(objectToDelete.GetComponent<FoodStorageBuilding>().getNodes());
            }
            else if (objectToDelete.GetComponent<ResourceStorageBuilding>())
            {
                gameManager.stoneCapacity -= objectToDelete.GetComponent<ResourceStorageBuilding>().GetMaxStone();
                gameManager.crystalCapacity -= objectToDelete.GetComponent<ResourceStorageBuilding>().GetMaxCrystal();
                gameManager.DeleteResourceStorageBuilding(objectToDelete.GetComponent<ResourceStorageBuilding>());
                grid.setNodesUnoccupied(objectToDelete.GetComponent<ResourceStorageBuilding>().getNodes());
            }


        }
        else
        {
            grid.setNodesUnoccupied(objectToDelete.GetComponent<BuildingCost>().getGridWidth(), objectToDelete.GetComponent<BuildingCost>().getGridHeight(), grid.getTile(objectToDelete.transform.position).GetComponent<Node>());
            grid.checkTilesRoads();
            updateRoadsJunction();
            heroBuilding.CheckAdyacentRoads();
            for (int i = 0; i < storageBuildings.Count; i++)
            {
                storageBuildings[i].CheckAdyacentRoads();
            }
            for (int i = 0; i < foodBuildings.Count; i++)
            {
                foodBuildings[i].CheckAdyacentRoads();
            }
            for (int i = 0; i < stoneMinerBuildings.Count; i++)
            {
                stoneMinerBuildings[i].RemoveRoad(selectedObjectToDelete);
            }

            for (int i = 0; i < crystalMinerBuildings.Count; i++)
            {
                crystalMinerBuildings[i].RemoveRoad(selectedObjectToDelete);
            }
            refreshNavMesh = true;
        }

        Destroy(objectToDelete);
        deleteBuildingSound.Play();
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
        if(firstNode == lastNode)
        {
            Instantiate(roadToPlace, firstNode.transform.position, roadShadow.transform.rotation);
            buildingPlaceParticles.transform.position = firstNode.transform.position;
            buildingPlaceParticles.Play();
            firstNode.GetComponent<Node>().setOcupied(true);
            firstNode.GetComponent<Node>().setRoad(true);
            gameManager.BuyBuilding(roadToPlace.GetComponent<BuildingCost>());
        }

        // First we have to know in which direction we are building
        float lineX = firstNode.transform.position.x - lastNode.transform.position.x;
        float lineZ = firstNode.transform.position.z - lastNode.transform.position.z;

        if (lineX < 0)   // Build to the right
        {
            for (int i = firstNode.getPosX(); i <= lastNode.getPosX(); i++)
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
            for (int i = lastNode.getPosX(); i <= firstNode.getPosX(); i++)
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
            for (int i = firstNode.getPosY(); i <= lastNode.getPosY(); i++)
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
            for (int i = lastNode.getPosY(); i <= firstNode.getPosY(); i++)
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
        T1HouseToPlace = null;
        T1FoodToPlace = null;
        T1PowerToPlace = null;
        stoneMineToPlace = null;
        crystalMineToPlace = null;
        customCursor.gameObject.SetActive(false);
        Cursor.visible = true;
        grid.setTilesActive(false);
        roadToPlace = null;
        initialToPlace = null;
        initialShadow.SetActive(false);
        roadShadow.SetActive(false);
        T1HouseShadow.SetActive(false);
        T2HouseShadow.SetActive(false);
        T3HouseShadow.SetActive(false);
        T1WaterShadow.SetActive(false);
        T2WaterShadow.SetActive(false);
        T3WaterShadow.SetActive(false);
        T1FoodShadow.SetActive(false);
        T2FoodShadow.SetActive(false);
        T3FoodShadow.SetActive(false);
        T1PowerShadow.SetActive(false);
        T2PowerShadow.SetActive(false);
        T3PowerShadow.SetActive(false);
        stoneMineShadow.SetActive(false);
        crystalMineShadow.SetActive(false);
        B_FoodStoageShadow.SetActive(false);
        B_ResourceStorageShadow.SetActive(false);
        isDeleting = false;
    }

    // Get all the materials of the shadow buildings
    private void getShadowMaterials()
    {
        T1HouseShadowMaterial = T1HouseShadow.GetComponentInChildren<Renderer>().material;
        T2HouseShadowMaterial = T2HouseShadow.GetComponentInChildren<Renderer>().material;
        T3HouseShadowMaterial = T3HouseShadow.GetComponentInChildren<Renderer>().material;

        T1WaterShadowMaterial = T1WaterShadow.GetComponentInChildren<Renderer>().material;
        T2WaterShadowMaterial = T2WaterShadow.GetComponentInChildren<Renderer>().material;
        T3WaterShadowMaterial = T3WaterShadow.GetComponentInChildren<Renderer>().material;

        T1FoodShadowMaterial = T1FoodShadow.GetComponentInChildren<Renderer>().material;
        T2FoodShadowMaterial = T2FoodShadow.GetComponentInChildren<Renderer>().material;
        T3FoodShadowMaterial = T3FoodShadow.GetComponentInChildren<Renderer>().material;

        T1PowerShadowMaterial = T1PowerShadow.GetComponentInChildren<Renderer>().material;
        T2PowerShadowMaterial = T2PowerShadow.GetComponentInChildren<Renderer>().material;
        T3PowerShadowMaterial = T3PowerShadow.GetComponentInChildren<Renderer>().material;

        roadShadowMaterial = roadShadow.GetComponentInChildren<Renderer>().material;
        stoneMineShadowMaterial = stoneMineShadow.GetComponentInChildren<Renderer>().material;
        crystalMineShadowMaterial = crystalMineShadow.GetComponentInChildren<Renderer>().material;
        B_FoodStorageShadowMaterial = B_FoodStoageShadow.GetComponentInChildren<Renderer>().material;
        B_ResourceStorageShadowMaterial = B_ResourceStorageShadow.GetComponentInChildren<Renderer>().material;
    }

    // Get all the scripts from the shadow game objects
    private void GetShadowScripts()
    {
        T1HouseShadowScript = T1HouseShadow.GetComponent<BuildingCost>();
        initialShadowScript = initialShadow.GetComponent<BuildingCost>();
        roadShadowScript = roadShadow.GetComponent<BuildingCost>();
        T2HouseShadowScript = T2HouseShadow.GetComponent<BuildingCost>();
        T3HouseShadowScript = T3HouseShadow.GetComponent<BuildingCost>();
        T1WaterShadowScript = T1WaterShadow.GetComponent<BuildingCost>();
        T2WaterShadowScript = T2WaterShadow.GetComponent<BuildingCost>();
        T3WaterShadowScript = T3WaterShadow.GetComponent<BuildingCost>();
        T1FoodShadowScript = T1FoodShadow.GetComponent<BuildingCost>();
        T2FoodShadowScript = T2FoodShadow.GetComponent<BuildingCost>();
        T3FoodShadowScript = T3FoodShadow.GetComponent<BuildingCost>();
        T1PowerShadowScript = T1PowerShadow.GetComponent<BuildingCost>();
        T2PowerShadowScript = T2PowerShadow.GetComponent<BuildingCost>();
        T3PowerShadowScript = T3PowerShadow.GetComponent<BuildingCost>();
        stoneMineShadowScript = stoneMineShadow.GetComponent<BuildingCost>();
        crystalMineShadowScript = crystalMineShadow.GetComponent<BuildingCost>();
        B_ResourceStorageShadowScript = B_ResourceStorageShadow.GetComponent<BuildingCost>();
        B_FoodStoageShadowScript = B_FoodStoageShadow.GetComponent<BuildingCost>();
    }

    /* Instantiate a building in the nodes necessary
     * 
     * building: the building to instantiate
     * shadowScript: the script of the shadow object, to get the width and height
     */
    private void createBuilding(GameObject building, GameObject shadow)
    {
        if(shadow.GetComponentInChildren<Renderer>().materials != deletingMaterial)
        {
            GameObject buildCreated;

            if (building.GetComponent<StoneMiner>())
            {
                if (lastNearActiveNode.activeInHierarchy && grid.areNodesStone(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()))
                {
                    building.GetComponent<BuildingCost>().setWH(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight());
                    buildPos = buildCentered(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));
                    grid.setNodesOccupied(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>());
                    
                    buildCreated = Instantiate(building, new Vector3(buildPos.x, 0f, buildPos.z), shadow.transform.rotation);
                    buildCreated.GetComponent<BuildingCost>().setNodes(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));

                    gameManager.addStoneMiner(buildCreated);

                    buildingPlaceSound.Play();
                    buildingPlaceParticles.transform.position = buildPos;
                    Debug.Log(buildPos);
                    buildingPlaceParticles.transform.localScale = new Vector3((building.GetComponent<BuildingCost>().getGridWidth() / 4) / 2, 1, (building.GetComponent<BuildingCost>().getGridHeight() / 4) / 2);
                    buildingPlaceParticles.Play();

                    gameManager.BuyBuilding(building.GetComponent<BuildingCost>());
                    gameManager.AddTreeLife(-buildCreated.GetComponent<BuildingCost>().getTier() * polutionMultiplicator);
                    stoneMinerBuildings.Add(buildCreated.GetComponent<StoneMiner>());

                    // If the sifht is down, continue 
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        Cursor.visible = true;
                        grid.setTilesActive(false);
                        shadow.SetActive(false);
                        T1HouseToPlace = null;
                        T2HouseToPlace = null;
                        T3HouseToPlace = null;
                        T1WaterToPlace = null;
                        T2WaterToPlace = null;
                        T3WaterToPlace = null;
                        T1FoodToPlace = null;
                        T2FoodToPlace = null;
                        T3FoodToPlace = null;
                        T1PowerToPlace = null;
                        T2PowerToPlace = null;
                        T3PowerToPlace = null;
                        stoneMineToPlace = null;
                        crystalMineToPlace = null;
                    }
                }

                return;
            }

            if (building.GetComponent<CrystalMiner>())
            {
                if (lastNearActiveNode.activeInHierarchy && grid.areNodesCrystal(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()))
                {
                    building.GetComponent<BuildingCost>().setWH(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight());
                    buildPos = buildCentered(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));
                    grid.setNodesOccupied(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>());
                    
                    buildCreated = Instantiate(building, new Vector3(buildPos.x, 0f, buildPos.z), shadow.transform.rotation);
                    buildCreated.GetComponent<BuildingCost>().setNodes(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));


                    gameManager.addCrystalMiner(buildCreated);


                    buildingPlaceSound.Play();
                    buildingPlaceParticles.transform.position = buildPos;
                    Debug.Log(buildPos);
                    buildingPlaceParticles.transform.localScale = new Vector3((building.GetComponent<BuildingCost>().getGridWidth() / 4) / 2, 1, (building.GetComponent<BuildingCost>().getGridHeight() / 4) / 2);
                    buildingPlaceParticles.Play();

                    gameManager.BuyBuilding(building.GetComponent<BuildingCost>());
                    gameManager.AddTreeLife(-buildCreated.GetComponent<BuildingCost>().getTier() * polutionMultiplicator);
                    crystalMinerBuildings.Add(buildCreated.GetComponent<CrystalMiner>());

                    // If the sifht is down, continue 
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        Cursor.visible = true;
                        grid.setTilesActive(false);
                        shadow.SetActive(false);
                        T1HouseToPlace = null;
                        T2HouseToPlace = null;
                        T3HouseToPlace = null;
                        T1WaterToPlace = null;
                        T2WaterToPlace = null;
                        T3WaterToPlace = null;
                        T1FoodToPlace = null;
                        T2FoodToPlace = null;
                        T3FoodToPlace = null;
                        T1PowerToPlace = null;
                        T2PowerToPlace = null;
                        T3PowerToPlace = null;
                        stoneMineToPlace = null;
                        crystalMineToPlace = null;
                    }
                }

                return;
            }

            if (lastNearActiveNode.activeInHierarchy && grid.areNodesFree(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()))
            {
                building.GetComponent<BuildingCost>().setWH(shadow.GetComponent<BuildingCost>().getGridWidth(), shadow.GetComponent<BuildingCost>().getGridHeight());
                buildPos = buildCentered(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));
                grid.setNodesOccupied(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>());
                
                buildCreated = Instantiate(building, new Vector3(buildPos.x, 0f, buildPos.z), shadow.transform.rotation);
                buildCreated.GetComponent<BuildingCost>().setNodes(grid.getNodes(building.GetComponent<BuildingCost>().getGridWidth(), building.GetComponent<BuildingCost>().getGridHeight(), lastNearActiveNode.GetComponent<Node>()));

                if (buildCreated.GetComponent<FoodBuilding>())
                {
                    gameManager.addFarm(buildCreated);
                }

                buildingPlaceSound.Play();
                buildingPlaceParticles.transform.position = buildPos;
                Debug.Log(buildPos);
                buildingPlaceParticles.transform.localScale = new Vector3((float)(building.GetComponent<BuildingCost>().getGridWidth() / 4) / 2, 1, (float)(building.GetComponent<BuildingCost>().getGridHeight() / 4) / 2); 
                buildingPlaceParticles.Play();

                gameManager.BuyBuilding(building.GetComponent<BuildingCost>());
                BuildingCost buildCreatedScript = buildCreated.GetComponent<BuildingCost>();
                if (buildCreatedScript.getTier() != 3 && buildCreated.tag != "StorageBuilding")
                {
                    if (buildCreated.tag == "Water")
                    {
                        gameManager.AddTreeLife(+buildCreated.GetComponent<BuildingCost>().getTier() * waterMultiplicator);
                    }
                    else
                    {
                        gameManager.AddTreeLife(-buildCreated.GetComponent<BuildingCost>().getTier() * polutionMultiplicator);
                    }
                }
                else
                {
                    
                }
                

                if (buildCreated.GetComponent<StartingConstruction>())
                {
                    gameManager.AddHeroBuilding(buildCreated.GetComponent<StartingConstruction>());
                    heroBuilding = buildCreated.GetComponent<StartingConstruction>();
                }
                else if (buildCreated.GetComponent<FoodStorageBuilding>())
                {
                    gameManager.AddFoodStorageBuilding(buildCreated.GetComponent<FoodStorageBuilding>());
                    storageBuildings.Add(buildCreated.GetComponent<StorageBuilding>());
                }else if (buildCreated.GetComponent<ResourceStorageBuilding>())
                {
                    gameManager.AddResourceStorageBuilding(buildCreated.GetComponent<ResourceStorageBuilding>());
                    storageBuildings.Add(buildCreated.GetComponent<StorageBuilding>());
                }else if (buildCreated.GetComponent<FoodBuilding>())
                {
                    foodBuildings.Add(buildCreated.GetComponent<FoodBuilding>());
                }

                if (!buildCreated.GetComponent<StartingConstruction>())
                {
                    gameManager.AddBuilding(buildCreated);
                }

                // If the sifht is down, continue 
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    Cursor.visible = true;
                    grid.setTilesActive(false);
                    shadow.SetActive(false);
                    T1HouseToPlace = null;
                    T2HouseToPlace = null;
                    T3HouseToPlace = null;
                    T1WaterToPlace = null;
                    T2WaterToPlace = null;
                    T3WaterToPlace = null;
                    T1FoodToPlace = null;
                    T2FoodToPlace = null;
                    T3FoodToPlace = null;
                    T1PowerToPlace = null;
                    T2PowerToPlace = null;
                    T3PowerToPlace = null;
                    stoneMineToPlace = null;
                    crystalMineToPlace = null;
                    B_FoodStorageToPlace = null;
                    B_ResourceStorageToPlace = null;
                }
            }
        }
        
    }

    /********************************************************************************************************************************/

    // Button event to create a T1House
    public void createT1House(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T1HouseToPlace = building;
            isDeleting = false;
            T1HouseShadow.SetActive(true);
        }
    }

    // Button event to create a T2House
    public void createT2House(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T2HouseToPlace = building;
            isDeleting = false;
            T2HouseShadow.SetActive(true);
        }
    }

    // Button event to create a T3House
    public void createT3House(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T3HouseToPlace = building;
            isDeleting = false;
            T3HouseShadow.SetActive(true);
        }
    }

    // Button event to create a T1Water
    public void createT1Water(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T1WaterToPlace = building;
            isDeleting = false;
            T1WaterShadow.SetActive(true);
        }
    }

    // Button event to create a T2Water
    public void createT2Water(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T2WaterToPlace = building;
            isDeleting = false;
            T2WaterShadow.SetActive(true);
        }
    }

    // Button event to create a T3Water
    public void createT3Water(GameObject building)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - building.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - building.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - building.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - building.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - building.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T3WaterToPlace = building;
            isDeleting = false;
            T3WaterShadow.SetActive(true);
        }
    }
    //button event to create T1Food
    public void createT1Food(GameObject farm)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - farm.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - farm.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - farm.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - farm.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - farm.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T1FoodToPlace = farm;
            isDeleting = false;
            T1FoodShadow.SetActive(true);
        }
        
    }

    //button event to create T2Food
    public void createT2Food(GameObject farm)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - farm.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - farm.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - farm.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - farm.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - farm.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T2FoodToPlace = farm;
            isDeleting = false;
            T2FoodShadow.SetActive(true);
        }

    }

    //button event to create T3Food
    public void createT3Food(GameObject farm)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - farm.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - farm.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - farm.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - farm.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - farm.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T3FoodToPlace = farm;
            isDeleting = false;
            T3FoodShadow.SetActive(true);
        }

    }

    //button event to create T1Power
    public void createT1Power(GameObject battery)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - battery.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - battery.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - battery.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - battery.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - battery.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T1PowerToPlace = battery;
            isDeleting = false;
            T1PowerShadow.SetActive(true);
        }
    }

    //button event to create T2Power
    public void createT2Power(GameObject battery)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - battery.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - battery.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - battery.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - battery.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - battery.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T2PowerToPlace = battery;
            isDeleting = false;
            T2PowerShadow.SetActive(true);
        }
    }

    //button event to create T3Power
    public void createT3Power(GameObject battery)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - battery.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - battery.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - battery.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - battery.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - battery.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            T3PowerToPlace = battery;
            isDeleting = false;
            T3PowerShadow.SetActive(true);
        }
    }

    //button event to create a Food Storage
    public void createFoodStorage(GameObject foodStorage)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - foodStorage.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - foodStorage.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - foodStorage.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - foodStorage.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - foodStorage.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            B_FoodStorageToPlace = foodStorage;
            isDeleting = false;
            B_FoodStoageShadow.SetActive(true);
        }
    }

    //button event to create a Resource Storage
    public void createResourceStorage(GameObject resourceStorage)
    {
        if (initialPlaced)
        {
            if (gameManager.GetTotalGold() - resourceStorage.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - resourceStorage.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - resourceStorage.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - resourceStorage.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - resourceStorage.GetComponent<BuildingCost>().CrystalCost < 0) return;

            grid.setTilesNearRoadActive(true);
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            B_ResourceStorageToPlace = resourceStorage;
            isDeleting = false;
            B_ResourceStorageShadow.SetActive(true);
        }
    }

    //button event to create a StoneMine
    public void createStoneMine(GameObject StoneMine)
    {
        if (initialPlaced && grid.isStoneAvailable())
        {
            if (gameManager.GetTotalGold() - StoneMine.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - StoneMine.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - StoneMine.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - StoneMine.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - StoneMine.GetComponent<BuildingCost>().CrystalCost < 0) return;

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
            if (gameManager.GetTotalGold() - CrystalMine.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - CrystalMine.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - CrystalMine.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - CrystalMine.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - CrystalMine.GetComponent<BuildingCost>().CrystalCost < 0) return;

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
            if (gameManager.GetTotalGold() - road.GetComponent<BuildingCost>().GoldCost < 0 ||
            gameManager.GetTotalFood() - road.GetComponent<BuildingCost>().FoodCost < 0 ||
            gameManager.GetTotalEnergy() - road.GetComponent<BuildingCost>().EnergyCost < 0 ||
            gameManager.GetTotalStone() - road.GetComponent<BuildingCost>().StoneCost < 0 ||
            gameManager.GetTotalCrystal() - road.GetComponent<BuildingCost>().CrystalCost < 0) return;

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