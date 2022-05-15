using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStorageBuilding : StorageBuilding
{

    List<GameObject> farms;
    public int maxFood;
    [SerializeField]
    private int storedFood;
    [SerializeField]
    private float foodPercentage;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        farms = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a truck available
        if (trucksAvailable.Count > 0)
        {
            // Get the farms
            farms = gm.getFarms();
            if (farms.Count > 0)
            {
                // Get the building with more rosources in
                currentBuilding = getMaxFarm();

                if (currentBuilding != null)
                {
                    currentTruck = trucksAvailable[0];
                    trucksAvailable.Remove(currentTruck);
                    trucksNoAvailable.Add(currentTruck);
                    currentTruck.transform.position = getNearestRoad().transform.position;
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
        GameObject res;

        if(!(storedFood + truckStorage <= maxFood))
        {
            return null;
        }

        if (!farms[0].GetComponent<FoodBuilding>().isRecollecting())
        {
            res = farms[0];
        }
        else
        {
            res = null;
        }

        for (int i = 1; i < farms.Count; i++)
        {
            if ((res == null && !farms[i].GetComponent<FoodBuilding>().isRecollecting()) ||
                (res != null && farms[i].GetComponent<FoodBuilding>().GetCurrentFoodStored() > res.GetComponent<FoodBuilding>().GetCurrentFoodStored() && !farms[i].GetComponent<FoodBuilding>().isRecollecting()))
            {
                res = farms[i];
            }
        }

        return res;
    }

    public void addFood(int f)
    {
        storedFood += f;
        foodPercentage = (storedFood * 100) / maxFood;
    }

    public int GetFoodStored()
    {
        return storedFood;
    }

    public float GetFoodPercentage()
    {
        return foodPercentage;
    }
}
