using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int mapWidth = 59;
    public int mapHeight = 45;
    public int wallThickness = 5;

    // ex. 4 = 40% , 10 = 100%
    public int randomEmptyRange = 7;
    public int randomEnergyAmountRange = 15;
    public int randomMagicAmountRange = 15;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    public int[,] blockMap;

    void Start() {
        initalizeBlockMap();
    }

    void Update()
    {
        
    }

    public Vector3 getEntrance() {
        return new Vector3((mapWidth+wallThickness*2)/2.0f-0.5f, -3.75f, 0);
    }

    void initalizeBlockMap(){
        blockMap = new int[mapHeight, mapWidth];
        for(int i=0; i<mapHeight; i++){
            for(int j=0; j<mapWidth; j++){
                blockMap[i,j] = 1;
            }
        }
        // 真ん中３つはあいてるから、０にする
        blockMap[0,(mapWidth+wallThickness*2)/2] = 0;
        blockMap[1,(mapWidth+wallThickness*2)/2] = 0;
        blockMap[2,(mapWidth+wallThickness*2)/2] = 0;
    }
}
