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
    public int armour = 10;
    [SerializeField] int magicResistance = 10;
    [SerializeField] int energyAmount = 10;
    [SerializeField] int magicAmount = 10;

    [SerializeField] bool isEnergyType = true;

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
    [SerializeField] bool attacking = false;

    [SerializeField] float timer = 0;
    [SerializeField] float waitNextActionTime = 0.5f;
    [SerializeField] float waitDrainTime = 1.2f;
    [SerializeField] float waitDeliverTime = 1.2f;

    [SerializeField] bool isFight = false;
    int currentHP;

    public int[,] walkedMap;

    [SerializeField] int[] beforePosition;

    GameObject cursor;
    GameObject dungeonControllerGO;
    GameObject enemy;
    CursorController cursorController;
    DungeonController dungeonController;
    EnemyController enemyController;
    
    [SerializeField] GameObject creatureDataGO;
    [SerializeField] CreatureData creatureData;

    Animator animator;

    void Start() {
        cursor = GameObject.Find("Cursor");
        dungeonControllerGO = GameObject.Find("DungeonController");
        enemy = GameObject.Find("Enemy");

        cursorController = cursor.GetComponent<CursorController>();
        dungeonController = dungeonControllerGO.GetComponent<DungeonController>();
        enemyController = enemy.GetComponent<EnemyController>();

        animator = GetComponent<Animator>();
        if (!isCarryType) {
            initalizeWalkedMap();
        }
        beforePosition = currentPosition();

        direction = randomDirection();
        currentHP = healthPoint;
    }

    void FixedUpdate() {
        if (isCarryType) {
            if (draining) waitDrain();
            else if (delivering) waitDeliver();
            else if (attacking) attack();
            else if (!changingDirection) {
                move();
                carrierAction();
            }
        } else {
            if (attacking) { 
                attack();
            } else if (!changingDirection) {
                move();
            }
        }

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
        if (isCarryType) {
            animator.SetBool("Drain",draining);
            animator.SetBool("Delivering",delivering);
            animator.SetBool("isCarrying",carrying);
        }
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
            if (aroundCenterOfBlock()) {
                exchangeEnergyOrMagic(isEnergyType, !carrying);
            }
        }
    }

    void attack() {
        int damage = Mathf.Max(attackDamage - enemyController.armour,0);
        if (attacking) {
            if (isFight) {

            }
            enemyController.gotDamaged(damage);
        }
        attacking = false;
    }

    public void gotDamaged(int damage) {
        currentHP = Mathf.Max(currentHP - damage, 0);
        Debug.Log(currentHP);
        if (currentHP == 0) {
            Destroy(gameObject);
        }
    }

    void changeDirection() {
        changingDirection = true;
    
        if (isCarryType) {
            direction = randomDirection();
            willChangeDirection = false;
        } else {
            direction = chooseDirection();
        }
        changingDirection = false;
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
        int random = Random.Range(0,possibleDirectionsIdx);
        return possibleDirections[random];
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
        if (isCarryType) {
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
        } else {
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
    }

    bool aroundCenterOfBlock() {
        float x = transform.position.x;
        float y = -(transform.position.y);

        bool centerX = 0.49f < x%1.00f && x%1.00f < 0.51f;
        bool centerY = 0.49f < y%1.00f && y%1.00f < 0.51f;

        if (direction == 0 && centerY) return true;
        else if (direction == 1 && centerX) return true;
        else if (direction == 2 && centerX) return true;
        else if (direction == 3 && centerY) return true;
        else return false;
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
                if ((isEnergy && blockController.energyAmount > 0) 
                    || (!isEnergy && blockController.magicAmount > 0)) {
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
                if (isEnergy) {
                    if (chosenBlockController.energyAmount > currentMaxCarryAmount) {
                        chosenBlockController.changeEnergy(-1*currentMaxCarryAmount);
                        carryingAmount += currentMaxCarryAmount;
                    } else {
                        carryingAmount += chosenBlockController.energyAmount;
                        chosenBlockController.changeEnergy(-1*chosenBlockController.energyAmount);
                    }
                } else {
                    if (chosenBlockController.magicAmount > currentMaxCarryAmount) {
                        chosenBlockController.changeMagic(-1*currentMaxCarryAmount);
                        carryingAmount += currentMaxCarryAmount;
                    } else {
                        carryingAmount += chosenBlockController.magicAmount;
                        chosenBlockController.changeMagic(-1*chosenBlockController.magicAmount);
                    }
                }

                carrying = carryingAmount > 1;
                draining = true;
                waitDraining = true;
                Debug.Log("drain");
            } else {
                if (carrying) {
                    if (isEnergy) {
                        chosenBlockController.changeEnergy(carryingAmount -1);
                    } else {
                        chosenBlockController.changeMagic(carryingAmount -1);
                    }
                    carryingAmount = 1;

                    carrying = false;
                    delivering = true;
                    waitDelivering = true;
                    Debug.Log("deliver");
                }
            }   
            waitNextAction = true;
        }
    }

    public void setCreatureStatus(int level,  int carrying_amount, bool is_Energy_Type, int energy_amount, int magic_amount, bool isAlone) {
        creatureDataGO = GameObject.Find("CreatureData");
        creatureData = creatureDataGO.GetComponent<CreatureData>();

        Hashtable data = creatureData.getStatus(level, is_Energy_Type, isAlone);

        name = data["name"].ToString();
        healthPoint = (int)data["healthPoint"];
        magicPoint = (int)data["magicPoint"];
        attackDamage = (int)data["attackDamage"];
        attackPoint = (int)data["attackPoint"];
        armour = (int)data["armour"];
        magicResistance = (int)data["magicResistance"];
        energyAmount = energy_amount;
        magicAmount = magic_amount;

        isCarryType = (int)data["isCarryType"] == 1;
        isEnergyType = (int)data["isEnergyType"] == 1;
        Debug.Log((int)data["isEnergyType"]);

        if (isCarryType) {
            carryingAmount = carrying_amount;
            carrying = true; 
        } 
    }

    public int getCarryingAmount() {
        return carryingAmount;
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

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("trigger");
        Debug.Log("Col" + other.gameObject.tag);
        if (other.tag == "Enemy") {
            attacking = true;
            isFight = true;
            attack();
        }
    }
}
