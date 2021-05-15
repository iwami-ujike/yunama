using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject tileMap;
    public GameObject DungeonControllerOB;

    GameObject chosenObject;
    

    DungeonMapCreator dungeonMapCreator;
    DungeonController dungeonController;

    public AudioClip sound_1;
    AudioSource soundEfect;

    void Start() {
        dungeonController = DungeonControllerOB.GetComponent<DungeonController>();
        dungeonMapCreator = tileMap.GetComponent<DungeonMapCreator>();
        transform.position = dungeonController.getEntrance();

        soundEfect = GetComponent<AudioSource>();
    }

    void Update() {
        
    }

    public void moveUp(){
        transform.position += new Vector3(0, 1, 0);
        soundEfect.PlayOneShot(sound_1);
    }

    public void moveLeft(){
        transform.position += new Vector3(-1, 0, 0);
        soundEfect.PlayOneShot(sound_1);
    }

    public void moveRight(){
        transform.position += new Vector3(1, 0, 0);
        soundEfect.PlayOneShot(sound_1);
    }

    public void moveDown(){
        transform.position += new Vector3(0, -1, 0);
        soundEfect.PlayOneShot(sound_1);
    }
    
    public void cameraZoom() {
        if(Camera.main.orthographicSize == 5) {
            Camera.main.orthographicSize += 5;
        } else {
            Camera.main.orthographicSize -= 5;
        }
    }

    public void destroyThisBlock() {
        chosenObject = null;

        int x = (int)this.transform.position.x;
        int y = -Mathf.FloorToInt(this.transform.position.y);
        chosenObject = GameObject.Find("Block_" + x + "_" + y);

        if (chosenObject != null) {
            dungeonController.destroyBlock(chosenObject);
        }
    }

    public void detailsBlock() {
        chosenObject = null;

        int x = (int)this.transform.position.x;
        int y = -Mathf.FloorToInt(this.transform.position.y);
        chosenObject = GameObject.Find("Block_" + x + "_" + y);

        if (chosenObject != null) {
            dungeonController.objectDetailsText(chosenObject);
        }
    }

    void initalizeCursor() {
        // transform.scale = new Vector3(8, 7.8, 1);
    }
}
