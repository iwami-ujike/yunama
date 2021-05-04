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

        Ray ray = Camera.main.ScreenPointToRay(this.transform.position);
        Vector2 position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.25f);
        RaycastHit2D hit2d = Physics2D.Raycast(position, new Vector2(0f,0f));

        if (hit2d != null) {
            chosenObject = hit2d.transform.gameObject;
            dungeonController.destroyBlock(chosenObject);
        }
    }

    void initalizeCursor() {
        // transform.scale = new Vector3(8, 7.8, 1);
    }
}
