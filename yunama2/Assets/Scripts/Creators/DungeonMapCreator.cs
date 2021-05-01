using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonMapCreator : MonoBehaviour
{
    public TileBase wallBlock;
    public TileBase groundBlock;
    public TileBase insidewallBlock;

    public GameObject blockPrefab;
    public GameObject dungeonControllerGO;
    DungeonController dungeonController;

    int mapWidth;
    int mapHeight;
    int wallThickness;

    int randomEmptyRange;
    int randomEnergyAmountRange;
    int randomMagicAmountRange;
    int randomEnergyMagicRatio;

    void Start() {
        initalizeMapVariables();
        CreateDungeonMap();
    }

    void Update() {
        
    }

    void initalizeMapVariables() {
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();

        mapHeight = dungeonController.mapHeight;
        mapWidth = dungeonController.mapWidth;
        wallThickness = dungeonController.wallThickness;

        randomEmptyRange = dungeonController.randomEmptyRange;
        randomEnergyAmountRange = dungeonController.randomEnergyAmountRange;
        randomEnergyMagicRatio = dungeonController.randomEnergyMagicRatio;
        randomMagicAmountRange = dungeonController.randomMagicAmountRange;
    }

    void CreateDungeonMap(){
        createOutsideWall();
        createInsideBlocks();
        createInisdeWallBlocks();
    }

    // 掘れるブロック生成
    void createInsideBlocks(){
        // 右上から左に行く毎に x＋１下に行く毎に y-1 ブロックの中心は 0.5, 0.5
        for(int i=0; i<mapHeight-1; i++){
            for(int j=0; j<mapWidth; j++){
                // 入り口から真ん中３つは開けておく
                if((i == 0 || i == 1 || i== 2) && (j == (mapWidth/2))) continue;
                GameObject block = Instantiate(blockPrefab, new Vector3(j+wallThickness+0.5f,-i-1+0.5f,0), Quaternion.identity);
                initalizeBlock(block);
            }
        }
    }
    
    // 外側の壁を生成
    void createOutsideWall(){
        var tilemap = GetComponent<Tilemap>();
        Vector3Int position = new Vector3Int(0,0,0);
        for(int i=0; i<mapHeight+wallThickness; i++){
            for(int j=0; j<mapWidth+wallThickness*2; j++){
                if(i==0){
                    // 上の一列の壁作成
                    if(j!=(mapWidth+wallThickness*2)/2) 
                        tilemap.SetTile(new Vector3Int(j,-i,0), groundBlock);
                } else if(i<mapHeight){
                    // 左右の壁
                    if(j<wallThickness || j>=mapWidth+wallThickness)                        
                        tilemap.SetTile(new Vector3Int(j,-i,0), wallBlock);
                } else {
                    // 下の壁                      
                        tilemap.SetTile(new Vector3Int(j,-i,0), wallBlock);
                } 
            }
        }
    }
    void createInisdeWallBlocks() {
        var insidewall = GetComponent<Tilemap>();
        Vector3Int position = new Vector3Int(0,0,0);
        for(int i=0; i<mapHeight-1; i++) {
            for(int j=0; j<mapWidth; j++) {
                insidewall.SetTile(new Vector3Int(j+wallThickness,-i-1,0),insidewallBlock);
            }
        }
    }       

    void initalizeBlock(GameObject block){
        BlockController blockController = block.GetComponent<BlockController>();
        bool isEmpty = Random.Range(0,10) < randomEmptyRange;
        if (!isEmpty) {
            // bool isEnergy = Random.Range();
            int energyAmount = 0;
            bool isEnergyRich = Random.Range(0,10) > 8;
            if (isEnergyRich) {
                energyAmount = Random.Range(10,randomEnergyAmountRange+1);
            } else {
                energyAmount = Random.Range(1,10);
            } 
            blockController.changeEnergy(energyAmount);
        }
    }

    bool isEmptyBlock() {
        return Random.Range(0, randomEmptyRange) == 0;
    }
}