using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSearch : MonoBehaviour
{
    CreatureController creatureController;
    void Start() {
        creatureController = GetComponentInParent<CreatureController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        //敵が入ってきたかどうか判定する
        Debug.Log("Col" + other.gameObject.tag);
        if (other.tag == "Enemy") {
            //範囲内に敵がいたら攻撃メソッドを呼び出す。
            creatureController.attack();
        }
    }
}
