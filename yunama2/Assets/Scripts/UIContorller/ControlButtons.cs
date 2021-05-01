using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtons : MonoBehaviour
{
    public GameObject cursor;
    public GameObject dungeonControllerGO;
    CursorController cursorController;
    DungeonController dungeonController;
    
    GameObject chosenObject;

    void Start() {
        cursorController = cursor.GetComponent<CursorController>();
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();
    }

    void Update() {
        
    }

    public void onClickUp() {
        cursorController.moveUp();
    }

    public void onClickLeft() {
        cursorController.moveLeft();
    }

    public void onClickRight() {
        cursorController.moveRight();
    }

    public void onClickDown() {
        cursorController.moveDown();
    }

    public void clickCircle() {
        chosenObject = null;

        Ray ray = Camera.main.ScreenPointToRay(cursorController.transform.position);
        Vector2 position = new Vector2(cursorController.transform.position.x + 0.5f, cursorController.transform.position.y + 0.25f);
        RaycastHit2D hit2d = Physics2D.Raycast(position, new Vector2(0f,0f));

        if (hit2d) {
            chosenObject = hit2d.transform.gameObject;
            dungeonController.destroyBlock(chosenObject);
        }
    }
}
