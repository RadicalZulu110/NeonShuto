using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Truck : MonoBehaviour
{

    public NavMeshAgent agent;
    public int capacity, maxCapacity;
    private GameObject  storageBuilding;
    private bool comingBack, food, stone, crystal;
    private GameManager gameManager;
    public float lastDistance;
    public float nextIncreaseTime; 
    public float timeBtwIncrease;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        comingBack = false;
        capacity = 0;
        lastDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (storageBuilding == null)
            MakeAvailable();


        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            // If it is not moving, reset the truck
            if (this.gameObject.activeInHierarchy && lastDistance == GetPathRemainingDistance())
            {
                MakeAvailable();
            }

            if (this.gameObject.activeInHierarchy)
                lastDistance = GetPathRemainingDistance();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If it colides with a sphere collider
        if(other is SphereCollider)
        {
            // If it is a production building
            if (other.gameObject.GetComponent<ProductionBuilding>())
            {
                ProductionBuilding otherScript = other.gameObject.GetComponent<ProductionBuilding>();
                
                if (otherScript.getTruckRecollecting() == this.gameObject)
                {
                    // If it is a farm
                    if (other.gameObject.GetComponent<FoodBuilding>())
                    {
                        otherScript = other.gameObject.GetComponent<FoodBuilding>();
                        food = true;
                        capacity = otherScript.GetCurrentFoodStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addFood(-capacity);
                    }
                    else if (other.gameObject.GetComponent<StoneMiner>())  // If it is a stone miner
                    {
                        otherScript = other.gameObject.GetComponent<StoneMiner>();
                        stone = true;
                        capacity = otherScript.GetCurrentStoneStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addStone(-capacity);
                    }
                    else if (other.gameObject.GetComponent<CrystalMiner>())  // If it is a crystal miner
                    {
                        otherScript = other.gameObject.GetComponent<CrystalMiner>();
                        crystal = true;
                        capacity = otherScript.GetCurrentCrystalStored();
                        if (capacity > maxCapacity)
                            capacity = maxCapacity;
                        otherScript.addCrystal(-capacity);
                    }


                    agent.SetDestination(storageBuilding.transform.position);
                    otherScript.setTruckRecollecting(null);
                    otherScript.setRecollecting(false);
                    comingBack = true;
                }
            }
            else
            {
                // If it is the original storage building
                if(other.gameObject == storageBuilding && comingBack)
                {

                    if (storageBuilding.GetComponent<StartingConstruction>())
                    {
                        storageBuilding.GetComponent<StartingConstruction>().makeAvailableTruck(this.gameObject);
                        if (food)
                        {
                            other.gameObject.GetComponent<StartingConstruction>().addFood(capacity);
                        }
                        else if (stone)
                        {
                            other.gameObject.GetComponent<StartingConstruction>().addStone(capacity);
                        }
                        else if (crystal)
                        {
                            other.gameObject.GetComponent<StartingConstruction>().addCrystal(capacity);
                        }
                    }
                    else if (storageBuilding.GetComponent<StorageBuilding>())
                    {
                        storageBuilding.GetComponent<StorageBuilding>().makeAvailableTruck(this.gameObject);
                        if (food)
                        {
                            other.gameObject.GetComponent<FoodStorageBuilding>().addFood(capacity);
                        }
                        else if (stone)
                        {
                            other.gameObject.GetComponent<ResourceStorageBuilding>().addStone(capacity);
                        }
                        else if (crystal)
                        {
                            other.gameObject.GetComponent<ResourceStorageBuilding>().addCrystal(capacity);
                        }
                    }

                    if (food)
                    {
                        food = false;
                        //gameManager.addTotalFood(capacity);
                    }else if (stone)
                    {
                        stone = false;
                        //gameManager.addTotalStone(capacity);
                    }else if (crystal)
                    {
                        crystal = false;
                        //gameManager.addTotalCrystal(capacity);
                    }

                    this.gameObject.SetActive(false);
                    comingBack = false;
                    capacity = 0;                
                }
            }
        }
    }

    public void setDestination(Vector3 goal)
    {
        agent.SetDestination(goal);
    }

    public GameObject getStorageBuilding()
    {
        return storageBuilding;
    }

    public void setStorageBuilding(GameObject sb)
    {
        storageBuilding = sb;
    }

    public int getCapacity()
    {
        return capacity;
    }

    public int getMaxCapacity()
    {
        return maxCapacity;
    }

    public void MakeAvailable()
    {
        if (storageBuilding && storageBuilding.GetComponent<StartingConstruction>())
        {
            storageBuilding.GetComponent<StartingConstruction>().makeAvailableTruck(this.gameObject);
        }
        else if (storageBuilding && storageBuilding.GetComponent<StorageBuilding>())
        {
            storageBuilding.GetComponent<StorageBuilding>().makeAvailableTruck(this.gameObject);
        }

        if (food)
        {
            food = false;
        }
        else if (stone)
        {
            stone = false;
        }
        else if (crystal)
        {
            crystal = false;
        }


        this.gameObject.SetActive(false);
        comingBack = false;
        capacity = 0;
    }

    private float GetPathRemainingDistance()
    {
        if (agent.pathPending ||
            agent.pathStatus == NavMeshPathStatus.PathInvalid ||
            agent.path.corners.Length == 0)
            return -1f;

        float distance = 0.0f;
        for (int i = 0; i < agent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }

        return distance;
    }
}
