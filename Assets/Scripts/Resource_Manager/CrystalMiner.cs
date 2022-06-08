using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMiner : MinerBuilding
{

    public int PersonalCrystalCapacity;
    public int currentCrystalStored;
    private List<GameObject> roadsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        //gm.crystalCapacity += PersonalCrystalCapacity;
        roadsToSpawn = new List<GameObject>();
        noRoadAccessIcon = transform.Find("NoRoadAccess").gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Debug.Log(roadsToSpawn.Count);

        if (roadsToSpawn.Count == 0)
        {
            noRoadAccessIcon.SetActive(true);
        }
        else
        {
            noRoadAccessIcon.SetActive(false);
        }

        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeBtwIncrease;
            currentCrystalStored += CrystalIncrease;

            gm.TotalGold -= MaintenanceGoldCost;
            gm.TotalEnergy -= MaintenanceEnergyCost;

            gm.AddCrystalPersonalCapacity(CrystalIncrease);
            currentCrystalStored += CrystalIncrease;

            if (currentCrystalStored > PersonalCrystalCapacity)
            {
                currentCrystalStored = PersonalCrystalCapacity;
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

    public override int GetCrystalIncrease()
    {
        return CrystalIncrease;
    }

    public override int GetCurrentCrystalStored()
    {
        return currentCrystalStored;
    }

    public override void addCrystal(int c)
    {
        currentCrystalStored += c;
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

    // Remove a road from the adyacent roads
    public void RemoveRoad(GameObject road)
    {
        for (int i = 0; i < roadsToSpawn.Count; i++)
        {
            if (roadsToSpawn[i] == road)
            {
                roadsToSpawn.RemoveAt(i);
                i--;
            }
        }
    }
}
