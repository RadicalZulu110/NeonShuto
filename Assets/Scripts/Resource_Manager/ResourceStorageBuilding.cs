using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorageBuilding : StorageBuilding
{

    List<GameObject> stoneMiners, crystalMiners;
    public int maxStone, maxCrystal;
    [SerializeField]
    private int storedStone, storedCrystal;
    private float stonePercentage, crystalPercentage;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        stoneMiners = new List<GameObject>();
        crystalMiners = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a truck available
        if (trucksAvailable.Count > 0)
        {
            // Get the buildings
            stoneMiners = gm.getStoneMiners();
            crystalMiners = gm.getCrystalMiners();
            if (stoneMiners.Count > 0 || crystalMiners.Count > 0)
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
                    
                    if (currentBuilding.GetComponent<StoneMiner>())     // if it is a stone miner
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

    // Get the building with more reosurces in
    private GameObject getMaxResourceBuilding()
    {
        
        GameObject res = null;
        int actual = 0;

        // Check stone miners
        if(storedStone + truckStorage <= maxStone)
        {
            for (int i = 0; i < stoneMiners.Count; i++)
            {
                if ((res == null && !stoneMiners[i].GetComponent<StoneMiner>().isRecollecting()) ||
                    (res != null && stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored() > actual))
                {
                    res = stoneMiners[i];
                    actual = stoneMiners[i].GetComponent<StoneMiner>().GetCurrentStoneStored();
                }
            }
        }

        // Check crystal miners
        if(storedCrystal + truckStorage <= maxCrystal)
        {
            for (int i = 0; i < crystalMiners.Count; i++)
            {
                if ((res == null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting()) ||
                    (res != null && crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored() > actual))
                {
                    res = crystalMiners[i];
                    actual = crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored();
                }
            }
        }
        

        return res;
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

    public int GetStoneStored()
    {
        return storedStone;
    }

    public int GetCrystalStored()
    {
        return storedCrystal;
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
