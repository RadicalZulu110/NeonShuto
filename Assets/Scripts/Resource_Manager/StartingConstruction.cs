using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class StartingConstruction : BuildingCost
{
    public int MaxPop;
    public int MinPop;
    public int ExpectedPop;
    private int nextPopIncreaseTime;
    public int timeBtwPopIncrease;
    public int maxFood, maxStone, maxCrystal;
    [SerializeField]
    private int storedFood, storedStone, storedCrystal;
    [SerializeField]
    private float foodPercentage, stonePercentage, crystalPercentage;

    public int maxTrucks, nTrucks;
    public GameObject truckPrefab;
    List<GameObject> trucksAvailable, trucksNoAvailable;
    List<GameObject> farms, stoneMiners, crystalMiners;
    private List<GameObject> roadsToSpawn;

    private GameObject currentBuilding, currentTruck, roadToSpawn;
    private int truckStorage;

    private void Start()
    {
        trucksAvailable = new List<GameObject>();
        trucksNoAvailable = new List<GameObject>();
        farms = new List<GameObject>();
        stoneMiners = new List<GameObject>();
        crystalMiners = new List<GameObject>();
        roadsToSpawn = new List<GameObject>();
        truckStorage = truckPrefab.GetComponent<Truck>().getMaxCapacity();
        gm.foodCapacity += maxFood;
        gm.stoneCapacity += maxStone;
        gm.crystalCapacity += maxCrystal;

        for(int i=0; i<maxTrucks; i++)
        {
            GameObject truck = Instantiate(truckPrefab, this.transform.position, Quaternion.identity);
            truck.SetActive(false);
            truck.GetComponent<Truck>().setStorageBuilding(this.gameObject);
            trucksAvailable.Add(truck);
        }

        noRoadAccessIcon = transform.Find("NoRoadAccess").gameObject;
    }

    void Update()
    {
        if (Time.time > nextPopIncreaseTime)
        {
            nextPopIncreaseTime = (int)(Time.time + timeBtwPopIncrease);
            ExpectedPop = Random.Range(MinPop, MaxPop);

            gm.AddTotalPop(ExpectedPop);
            gm.goldIncome = gm.TotalPop * GoldIncreasePerPerson;
            gm.TotalGold += gm.goldIncome;
        }


        if(roadsToSpawn.Count == 0)
        {
            noRoadAccessIcon.SetActive(true);
        }
        else
        {
            noRoadAccessIcon.SetActive(false);
            // If there is a truck available
            if (trucksAvailable.Count > 0)
            {
                // Get the buildings
                farms = gm.getFarms();
                stoneMiners = gm.getStoneMiners();
                crystalMiners = gm.getCrystalMiners();

                if (farms.Count > 0 || stoneMiners.Count > 0 || crystalMiners.Count > 0)
                {
                    // Get the building with more rosources in
                    currentBuilding = getMaxResourceBuilding();

                    if (currentBuilding != null)
                    {
                        currentTruck = trucksAvailable[0];
                        trucksAvailable.Remove(currentTruck);
                        trucksNoAvailable.Add(currentTruck);
                        currentTruck.transform.position = roadToSpawn.transform.position;
                        currentTruck.SetActive(true);
                        currentTruck.GetComponent<Truck>().setDestination(currentBuilding.transform.position);

                        // If it is a farm
                        if (currentBuilding.GetComponent<FoodBuilding>())
                        {
                            currentBuilding.GetComponent<FoodBuilding>().setRecollecting(true);
                            currentBuilding.GetComponent<FoodBuilding>().setTruckRecollecting(currentTruck);
                        }
                        else if (currentBuilding.GetComponent<StoneMiner>())     // if it is a stone miner
                        {
                            currentBuilding.GetComponent<StoneMiner>().setRecollecting(true);
                            currentBuilding.GetComponent<StoneMiner>().setTruckRecollecting(currentTruck);
                        }
                        else if (currentBuilding.GetComponent<CrystalMiner>())     // If it is a crystal miner
                        {
                            currentBuilding.GetComponent<CrystalMiner>().setRecollecting(true);
                            currentBuilding.GetComponent<CrystalMiner>().setTruckRecollecting(currentTruck);
                        }
                    }
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the roads available to spawn 
        if(other.gameObject.tag == "Road")
        {
            roadsToSpawn.Add(other.gameObject);
        }
    }

    // Check if all the roads to spawn are not null. If null, delete it
    public void CheckAdyacentRoads()
    {
        for(int i=0; i<roadsToSpawn.Count; i++)
        {
            if(roadsToSpawn[i] == null)
            {
                roadsToSpawn.RemoveAt(i);
                i--;
            }
        }
    }

    // Remove a road from the adyacent roads
    public void RemoveRoad(GameObject road)
    {
        for(int i = 0; i < roadsToSpawn.Count; i++)
        {
            if (roadsToSpawn[i] == road)
            {
                roadsToSpawn.RemoveAt(i);
                break;
            }
        }
    }


    public int GetExpectedPop()
    {
        return ExpectedPop;
    }

    

    // Get the building with more reosurces in
    private GameObject getMaxResourceBuilding()
    {
        if(farms.Count == 0 && stoneMiners.Count == 0 && crystalMiners.Count == 0)
        {
            return null;
        }

        GameObject res = null;
        int actual = 0;

        /*if (farms.Count > 0 && !farms[0].GetComponent<FoodBuilding>().isRecollecting())
        {
            res = farms[0];
            actual = farms[0].GetComponent<FoodBuilding>().GetCurrentFoodStored();
        }
        else
        {
            res = null;
        }*/

        // Check farms
        if(storedFood + truckStorage <= maxFood)
        {
            for (int i = 0; i < farms.Count; i++)
            {
                if ((res == null && !farms[i].GetComponent<FoodBuilding>().isRecollecting() && farms[i].GetComponent<FoodBuilding>().GetNumberRoads() > 0) ||
                    (res != null && !farms[i].GetComponent<FoodBuilding>().isRecollecting() && farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored() > actual && farms[i].GetComponent<FoodBuilding>().GetNumberRoads() > 0))
                {
                    NavMeshPath path = new NavMeshPath();
                    for(int j=0; j<roadsToSpawn.Count; j++)
                    {
                        if(roadsToSpawn[j] != null)
                        {
                            NavMesh.CalculatePath(roadsToSpawn[j].transform.position, farms[i].GetComponent<FoodBuilding>().getNearestRoad().transform.position, NavMesh.AllAreas, path);
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                res = farms[i];
                                actual = farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored();
                                roadToSpawn = roadsToSpawn[j];
                                break;
                            }
                        }
                        
                    }
                }
            }
        }

        // Check stone miners
        if(storedStone + truckStorage <= maxStone)
        {
            for (int i = 0; i < stoneMiners.Count; i++)
            {
                if ((res == null && !stoneMiners[i].GetComponent<StoneMiner>().isRecollecting() && stoneMiners[i].GetComponent<StoneMiner>().GetNumberRoads() > 0) ||
                    (res != null && !stoneMiners[i].GetComponent<StoneMiner>().isRecollecting() && stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored() > actual && stoneMiners[i].GetComponent<StoneMiner>().GetNumberRoads() > 0))
                {
                    NavMeshPath path = new NavMeshPath();
                    for (int j = 0; j < roadsToSpawn.Count; j++)
                    {
                        if(roadsToSpawn[j] != null)
                        {
                            NavMesh.CalculatePath(roadsToSpawn[j].transform.position, stoneMiners[i].GetComponent<StoneMiner>().getNearestRoad().transform.position, NavMesh.AllAreas, path);
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                res = stoneMiners[i];
                                actual = stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored();
                                roadToSpawn = roadsToSpawn[j];
                                break;
                            }
                        }
                        
                    }
                }
            }
        }

        // Check crystal miners
        if(storedCrystal + truckStorage <= maxCrystal)
        {
            for (int i = 0; i < crystalMiners.Count; i++)
            {
                if ((res == null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting() && crystalMiners[i].GetComponent<CrystalMiner>().GetNumberRoads() > 0) ||
                    (res != null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting() && crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored() > actual && crystalMiners[i].GetComponent<CrystalMiner>().GetNumberRoads() > 0))
                {
                    NavMeshPath path = new NavMeshPath();
                    for (int j = 0; j < roadsToSpawn.Count; j++)
                    {
                        if(roadsToSpawn[j] != null)
                        {
                            NavMesh.CalculatePath(roadsToSpawn[j].transform.position, crystalMiners[i].GetComponent<CrystalMiner>().getNearestRoad().transform.position, NavMesh.AllAreas, path);
                            if (path.status == NavMeshPathStatus.PathComplete)
                            {
                                res = crystalMiners[i];
                                actual = crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored();
                                roadToSpawn = roadsToSpawn[j];
                                break;
                            }
                        }
                        
                    }
                }
            }
        }
        

        return res;
    }

    // Get the nearest road to the hero building
    private GameObject getNearestRoad()
    {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        GameObject res = roads[0];

        for(int i=1; i<roads.Length; i++)
        {
            if (Vector3.Distance(roads[i].transform.position, this.transform.position) < Vector3.Distance(res.transform.position, this.transform.position))
                res = roads[i];
        }

        return res;
    }

    // Recieve a truck to move it to the unavailable to availables trucks
    public void makeAvailableTruck(GameObject truck)
    {
        trucksNoAvailable.Remove(truck);
        trucksAvailable.Add(truck);
    }

    public void addFood(int f)
    {
        storedFood += f;
        foodPercentage = (storedFood * 100) / maxFood;
    }

    public void addStone(int s)
    {
        storedStone += s;
        stonePercentage = (storedStone * 100) / maxStone;
    }

    public void addCrystal(int c)
    {
        storedCrystal += c;
        crystalPercentage = (storedCrystal * 100) / maxCrystal;
    }

    public int GetFoodStored()
    {
        return storedFood;
    }

    public int GetStoneStored()
    {
        return storedStone;
    }

    public int GetCrystalStored()
    {
        return storedCrystal;
    }

    public float GetFoodPercentage()
    {
        return foodPercentage;
    }

    public float GetStonePercentage()
    {
        return stonePercentage;
    }

    public float GetCrystalPercentage()
    {
        return crystalPercentage;
    }
}
