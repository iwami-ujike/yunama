using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    private void Start() {

    }

    public Hashtable getStatus(int level, bool isEnergyType, bool isAlone) {
        Hashtable data = new Hashtable();
        if (isEnergyType) {
            data.Add("isEnergyType", 1);

            if (level == 1) {
                Debug.Log("get yukiusagi");
                data.Add("name", "Yukiusagi");
                data.Add("healthPoint", 21);
                data.Add("spawnHp",16);
                data.Add("attackDamage", 13);
                data.Add("attackPoint", 12);
                data.Add("armour", 2);
                data.Add("magicResistance", 0);
                data.Add("isCarryType", 1);
            } else if (level == 2) {
                Debug.Log("get Medama");
                data.Add("name", "Medama");
                data.Add("healthPoint", 64);
                data.Add("spawnHp",23);
                data.Add("attackDamage", 19);
                data.Add("attackPoint", 0);
                data.Add("armour", 2);
                data.Add("magicResistance", 0);
                data.Add("isCarryType", 0);
            } else if (level == 3 && isAlone) {
                Debug.Log("get Pegasus");
                data.Add("name", "Pegasus");
                data.Add("healthPoint", 480);
                data.Add("spawnHp",350);
                data.Add("attackDamage", 36);
                data.Add("attackPoint", 0);
                data.Add("armour", 11);
                data.Add("magicResistance", 24);
                data.Add("isCarryType", 0);
            } else if (level == 3) {
                Debug.Log("get Mithril");
                data.Add("name", "Mithril");
                data.Add("healthPoint", 240);
                data.Add("spawnHp",160);
                data.Add("attackDamage", 35);
                data.Add("attackPoint", 12);
                data.Add("armour", 11);
                data.Add("magicResistance", 5);
                data.Add("isCarryType", 0);
            }
        } else { // magic type
            data.Add("isEnergyType", 0);
            
            if (level == 1) {
                Debug.Log("get Io");
                data.Add("name", "Io");
                data.Add("healthPoint", 60);
                data.Add("spawnHp",10);
                data.Add("attackDamage", 0);
                data.Add("attackPoint", 3);
                data.Add("armour", 0);
                data.Add("magicResistance", 10);
                data.Add("isCarryType", 1);
            } else if (level == 2) {
                Debug.Log("get Obake");
                data.Add("name", "Obake");
                data.Add("healthPoint", 180);
                data.Add("spawnHp",80);
                data.Add("attackDamage", 13);
                data.Add("attackPoint", 26);
                data.Add("armour", 0);
                data.Add("magicResistance", 10);
                data.Add("isCarryType", 0);
            } else if (level == 3 && isAlone) {
                Debug.Log("get Dragon");
                data.Add("name", "Dragon");
                data.Add("healthPoint", 300);
                data.Add("spawnHp",250);
                data.Add("attackDamage", 21);
                data.Add("attackPoint", 28);
                data.Add("armour", 41);
                data.Add("magicResistance", 21);
                data.Add("isCarryType", 0);
            } else if (level == 3) {
                Debug.Log("get Death");
                data.Add("name", "Death");
                data.Add("healthPoint", 660);
                data.Add("spawnHp",480);
                data.Add("attackDamage", 42);
                data.Add("attackPoint", 0);
                data.Add("armour", 55);
                data.Add("magicResistance", 77);
                data.Add("isCarryType", 0);
            }
        }
        return data;
    }
}
