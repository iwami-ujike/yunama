using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonMapCreator : MonoBehaviour
{
    public TileBase tile;

    public int randomEmptyRange = 4;
    public int randomEnergyAmountRange = 15;
    public int randomMagicAmountRange = 15;
    // energy / magic ex. 5:1 = 5
    public int randomEnergyMagicRatio = 2;

    // public Block block = createBlock();

    void Start()
    {
        var tilemap = GetComponent<Tilemap>();
        Debug.Log(tilemap);
        var position = new Vector3Int( 0, 0, 0 );
        tilemap.SetTile( position, tile );
    }

    void Update()
    {
        
    }

    bool emptyBlock() 
    {
        return Random.Range(0, randomEmptyRange) == 0;
    }
}