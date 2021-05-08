using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    private void Start() {
        Debug.Log("creature data");    
    }

    public Hashtable getStatus(int level, bool isEnergyType) {
        Hashtable data = new Hashtable();
        if (isEnergyType) {
            data.Add("isEnergyType", 1);

            if (level == 1) {
                Debug.Log("get yukiusagi");
                data.Add("name", "Yukiusagi");
                data.Add("healthPoint", 12);
                data.Add("magicPoint", 12);
                data.Add("attackDamage", 12);
                data.Add("attackPoint", 12);
                data.Add("armour", 12);
                data.Add("magicResistance", 12);
                data.Add("isCarryType", 1);
            } else if (level == 2) {
                Debug.Log("get Medama");
                data.Add("name", "Medama");
                data.Add("healthPoint", 15);
                data.Add("magicPoint", 15);
                data.Add("attackDamage", 15);
                data.Add("attackPoint", 15);
                data.Add("armour", 15);
                data.Add("magicResistance", 15);
                data.Add("isCarryType", 0);
            } else if (level == 3) {
                Debug.Log("get Mithril");
                data.Add("name", "Mithril");
                data.Add("healthPoint", 12);
                data.Add("magicPoint", 12);
                data.Add("attackDamage", 12);
                data.Add("attackPoint", 12);
                data.Add("armour", 12);
                data.Add("magicResistance", 12);
                data.Add("isCarryType", 0);
            }
        } else { // magic type
            data.Add("isEnergyType", 0);
            
            if (level == 1) {
                Debug.Log("get Io");
                data.Add("name", "Io");
                data.Add("healthPoint", 12);
                data.Add("magicPoint", 12);
                data.Add("attackDamage", 12);
                data.Add("attackPoint", 12);
                data.Add("armour", 12);
                data.Add("magicResistance", 12);
                data.Add("isCarryType", 1);
            } else if (level == 2) {
                Debug.Log("get aaaa");
                data.Add("name", "aaaa");
                data.Add("healthPoint", 15);
                data.Add("magicPoint", 15);
                data.Add("attackDamage", 15);
                data.Add("attackPoint", 15);
                data.Add("armour", 15);
                data.Add("magicResistance", 15);
                data.Add("isCarryType", 0);
            } else if (level == 3) {
                Debug.Log("get aaa");
                data.Add("name", "aaa");
                data.Add("healthPoint", 15);
                data.Add("magicPoint", 15);
                data.Add("attackDamage", 15);
                data.Add("attackPoint", 15);
                data.Add("armour", 15);
                data.Add("magicResistance", 15);
                data.Add("isCarryType", 0);
            }
        }
        return data;
    }
}
