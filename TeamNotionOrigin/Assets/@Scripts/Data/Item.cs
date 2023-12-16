using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data { 
public enum ItemType
{
    Pickup,//단순 소모성, 재화
    Weapon,//상점, 방 클리어 보상, 보스
    Passive,//상점, 방 클리어 보상, 보스
}

public enum BulletType
{
    Arrow,
    Cannon,
    Gun,
    Wand,
}

public enum ItemRarity
{
    Normal,
    Rare,
    Unique,
    Legendary,
}

public enum EffectType
{
    //데미지 증가
    DamageBoost,
    //이동속도 증가
    SpeedBoost,
    //탄환 크기 증가
    BulletSizeBoost,
    //탄창 증가
    MagazineBoost
}

[System.Serializable]
public class Item
{
    public ItemRarity itemRarity;
    public ItemType itemType;
    public string name;
    public string description;
}

public class Weapon : Item
{
    public int damage;
    public float attackSpeed;
    public float bulletSizeX;
    public float bulletSizeY;
    public float bulletSizeZ;
    //장전시간
    public float reloadTime;
    public BulletType bulletType;
    public float critical;
    public float maxBulletAmount;
}

public class Pickup : Item
{
    public float durationTime;
    //탄환같은 경우는 수량, 포션같은 경우는 수치
    public float numericalValue;

}

public class Passive : Item
{
    public bool stackable;
    public int maxStack;
    public EffectType effectType;
    //해당 효과들에 대한 수치
    public float numericalValue;
}

[System.Serializable]
public class ItemData : ILoader<string, Item>
{
    public List<Item> items = new List<Item>();

    public Dictionary<string, Item> MakeDict()
    {
        Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

        foreach (Item item in items)
        {
            itemDict.Add(item.name, item);
        }

        return itemDict;
    }
}
}