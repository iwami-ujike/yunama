using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject tileMap;
    public GameObject DungeonControllerOB;
    GameObject chosenBlock;

    DungeonMapCreator dungeonMapCreator;
    DungeonController dungeonController;

    void Start() {
        dungeonController = DungeonControllerOB.GetComponent<DungeonController>();
        dungeonMapCreator = tileMap.GetComponent<DungeonMapCreator>();
        transform.position = dungeonController.getEntrance();
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

    void initalizeCursor() {
        // transform.scale = new Vector3(8, 7.8, 1);
    }
}
