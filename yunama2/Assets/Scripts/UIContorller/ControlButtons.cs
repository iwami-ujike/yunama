using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtons : MonoBehaviour
{
    public GameObject cursor;
    GameObject chosenObject;
    CursorController cursorController;

    void Start() {
        cursorController = cursor.GetComponent<CursorController>();
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

        Ray ray = Camera.main.ScreenPointToRay(transform.position);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)transform.position, -(Vector2)ray.direction);

        if (hit2d) 
            chosenObject = hit2d.transform.gameObject;

        if (chosenObject && chosenObject.name == "Block(Clone)") 
            Destroy(chosenObject);
    }
}
