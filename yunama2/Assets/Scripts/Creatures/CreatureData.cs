using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    private void Start() {
        Debug.Log("creature data");    
    }

    public Hashtable getStatus(string name) {
        Hashtable data = new Hashtable();
        switch(name) {
            case "Yukiusagi":
            Debug.Log("get yukiusagi");
                data.Add("healthPoint", 12);
                data.Add("magicPoint", 12);
                data.Add("attackDamage", 12);
                data.Add("attackPoint", 12);
                data.Add("armour", 12);
                data.Add("magicResistance", 12);
                data.Add("energyAmount", 12);
                data.Add("magicAmount", 12);
                data.Add("isCarryType", 1);
            break;
            case "Medama":
                data.Add("healthPoint", 15);
                data.Add("magicPoint", 15);
                data.Add("attackDamage", 15);
                data.Add("attackPoint", 15);
                data.Add("armour", 15);
                data.Add("magicResistance", 15);
                data.Add("energyAmount", 15);
                data.Add("magicAmount", 15);
                data.Add("isCarryType", 0);
            break;
        }
        return data;
    }
}
