using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : BuildingCost 
{

    public int maxTrucks, nTrucks;
    public GameObject truckPrefab;
    public List<GameObject> trucksAvailable, trucksNoAvailable;
    public GameObject currentBuilding, currentTruck;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        trucksAvailable = new List<GameObject>();
        trucksNoAvailable = new List<GameObject>();

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
