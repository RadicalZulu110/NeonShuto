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
    List<GameObject> farms;

    private GameObject currentFarm, currentTruck;

    private void Start()
    {
        trucksAvailable = new List<GameObject>();
        trucksNoAvailable = new List<GameObject>();
        farms = new List<GameObject>();

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

        if(trucksAvailable.Count > 0)
        {
            farms = gm.getFarms();
            if(farms.Count > 0)
            {
                currentFarm = getMaxFarm();
                if (currentFarm != null)
                {
                    currentTruck = trucksAvailable[0];
                    trucksAvailable.Remove(currentTruck);
                    trucksNoAvailable.Add(currentTruck);
                    currentTruck.transform.position = getNearestRoad().transform.position;
                    currentTruck.SetActive(true);
                    currentTruck.GetComponent<Truck>().setDestination(currentFarm.transform.position);
                    currentFarm.GetComponent<FoodBuilding>().setRecollecting(true);
                    currentFarm.GetComponent<FoodBuilding>().setTruckRecollecting(currentTruck);
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
