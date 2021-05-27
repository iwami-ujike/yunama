using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonController : MonoBehaviour
{
    public GameObject energyPrefab;
    public GameObject magicPrefab;
    public GameObject medamaPrefab;
    public GameObject obakePrefab;
    public GameObject deathPrefab;
    public GameObject dragonPrefab;
    public GameObject mithrilPrefab;
    public GameObject pegasusPrefab;
    public GameObject pogsusPrefab;
    public GameObject pugsusPrefab;
    public GameObject pigsusPrefab;
    public GameObject enemyPrefab;

    public GameObject details_object = null;

    public int mapWidth = 59;
    public int mapHeight = 45;
    public int wallThickness = 5;

    // ex. 4 = 40% , 10 = 100%
    public int randomEmptyRange = 7;
    public int randomEnergyAmountRange = 10;
    public int randomMagicAmountRange = 10;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    public int[,] blockMap;

    GameObject chosenBlock;

    void Start() {
        initalizeBlockMap();
    }

    void Update() {
        
    }

    public Vector3 getEntrance() {
        return new Vector3((mapWidth+wallThickness*2)/2.0f, 0, 0);
    }

    public void destroyBlock(GameObject block) {
        if (isBlockDestroyable(block)) {
            BlockController blockController = block.GetComponent<BlockController>();
            int[] position = blockController.getPostition(); 
            position[1] = -position[1];

            int blockEnergyAmount = blockController.energyAmount;
            int blockMagicAmount = blockController.magicAmount;
            int blockLevel = blockController.getLevel();

            bool createCreature = blockLevel > 0;

            Destroy(block);
            setBlockMap(position[0], position[1], 0);

            if (createCreature) {
                bool isEnergyType = blockEnergyAmount >= blockMagicAmount;
                bool isAlone = !aroundBlock(position[0], position[1]);
                int carryingAmount = Mathf.Max(blockEnergyAmount, blockMagicAmount);
                GameObject newCreature = enemyPrefab;
                if (isEnergyType) {
                    if (blockLevel == 1) {
                        newCreature = Instantiate(energyPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 2) {
                        newCreature = Instantiate(medamaPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3 && isAlone && blockEnergyAmount >= 30) {
                        newCreature = Instantiate(pogsusPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3 && isAlone) {
                        newCreature = Instantiate(pegasusPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3) {
                        newCreature = Instantiate(mithrilPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    }
                } else {
                    if (blockLevel == 1) {
                        newCreature = Instantiate(magicPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 2) {
                        newCreature = Instantiate(obakePrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3 && isAlone && blockMagicAmount >= 30) {
                        newCreature = Instantiate(pigsusPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3 && isAlone) {
                        newCreature = Instantiate(deathPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    } else if (blockLevel == 3) {
                        newCreature = Instantiate(dragonPrefab, new Vector3(position[0]+0.5f,-position[1]+0.5f,0), Quaternion.identity);
                    }
                }
                CreatureController creatureController = newCreature.GetComponent<CreatureController>();
                creatureController.setCreatureStatus(blockLevel, carryingAmount, isEnergyType, blockEnergyAmount, blockMagicAmount, isAlone);
            }
        } else {
            // 消す
            Debug.Log("not destroyable");
            // 消す
        }
    }

    public bool aroundBlock (int x, int y) {
        int[] z = new int[] {x-1, x, x+1};
        int[] c = new int[] {y-1, y, y+1};
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (i == 1 && j == 1) { 
                } else if (GameObject.Find("Block_" + z[i] + "_" + c[j])) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isBlockEmpty(int[] position) {
        return isInsideWall(position[0], position[1]) && blockMap[position[1], position[0]] == 0;
    }

    bool isBlockDestroyable(GameObject block) {
        // block じゃないときは破壊しない
        if (block.tag != "Block") return false;
        
        BlockController blockController = block.GetComponent<BlockController>();

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
    }

    void setBlockMap(int x, int y, int setNum) {
        blockMap[y, x] = setNum;
    }

    public bool isInsideWall(int x, int y) {
        return wallThickness < x && x < mapWidth+wallThickness && 1 <= y && y < mapHeight-1;
    }

    public void objectDetailsText(GameObject gameObject) {
        string text = "";
        int eAmount = 0;
        int mAmount = 0;
        Text details_text = details_object.GetComponent<Text>();

        if (gameObject.tag == "Block") {
            BlockController blockController = gameObject.GetComponent<BlockController>();
            eAmount = blockController.energyAmount;
            mAmount = blockController.magicAmount;
            text = "Energy: " + eAmount + "\n" + "Magic:  " + mAmount;
            details_text.text = text;
        } else if (gameObject.tag == "Creature") {
            CreatureController creatureController = gameObject.GetComponent<CreatureController>();
            int healthPoint = creatureController.currentHP;
            eAmount = creatureController.getCarryingAmount();
            text = "Name: " + gameObject.name + "\nHealth: " + healthPoint + "\nEnergy: " + eAmount + "\n" + "Magic:  " + mAmount;
            details_text.text = text;
        }

    }

    public void objectKillTheCreature(GameObject gameObject) {
        CreatureController creatureController = gameObject.GetComponent<CreatureController>();
        int damage = Mathf.CeilToInt(creatureController.healthPoint * 0.3f);
        creatureController.gotDamaged(damage);
    }

    public void scatterEM (int x, int y, int energyAmount, int magicAmount) {
        List<BlockController> blockLists = new List<BlockController>();
        int[] z = new int[] {   
            x, x-1, x+1, 
            x-1, x+1, 
            x, x-1, x+1, 
            x, x-1, x+1, x-2, x+2, 
            x-2, x+2, 
            x-2, x+2, 
            x-2, x+2, 
            x
            };
        int[] c = new int[] {   
            Mathf.Abs(y-2), Mathf.Abs(y-2), Mathf.Abs(y-2), 
            Mathf.Abs(y-1), Mathf.Abs(y-1), 
            Mathf.Abs(y), Mathf.Abs(y), Mathf.Abs(y), 
            Mathf.Abs(y-3), Mathf.Abs(y-3), Mathf.Abs(y-3), Mathf.Abs(y-3), Mathf.Abs(y-3), 
            Mathf.Abs(y-2), Mathf.Abs(y-2), 
            Mathf.Abs(y-1), Mathf.Abs(y-1), 
            Mathf.Abs(y), Mathf.Abs(y),
            Mathf.Abs(y+1)
            };
        for (int i=0; i<20; i++) {           
            GameObject block = GameObject.Find("Block_" + z[i] + "_" + c[i]);
            if (block) {  
                blockLists.Add(block.GetComponent<BlockController>());
            }
        }
        for (int i=0; i<blockLists.Count; i++) {
            blockLists[i].energyAmount += energyAmount / blockLists.Count;
            blockLists[i].magicAmount += magicAmount / blockLists.Count;
            if (energyAmount % blockLists.Count >= i+1) blockLists[i].energyAmount++;
            if (magicAmount % blockLists.Count >= i+1) blockLists[i].magicAmount++;
        }
        
    }
}