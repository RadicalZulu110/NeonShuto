using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public int CrystalNodeAmount;
    public int StoneNodeAmount;
    private bool isCrystal, isStone;

    private void Start()
    {
        if (CrystalNodeAmount > 0)
            isCrystal = true;
        else
            isStone = true;
    }

    private void Update()
    {
        if (CrystalNodeAmount <= 0 && StoneNodeAmount <= 0)
        {
            if (isCrystal)
                GameObject.FindObjectOfType<Spawner1>().createCrystals(1);
            else
                GameObject.FindObjectOfType<Spawner1>().createStones(1);
            Destroy(this.gameObject);
        }
            
    }
}
