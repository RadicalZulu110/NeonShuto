using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodStorageBuilding : StorageBuilding
{

    List<GameObject> farms;
    public int maxFood;
    [SerializeField]
    private int storedFood;
    [SerializeField]
    private float foodPercentage;
    public float noTruckDistance;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        farms = new List<GameObject>();
        gm.foodCapacity += maxFood;
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a truck available
        if (trucksAvailable.Count > 0)
        {
            // Get the farms
            farms = gm.getFarms();
            CheckBuildings();

            if (farms.Count > 0)
            {
                // Get the building with more rosources in
                currentBuilding = getMaxFarm();

                if (currentBuilding != null)
                {
                    currentTruck = trucksAvailable[0];
                    trucksAvailable.Remove(currentTruck);
                    trucksNoAvailable.Add(currentTruck);
                    currentTruck.transform.position = roadToSpawn.transform.position;
                    currentTruck.SetActive(true);
                    currentTruck.GetComponent<Truck>().setDestination(currentBuilding.transform.position);
                    
                    currentBuilding.GetComponent<FoodBuilding>().setRecollecting(true);
                    currentBuilding.GetComponent<FoodBuilding>().setTruckRecollecting(currentTruck);
                }
            }

        }
    }

    // Get the farm with more food in the storage
    private GameObject getMaxFarm()
    {
        GameObject res = null;
        int actual = 0;

        if(!(storedFood + truckStorage <= maxFood))
        {
            return null;
        }

        if (storedFood + truckStorage <= maxFood)
        {
            for (int i = 0; i < farms.Count; i++)
            {
                if ((res == null && !farms[i].GetComponent<FoodBuilding>().isRecollecting() && farms[i].GetComponent<FoodBuilding>().GetNumberRoads() > 0) ||
                    (res != null && !farms[i].GetComponent<FoodBuilding>().isRecollecting() && farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored() > actual && farms[i].GetComponent<FoodBuilding>().GetNumberRoads() > 0))
                {
                    NavMeshPath path = new NavMeshPath();
                    for (int j = 0; j < roadsToSpawn.Count; j++)
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

        return res;
    }

    public void addFood(int f)
    {
        storedFood += f;
        foodPercentage = (storedFood * 100) / maxFood;
    }

    public int GetMaxFood()
    {
        return maxFood;
    }

    public int GetFoodStored()
    {
        return storedFood;
    }

    public float GetFoodPercentage()
    {
        return foodPercentage;
    }

    private void CheckBuildings()
    {
        for (int i = 0; i < farms.Count; i++)
        {
            if (Vector3.Distance(this.gameObject.transform.position, farms[i].gameObject.transform.position) < noTruckDistance)
            {
                farms.RemoveAt(i);
                i--;
            }
        }
    }
}
