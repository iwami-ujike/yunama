using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonMapCreator : MonoBehaviour
{
    public TileBase wallBlock;
    public TileBase groundBlock;

    public int mapWidth = 59;
    public int mapHeight = 45;
    public int wallThickness = 5;

    int tileOffsetX = 0;
    int tileOffsetY = 0;

    public GameObject blockPrefab;

    // ex. 4 = 40% , 10 = 100%
    public int randomEmptyRange = 7;
    public int randomEnergyAmountRange = 15;
    public int randomMagicAmountRange = 15;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    void Start()
    {   
        CreateDungeonMap();
    }

    void Update()
    {
        
    }

    void CreateDungeonMap(){
        createOutsideWall();
        createInsideBlocks();
    }

    // 掘れるブロック生成
    void createInsideBlocks(){
        // 右上から左に行く毎に x＋１下に行く毎に y-1 ブロックの中心は 0.5, 0.5
        for(int i=0; i<mapHeight-1; i++){
            for(int j=0; j<mapWidth-1; j++){
                GameObject block = Instantiate(blockPrefab, new Vector3(j+wallThickness+1.5f,-i-1+0.5f,0),Quaternion.identity);
                initalizeBlock(block);
            }
        }
    }
    
    // 外側の壁を生成
    void createOutsideWall(){
        var tilemap = GetComponent<Tilemap>();
        Vector3Int position = new Vector3Int(0,0,0);
        for(int i=0; i<=mapHeight+wallThickness; i++){
            for(int j=0; j<=mapWidth+wallThickness*2; j++){
                if(i==0){
                    // 上の一列の壁作成
                    if(j!=(mapWidth+wallThickness*2)/2) 
                        tilemap.SetTile(new Vector3Int(j+tileOffsetX,tileOffsetY-i,0), groundBlock);
                } else if(i<mapHeight){
                    // 左右の壁
                    if(j<=wallThickness || j>=mapWidth+wallThickness)                        
                        tilemap.SetTile(new Vector3Int(j+tileOffsetX,tileOffsetY-i,0), wallBlock);
                } else {
                    // 下の壁                      
                        tilemap.SetTile(new Vector3Int(j+tileOffsetX,tileOffsetY-i,0), wallBlock);
                } 
            }
        }
    }

    void initalizeBlock(GameObject block){
        BlockController blockController = block.GetComponent<BlockController>();
        bool isEmpty = Random.Range(0,10) < randomEmptyRange;
        if (!isEmpty) {
            // bool isEnergy = Random.Range();
            int energyAmount = Random.Range(1,randomEnergyAmountRange+1);
            blockController.changeEnergy(energyAmount);
        }
    }

    bool isEmptyBlock() {
        return Random.Range(0, randomEmptyRange) == 0;
    }
}