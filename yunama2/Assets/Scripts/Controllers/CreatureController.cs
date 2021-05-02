using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
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

    [SerializeField] bool isCarryType = true;
    [SerializeField] bool carryTypeIsEnergy = true;
    [SerializeField] int carryingAmount = 0;
    [SerializeField] int maxCarryAmount = 3;
    [SerializeField] bool checkIfNonEmptyBlockAvailable = false;

    [SerializeField] bool waitToDrain = false;
    [SerializeField] bool waitToDeliver = false;
    [SerializeField] bool waitDraining = false;
    [SerializeField] bool waitDelivering = false;
    [SerializeField] bool waitNextAction = false;

    [SerializeField] bool carrying = false;
    [SerializeField] bool draining = false;
    [SerializeField] bool delivering = false;

    float timer = 0;
    float waitNextActionTime = 0.5f;
    float waitDrainTime = 1.0f;
    float waitDeliverTime = 1.0f;

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
    }

    void FixedUpdate() {
        if (draining) waitDrain();
        else if (delivering) waitDeliver();
        else if (!changingDirection) {
            move();
            carrierAction();
        }
        Debug.Log(checkIfNonEmptyBlockAvailable);
        //animator 
        if (direction == 0) {
            animator.SetFloat("Move Y", 1);
            animator.SetFloat("Move X", 0);
        } else if (direction == 1) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", -1);
        } else if (direction == 2) {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", 1);
        } else if (direction == 3) {
            animator.SetFloat("Move Y", -1);
            animator.SetFloat("Move X", 0);
        }
        animator.SetBool("Drain",draining);
        animator.SetBool("Delivering",delivering);
        animator.SetBool("isCarrying",carrying);
    }

    void waitDrain() {
        if (waitDraining) {
            timer += Time.deltaTime;
            if (timer >= waitDrainTime) {
                timer = 0;
                waitDraining = false;
                draining = false;
            }
        }
    }

    void waitDeliver() {
        if (waitDelivering) {
            timer += Time.deltaTime;
            if (timer >= waitDeliverTime) {
                timer = 0;
                waitDelivering = false;
                delivering = false;
            }
        }
    }

    void carrierAction() {
        if (waitNextAction) {
            timer += Time.deltaTime;
            if (timer >= waitNextActionTime) {
                timer = 0;
                waitNextAction = false;
            }
        } else if(!draining && !delivering) {
            if (checkIfNonEmptyBlockAvailable) {
                exchangeEnergyOrMagic(true, !carrying);
            }
            setcheckIfNonEmptyBlockAvailable();
        }
    }

    void changeDirection() {
        changingDirection = true;
        direction = randomDirection();
        changingDirection = false;
        willChangeDirection = false;
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
            }
        }
        int random = Random.Range(0,possibleDirectionsIdx+1);
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
                if (!willChangeDirection) willChangeDirection =
                    !dungeonController.isBlockEmpty(new int[] {Mathf.FloorToInt(transform.position.x), -Mathf.CeilToInt(transform.position.y)});
                if (willChangeDirection && (-transform.position.y)%1.0f < 0.5f) {
                    changeDirection();
                }
                break;
            case 1: 
                transform.position = transform.position + new Vector3(-1f*speed, 0, 0);
                if (!willChangeDirection) willChangeDirection =
                    !dungeonController.isBlockEmpty(new int[] {Mathf.FloorToInt(transform.position.x)-1, -Mathf.FloorToInt(transform.position.y)});
                if (willChangeDirection && transform.position.x%1.0f < 0.5f) {
                    changeDirection();
                }
                break;
            case 2: 
                transform.position = transform.position + new Vector3(1f*speed, 0, 0);
                if (!willChangeDirection) willChangeDirection =
                    !dungeonController.isBlockEmpty(new int[] {Mathf.CeilToInt(transform.position.x), -Mathf.FloorToInt(transform.position.y)});
                if (willChangeDirection && transform.position.x%1.0f > 0.5f) {
                    changeDirection();
                }
                break;
            case 3: 
                transform.position = transform.position + new Vector3(0, -1f*speed, 0);
                if (!willChangeDirection) willChangeDirection =
                    !dungeonController.isBlockEmpty(new int[] {Mathf.FloorToInt(transform.position.x), -Mathf.FloorToInt(transform.position.y-1.0f)});
                if (willChangeDirection && (-transform.position.y)%1.0f > 0.5f) {
                    changeDirection();
                }
                break;
        }
    }

    bool aroundCenterOfBlock() {
        float x = transform.position.x;
        float y = -(transform.position.y);

        bool centerX = x%1.00f > 0.48f && x%1.00f < 0.52f;
        bool centerY = y%1.00f > 0.48f && y%1.00f < 0.52f;

        if (direction == 0 && centerY) return true;
        else if (direction == 1 && centerX) return true;
        else if (direction == 2 && centerX) return true;
        else if (direction == 3 && centerY) return true;
        else return false;
    }

    void setcheckIfNonEmptyBlockAvailable() {
        checkIfNonEmptyBlockAvailable = aroundCenterOfBlock(); 
    }

    void exchangeEnergyOrMagic(bool isEnergy, bool isDrain) {
        int[] possibleDirections = new int[4];
        int possibleDirectionsIdx = 0;
        int[] position = currentPosition();

        int[] dx = {0, -1, 1, 0};
        int[] dy = {-1, 0, 0, 1};
        for(int i=0; i<4; i++){
            int x = position[0] + dx[i];
            int y = position[1] + dy[i];
            GameObject block = GameObject.Find("Block_" + x + "_" + y);

            if(block != null) {
                BlockController blockController = block.GetComponent<BlockController>();
                if (blockController.energyAmount > 0) {
                    possibleDirections[possibleDirectionsIdx] = i;
                    possibleDirectionsIdx++;
                }
            }
        }
        int random = Random.Range(0,possibleDirectionsIdx);

        if (possibleDirectionsIdx > 0) {
            int nextX = position[0] + dx[possibleDirections[random]];
            int nextY = position[1] + dy[possibleDirections[random]];
            GameObject chosenBlock = GameObject.Find("Block_" + nextX + "_" + nextY);
            BlockController chosenBlockController = chosenBlock.GetComponent<BlockController>();

            if (isDrain) {
                int currentMaxCarryAmount = maxCarryAmount - carryingAmount;

                if (chosenBlockController.energyAmount > currentMaxCarryAmount) {
                    chosenBlockController.changeEnergy(-1*currentMaxCarryAmount);
                    carryingAmount += currentMaxCarryAmount;
                } else {
                    carryingAmount += chosenBlockController.energyAmount;
                    chosenBlockController.changeEnergy(-1*chosenBlockController.energyAmount);
                }

                carrying = carryingAmount > 1;
                draining = true;
                waitDraining = true;
                checkIfNonEmptyBlockAvailable = false;
                Debug.Log("drain");
            } else {
                if (carrying) {
                    chosenBlockController.changeEnergy(carryingAmount -1);
                    carryingAmount = 1;

                    carrying = false;
                    delivering = true;
                    waitDelivering = true;
                    checkIfNonEmptyBlockAvailable = false;
                    Debug.Log("deliver");
                }
            }
            waitNextAction = true;
        }
    }

    void checkNextBlock() {
        if (willChangeDirection) willChangeDirection =
            !dungeonController.isBlockEmpty(new int[] {Mathf.CeilToInt(transform.position.x), -Mathf.FloorToInt(transform.position.y)});
        if (willChangeDirection && transform.position.y%1.0f < 0.5f) {
            transform.position = new Vector3(transform.position.x, Mathf.FloorToInt(transform.position.y) + 0.5f, 0);
            changeDirection();
        }
    }
}
