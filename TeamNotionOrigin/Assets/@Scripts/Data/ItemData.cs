using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData {

    public int id;
    public ItemType type;
    public string name;
    public string description;
    public float cost;
    public List<StatModifier> modifiers;

}

public enum ItemType {
    Weapon,
    Passive,
}