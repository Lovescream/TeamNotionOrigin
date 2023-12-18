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
    public enum ItemRarity
    {
        Normal,
        Rare,
        Unique,
        Legendary,
    }

    [System.Serializable]
    public class Item
    {
        public int id;
        public ItemRarity itemRarity;
        public ItemType itemType;
        public string name;
        public string description;
        public float cost;
        public List<StatModifier> modifiers;
    }

    [System.Serializable]
    public class ItemData
    { 
        public Dictionary<ItemType, Dictionary<int, Data.Item>> MakeDict()
        {
            Dictionary<ItemType, Dictionary<int, Data.Item>> itemDict = new Dictionary<ItemType, Dictionary<int, Data.Item>>();

            WeaponData weaponData = Main.Data.LoadJsonList<WeaponData>("weaponData");
            PickupData pickupData = Main.Data.LoadJsonList<PickupData>("pickupData");
            PassiveData passiveData = Main.Data.LoadJsonList<PassiveData>("passiveData");

            AddListToDict(itemDict, weaponData.weapons, ItemType.Weapon);
            AddListToDict(itemDict, pickupData.pickups, ItemType.Pickup);
            AddListToDict(itemDict, passiveData.passives, ItemType.Passive);

            return itemDict;
        }

        public void AddListToDict<T>(Dictionary<ItemType, Dictionary<int, Data.Item>> itemDict, List<T> items, ItemType itemType) where T : Data.Item
        {
            foreach (T item in items)
            {
                if (!itemDict.ContainsKey(itemType))
                {
                    itemDict.Add(itemType, new Dictionary<int, Data.Item>());
                }
                itemDict[itemType].Add(item.id, item);
            }
        }
    }
}