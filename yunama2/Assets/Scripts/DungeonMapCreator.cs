using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapCreator : MonoBehaviour
{
    public int randomEmptyRange = 4;
    public int randomEnergyAmountRange = 15;
    public int randomMagicAmountRange = 15;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    // public Block block = createBlock();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    bool emptyBlock() 
    {
        return Random.Range(0, randomEmptyRange) == 0;
    }
}