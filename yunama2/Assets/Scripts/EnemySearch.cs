using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    EnemyController enemyController;   
    void Start() {
        enemyController = GetComponentInParent<EnemyController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Col" + other.gameObject.tag);
        if (other.tag == "Creature") {
            enemyController.attack();
        }
    }
}
