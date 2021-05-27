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
    AudioSource soundEffect;

    void Start() {
        dungeonController = DungeonControllerOB.GetComponent<DungeonController>();
        dungeonMapCreator = tileMap.GetComponent<DungeonMapCreator>();
        transform.position = dungeonController.getEntrance() + new Vector3(-0.5f, -3.75f, 0);

        soundEffect = GetComponent<AudioSource>();
    }

    void Update() {
        
    }

    public void moveUp(){
        transform.position += new Vector3(0, 1, 0);
        soundEffect.PlayOneShot(sound_1);
    }

    public void moveLeft(){
        transform.position += new Vector3(-1, 0, 0);
        soundEffect.PlayOneShot(sound_1);
    }

    public void moveRight(){
        transform.position += new Vector3(1, 0, 0);
        soundEffect.PlayOneShot(sound_1);
    }

    public void moveDown(){
        transform.position += new Vector3(0, -1, 0);
        soundEffect.PlayOneShot(sound_1);
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
        } else if (chosenObject = GameObject.FindWithTag("Creature")) {
            dungeonController.objectKillTheCreature(chosenObject);
        }
    }

    public void detailsBlock() {
        chosenObject = null;

        int x = (int)this.transform.position.x;
        int y = -Mathf.FloorToInt(this.transform.position.y);
        // chosenObject = GameObject.Find("Block_" + x + "_" + y);

        if (chosenObject = GameObject.Find("Block_" + x + "_" + y)) {
            dungeonController.objectDetailsText(chosenObject);
        } else if (chosenObject = GameObject.FindWithTag("Creature")) {
            dungeonController.objectDetailsText(chosenObject);
        }
    }
    void initalizeCursor() {
        // transform.scale = new Vector3(8, 7.8, 1);
    }
}
