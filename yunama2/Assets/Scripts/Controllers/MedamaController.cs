using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedamaController : MonoBehaviour
{
    // 0→上 1→左 2→右 3→下
    [SerializeField] int direction = 0;
    public float speed = 0.02f;
    [SerializeField] bool willChangeDirection = false;
    [SerializeField] bool changingDirection = false;

    [SerializeField] int healthPoint = 10;
    [SerializeField] int magicPoint = 10;
    [SerializeField] int attackDamage = 10;
    [SerializeField] int attackPoint = 10;
    [SerializeField] int armour = 10;
    [SerializeField] int magicResistance = 10;
    [SerializeField] int energyAmount = 10;
    [SerializeField] int magicAmount = 10;

    [SerializeField] bool waitNextAction = false;

    [SerializeField] bool attacking = false;
    [SerializeField] bool kidnapping = false;

    float timer = 0;
    float waitNextActionTime = 0.5f;
    float waitDrainTime = 1.0f;
    float waitDeliverTime = 1.0f;

    public int[,] walkedMap;

    [SerializeField] int[] beforePosition;

    GameObject cursor;
    GameObject dungeonControllerGO;
    CursorController cursorController;
    DungeonController dungeonController;

    Animator animator;

    void Start() {
        cursor = GameObject.Find("Cursor");
        dungeonControllerGO = GameObject.Find("DungeonController");
        cursorController = cursor.GetComponent<CursorController>();
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();
        
        animator = GetComponent<Animator>();

        initalizeWalkedMap();
        beforePosition = currentPosition();
    }

    void FixedUpdate() {
        if (attacking) { 
            attack();
        } else if (!changingDirection) {
            move();
        }
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
    }

    void attack() {

    }

    void changeDirection() {
        changingDirection = true;

        int[] position = currentPosition();
        walkedMap[position[1], position[0]] += 1;

        direction = chooseDirection();

        changingDirection = false;
    }

    int chooseDirection() {
        int[] possibleDirections = new int[4];
        int possibleDirectionsIdx = 0;
        int[] position = currentPosition();

        int leastWalkedNum = 1000000;

        int[] dx = {0, -1, 1, 0};
        int[] dy = {-1, 0, 0, 1};
        for(int i=0; i<4; i++){
            int nextX = position[0] + dx[i];
            int nextY = position[1] + dy[i];
            if(dungeonController.isBlockEmpty(new int[] {nextX, nextY})) {
                if (walkedMap[nextY, nextX] < leastWalkedNum) {
                    leastWalkedNum = walkedMap[nextY, nextX];
                    possibleDirections = new int[4];
                    possibleDirectionsIdx = 0;
                } else if (walkedMap[nextY, nextX] > leastWalkedNum) {
                    continue;
                }
                possibleDirections[possibleDirectionsIdx] = i;
                possibleDirectionsIdx++;
            }
        }
        int random = Random.Range(0,possibleDirectionsIdx);
        return possibleDirections[random];
    }

    int[] currentPosition() {
        int x = Mathf.FloorToInt(transform.position.x);
        int y = Mathf.FloorToInt(transform.position.y);
        return new int[] { x, -y };
    }

    void move() {
        if (beforePosition[0] != currentPosition()[0] || beforePosition[1] != currentPosition()[1]) {
            willChangeDirection = true;
            beforePosition = currentPosition();
        }

        switch(direction) {
            case 0: 
                transform.position = transform.position + new Vector3(0, 1f*speed, 0);
                if (willChangeDirection && (-transform.position.y)%1.0f < 0.5f) {
                    willChangeDirection = false;
                    changeDirection();
                }
                break;
            case 1: 
                transform.position = transform.position + new Vector3(-1f*speed, 0, 0);
                if (willChangeDirection && transform.position.x%1.0f < 0.5f) {
                    willChangeDirection = false;
                    changeDirection();
                }
                break;
            case 2: 
                transform.position = transform.position + new Vector3(1f*speed, 0, 0);
                if (willChangeDirection && transform.position.x%1.0f > 0.5f) {
                    willChangeDirection = false;
                    changeDirection();
                }
                break;
            case 3: 
                transform.position = transform.position + new Vector3(0, -1f*speed, 0);
                if (willChangeDirection && (-transform.position.y)%1.0f > 0.5f) {
                    willChangeDirection = false;
                    changeDirection();
                }
                break;
        }
    }

    void initalizeWalkedMap(){
        int mapHeight = dungeonController.mapHeight;
        int mapWidth = dungeonController.mapWidth; 
        int wallThickness = dungeonController.wallThickness;

        walkedMap = new int[mapHeight+ wallThickness + 1, mapWidth + wallThickness*2 + 1];
        for (int j=wallThickness; j<mapWidth+wallThickness; j++) {
                walkedMap[0,j] = 10000;
        }
        for (int i=1; i<=mapHeight; i++) {
            for (int j=wallThickness; j<mapWidth+wallThickness; j++) {
                walkedMap[i,j] = 0;
            }
        }
    }
}

