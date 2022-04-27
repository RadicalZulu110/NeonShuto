using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StartingConstruction : BuildingCost
{
    public int MaxPop;
    public int MinPop;
    public int ExpectedPop;
    private int nextPopIncreaseTime;
    public int timeBtwPopIncrease;
    public int maxTrucks, nTrucks;
    public GameObject truckPrefab;
    List<GameObject> trucksAvailable, trucksNoAvailable;
    List<GameObject> farms, stoneMiners, crystalMiners;

    private GameObject currentBuilding, currentTruck;

    private void Start()
    {
        trucksAvailable = new List<GameObject>();
        trucksNoAvailable = new List<GameObject>();
        farms = new List<GameObject>();
        stoneMiners = new List<GameObject>();
        crystalMiners = new List<GameObject>();

        for(int i=0; i<maxTrucks; i++)
        {
            GameObject truck = Instantiate(truckPrefab, this.transform.position, Quaternion.identity);
            truck.SetActive(false);
            trucksAvailable.Add(truck);
        }
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

        // If there is a truck available
        if(trucksAvailable.Count > 0)
        {
            // Get the buildings
            farms = gm.getFarms();
            stoneMiners = gm.getStoneMiners();
            crystalMiners = gm.getCrystalMiners();
            if(farms.Count > 0 || stoneMiners.Count > 0 || crystalMiners.Count > 0)
            {
                // Get the building with more rosources in
                currentBuilding = getMaxResourceBuilding();

                if (currentBuilding != null)
                {
                    currentTruck = trucksAvailable[0];
                    trucksAvailable.Remove(currentTruck);
                    trucksNoAvailable.Add(currentTruck);
                    currentTruck.transform.position = getNearestRoad().transform.position;
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
                    }else if (currentBuilding.GetComponent<CrystalMiner>())     // If it is a crystal miner
                    {
                        currentBuilding.GetComponent<CrystalMiner>().setRecollecting(true);
                        currentBuilding.GetComponent<CrystalMiner>().setTruckRecollecting(currentTruck);
                    }
                }
            }
            
        }
    }

    public int GetExpectedPop()
    {
        return ExpectedPop;
    }

    // Get the farm with more food in the storage
    private GameObject getMaxFarm()
    {
        GameObject res;

        if (!farms[0].GetComponent<FoodBuilding>().isRecollecting())
        {
            res = farms[0];
        }
        else
        {
            res = null;
        }

        for (int i=1; i<farms.Count; i++)
        {
            if((res == null && !farms[i].GetComponent<FoodBuilding>().isRecollecting()) ||
                (res != null && farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored() > res.GetComponent<FoodBuilding>().GetCurrentFoodStored() && !farms[i].GetComponent<FoodBuilding>().isRecollecting()))
            {
                res = farms[i];
            }
        }

        return res;
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
        for (int i = 0; i < farms.Count; i++)
        {
            if ((res == null && !farms[i].GetComponent<FoodBuilding>().isRecollecting()) ||
                (res != null && farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored() > actual))
            {
                res = farms[i];
                actual = farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored();
            }
        }

        // Check stone miners
        for (int i = 0; i < stoneMiners.Count; i++)
        {
            if ((res == null && !stoneMiners[i].GetComponent<StoneMiner>().isRecollecting()) ||
                (res != null && stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored() > actual))
            {
                res = stoneMiners[i];
                actual = stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored();
            }
        }

        // Check crystal miners
        for (int i = 0; i < crystalMiners.Count; i++)
        {
            if ((res == null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting()) ||
                (res != null && crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored() > actual))
            {
                res = crystalMiners[i];
                actual = crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored();
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
}
