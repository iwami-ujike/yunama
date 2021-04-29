using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public class Block 
    {
        public int energyAmount;
        public int magicAmount;

        public Block(int energy, int magic) 
        {
            energyAmount = energy;
            magicAmount = magic;
        }
    }


     
    void Start()
    {

    }


    void Update()
    {
        
    }


    public int getLevel() 
    {
        // まだ作業中
        return 0;
    }
}