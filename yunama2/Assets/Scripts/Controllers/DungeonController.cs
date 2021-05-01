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

    GameObject chosenBlock;

    void Start() {
        initalizeBlockMap();
    }

    void Update()
    {
        
    }

    public Vector3 getEntrance() {
        return new Vector3((mapWidth+wallThickness*2)/2.0f-0.5f, -3.75f, 0);
    }

    public void destroyBlock(GameObject block) {
        if (isBlockDestroyable(block)) {
            BlockController blockController = block.GetComponent<BlockController>();
            int[] position = blockController.getPostition();
            position[1] = -position[1];

            Destroy(block);
            setBlockMap(position[0], position[1], 0);
        } else {
            // 消す
            Debug.Log("not destroyable");
            // 消す
        }
    }

    public bool isBlockEmpty(int[] position) {
        return isInsideWall(position[0], position[1]) && blockMap[position[1], position[0]] == 0;
    }

    bool isBlockDestroyable(GameObject block) {
        BlockController blockController = block.GetComponent<BlockController>();
        // block じゃないときは破壊しない
        if (blockController.name != "Block(Clone)") return false;

        int[] position = blockController.getPostition();
        // y は反転
        position[1] = -position[1];
        
        // 左右上下を確認
        int[] dx = {0, 0, 1, -1};
        int[] dy = {1, -1, 0, 0};
        bool hasEmptyNeighboringBlock = false;

        for(int i=0; i<4; i++){
            int nextX = position[0] + dx[i];
            int nextY = position[1] + dy[i];
            // 壁の内側にあるか確認
            if (!hasEmptyNeighboringBlock && isInsideWall(nextX, nextY)) {
                // 隣に空のブロックがある
                if (blockMap[nextY,nextX] == 0) hasEmptyNeighboringBlock = true;
            }
            Debug.Log(nextX);
            Debug.Log(nextX);
            Debug.Log(blockMap[nextY,nextX]);
            
        }
        return hasEmptyNeighboringBlock;
    }

    void initalizeBlockMap(){
        blockMap = new int[mapHeight+ wallThickness + 1, mapWidth + wallThickness*2 + 1];
        for(int i=1; i<=mapHeight; i++){
            for(int j=wallThickness; j<mapWidth+wallThickness; j++){
                blockMap[i,j] = 1;
            }
        }
        // 真ん中３つはあいてるから、０にする
        int middle = (mapWidth+wallThickness*2)/2;
        setBlockMap(middle, 1, 0); 
        setBlockMap(middle, 2, 0); 
        setBlockMap(middle, 3, 0);
        Debug.Log(middle); 
    }

    void setBlockMap(int x, int y, int setNum) {
        blockMap[y, x] = setNum;
    }

    bool isInsideWall(int x, int y) {
        return wallThickness < x && x < mapWidth+wallThickness && 1 <= y && y < mapHeight-1;
    }
}
