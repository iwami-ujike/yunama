using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    // 0→上 1→左 2→右 3→下
    int direction = 0;
    public float speed = 0.01f;

    public GameObject cursor;
    public GameObject dungeonControllerGO;
    CursorController cursorController;
    DungeonController dungeonController;

    Animator animator;

    void Start() {
        cursorController = cursor.GetComponent<CursorController>();
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();
        
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        // Debug.Log(currentPosition()[0]);
        // Debug.Log(currentPosition()[1]);

        //animator 
        if (direction==0) {
            animator.SetFloat("Move Y", 1);
            animator.SetFloat("Move X", 0);
        } else if (direction==1) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", -1);
        } else if (direction==2) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", 1);
        } else if (direction==3) {
            animator.SetFloat("Move Y", -1);
            animator.SetFloat("Move X", 0);
        }
        
        move();
    }

    void changeDirection() {
        direction = randomDirection();
    }

    int randomDirection() {
        int[] possibleDirections = new int[4];
        int possibleDirectionsIdx = 0;
        int[] position = currentPosition();

        int[] dx = {0, -1, 1, 0};
        int[] dy = {-1, 0, 0, 1};
        for(int i=0; i<4; i++){
            int nextX = position[0] + dx[i];
            int nextY = position[1] + dy[i];
            if(dungeonController.isBlockEmpty(new int[] {nextX, nextY})) {
                possibleDirections[possibleDirectionsIdx] = i;
                possibleDirectionsIdx++;
                // Debug.Log(nextX);
                // Debug.Log(nextY);
            }
        }

        int random = Random.Range(0,possibleDirectionsIdx+1);
        //Debug.Log(possibleDirections[random]);
        return possibleDirections[random];
    }

    int[] currentPosition() {
        int x = Mathf.FloorToInt(transform.position.x);
        int y = Mathf.FloorToInt(transform.position.y);
        return new int[] { x, -y };
    }

    void move() {
        switch(direction) {
            case 0: 
                transform.position = transform.position + new Vector3(0, 1f*speed, 0);
                if (!dungeonController.isBlockEmpty(new int[] {Mathf.CeilToInt(transform.position.x), -Mathf.FloorToInt(transform.position.y)})) {

                }
                break;
            case 1: 
                transform.position = transform.position + new Vector3(-1f*speed, 0, 0);
                break;
            case 2: 
                transform.position = transform.position + new Vector3(1f*speed, 0, 0);
                break;
            case 3: 
                transform.position = transform.position + new Vector3(0, -1f*speed, 0);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Block") {
            Debug.Log("敵と接触した！");
            changeDirection();
        }
    }
}
