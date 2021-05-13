using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtons : MonoBehaviour
{
    public GameObject cursor;
    public GameObject dungeonControllerGO;
    CursorController cursorController;
    DungeonController dungeonController;
    

    public float waitInput = 0.1f;
    float timer = 0;
    public bool waitingInput = true;
    
    void Start() {
        cursorController = cursor.GetComponent<CursorController>();
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();

        cursorController.detailsBlock();
    }

    void Update() {
        if (waitingInput) {
            float ad = Input.GetAxisRaw("Horizontal");
            float ws = Input.GetAxisRaw("Vertical");
            if (ad == 1) {
                onClickRight();
            } else if (ad == -1) {
                onClickLeft();
                waitingInput = false;
            } else if (ws == 1) {
                onClickUp();
                waitingInput = false;
            } else if (ws == -1) {
                onClickDown();
                waitingInput = false;
            }
        } else {
            timer += Time.deltaTime;
            if(timer > waitInput) {
                timer = 0;
                waitingInput = true;
                cursorController.detailsBlock();
            }
        }
        if (Input.GetKeyDown("space")) {
            clickCircle();
        }
    }

    public void onClickUp() {
        cursorController.moveUp();
        waitingInput = false;
    }

    public void onClickLeft() {
        cursorController.moveLeft();
        waitingInput = false;
    }

    public void onClickRight() {
        cursorController.moveRight();
        waitingInput = false;
    }

    public void onClickDown() {
        cursorController.moveDown();
        waitingInput = false;
    }
    
    public void clickCircle() {
        cursorController.destroyThisBlock();
        cursorController.detailsBlock();
    }

    public void clickDelta() {
        cursorController.cameraZoom();    
    }

    public void clickSquare() {
        cursorController.detailsBlock();
    }
}
