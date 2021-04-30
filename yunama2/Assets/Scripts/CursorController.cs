using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject tileMap;
    GameObject chosenBlock;

    DungeonMapCreator dungeonMapCreator;
    // TransForm transForm;
    // Start is called before the first frame update
    void Start()
    {
        dungeonMapCreator = tileMap.GetComponent<DungeonMapCreator>();
        transform.position = dungeonMapCreator.getEntrance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
 
            chosenBlock = null;
 
            Ray ray = Camera.main.ScreenPointToRay(transform.position);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
 
            if (hit2d) {
                chosenBlock = hit2d.transform.gameObject;
            } 
 
            Debug.Log(chosenBlock);
        }
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
