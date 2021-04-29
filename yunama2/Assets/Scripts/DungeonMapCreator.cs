using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonMapCreator : MonoBehaviour
{
    public TileBase wallBlock;
    public TileBase groundBlock;
    public GameObject emptyBlock;
    public GameObject energyBlock1;
    public GameObject energyBlock2;
    public GameObject energyBlock3;
    public GameObject magicBlock1;
    public GameObject magicBlock2;
    public GameObject magicBlock3;

    public int randomEmptyRange = 4;
    public int randomEnergyAmountRange = 15;
    public int randomMagicAmountRange = 15;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    public int mapWidth = 59;
    public int mapHeight = 45;
    public int wallThickness = 5;

    int tileOffsetX = 0;
    int tileOffsetY = 0;

    // public Block block = createBlock();

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
        for(int i=0; i<mapWidth; i++){
            for(int j=0; j<mapHeight; j++){

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

    bool isEmptyBlock() 
    {
        return Random.Range(0, randomEmptyRange) == 0;
    }
}