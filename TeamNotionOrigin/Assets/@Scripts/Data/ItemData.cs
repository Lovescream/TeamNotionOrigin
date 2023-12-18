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

public enum ItemType
{
    Pickup,//단순 소모성, 재화
    Weapon,//상점, 방 클리어 보상, 보스
    Passive,//상점, 방 클리어 보상, 보스
}