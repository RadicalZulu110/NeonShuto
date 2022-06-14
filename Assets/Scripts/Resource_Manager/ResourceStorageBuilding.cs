using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceStorageBuilding : StorageBuilding
{

    List<GameObject> stoneMiners, crystalMiners;
    public int maxStone, maxCrystal;
    [SerializeField]
    private int storedStone, storedCrystal;
    private float stonePercentage, crystalPercentage;
    public float noTruckDistance;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        stoneMiners = new List<GameObject>();
        crystalMiners = new List<GameObject>();
        gm.stoneCapacity += maxStone;
        gm.crystalCapacity += maxCrystal;
    }

    // Update is called once per frame
    void Update()
    {
        if (roadsToSpawn.Count == 0)
        {
            noRoadAccessIcon.SetActive(true);
        }
        else
        {
            noRoadAccessIcon.SetActive(false);
        }

        // If there is a truck available
        if (trucksAvailable.Count > 0)
        {
            // Get the buildings
            stoneMiners = gm.getStoneMiners();
            crystalMiners = gm.getCrystalMiners();
            CheckBuildings();

            if (stoneMiners.Count > 0 || crystalMiners.Count > 0)
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

    private void OnTriggerEnter(Collider other)
    {
        // Add the roads available to spawn 
        if (other.gameObject.tag == "Road")
        {
            roadsToSpawn.Add(other.gameObject);
        }
    }

    // Get the building with more reosurces in
    private GameObject getMaxResourceBuilding()
    {
        
        GameObject res = null;
        int actual = 0;

        // Check stone miners
        if (storedStone + truckStorage <= maxStone)
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
        if (storedCrystal + truckStorage <= maxCrystal)
        {
            for (int i = 0; i < crystalMiners.Count; i++)
            {
                if ((res == null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting() && crystalMiners[i].GetComponent<CrystalMiner>().GetNumberRoads() > 0) ||
                    (res != null && !crystalMiners[i].GetComponent<CrystalMiner>().isRecollecting() && crystalMiners[i].GetComponent<CrystalMiner>().GetCurrentCrystalStored() > actual) && crystalMiners[i].GetComponent<CrystalMiner>().GetNumberRoads() > 0)
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

    public int GetMaxStone()
    {
        return maxStone;
    }

    public int GetMaxCrystal()
    {
        return maxCrystal;
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

    private void CheckBuildings()
    {
        for (int i = 0; i < stoneMiners.Count; i++)
        {
            if (Vector3.Distance(this.gameObject.transform.position, stoneMiners[i].gameObject.transform.position) < noTruckDistance)
            {
                stoneMiners.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < crystalMiners.Count; i++)
        {
            if (Vector3.Distance(this.gameObject.transform.position, crystalMiners[i].gameObject.transform.position) < noTruckDistance)
            {
                crystalMiners.RemoveAt(i);
                i--;
            }
        }
    }
}
