using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject tileMap;
    GameObject chosenBlock;

    DungeonMapCreator dungeonMapCreator;

    void Start() {
        dungeonMapCreator = tileMap.GetComponent<DungeonMapCreator>();
        transform.position = dungeonMapCreator.getEntrance();
    }

    void Update() {

    }

    public void moveUp(){
        transform.position += new Vector3(0, 1, 0);
    }

    public void moveLeft(){
        transform.position += new Vector3(-1, 0, 0);
    }

    public void moveRight(){
        transform.position += new Vector3(1, 0, 0);
    }

    public void moveDown(){
        transform.position += new Vector3(0, -1, 0);
    }
}
