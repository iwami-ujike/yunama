using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    EnemyController enemyController;   
    CreatureController creatureController;
    void Start() {
        enemyController = GetComponentInParent<EnemyController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Creature") {
            Debug.Log("enemy");
            enemyController.attacking = true;
            enemyController.creatureController = other.GetComponent<CreatureController>();
        }
    }
}
