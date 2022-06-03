using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : BuildingCost 
{

    public int maxTrucks, nTrucks;
    public GameObject truckPrefab;
    public List<GameObject> trucksAvailable, trucksNoAvailable;
    protected GameObject currentBuilding, currentTruck, roadToSpawn;
    protected int truckStorage;
    protected List<GameObject> roadsToSpawn;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        trucksAvailable = new List<GameObject>();
        trucksNoAvailable = new List<GameObject>();
        roadsToSpawn = new List<GameObject>();
        truckStorage = truckPrefab.GetComponent<Truck>().getMaxCapacity();

        for (int i = 0; i < maxTrucks; i++)
        {
            GameObject truck = Instantiate(truckPrefab, this.transform.position, Quaternion.identity);
            truck.SetActive(false);
            truck.GetComponent<Truck>().setStorageBuilding(this.gameObject);
            trucksAvailable.Add(truck);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            gm.AddTreeLife(-T3TreeLife);
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

    // Check if all the roads to spawn are not null. If null, delete it
    public void CheckAdyacentRoads()
    {
        for (int i = 0; i < roadsToSpawn.Count; i++)
        {
            if (roadsToSpawn[i] == null)
            {
                roadsToSpawn.RemoveAt(i);
                i--;
            }
        }
    }

    // Get the nearest road to the hero building
    public GameObject getNearestRoad()
    {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        GameObject res = roads[0];

        for (int i = 1; i < roads.Length; i++)
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
