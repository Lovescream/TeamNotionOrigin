using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data { 

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
        public ItemRarity itemRarity;
        public ItemType itemType;
        public string name;
        public string description;
    }

    [System.Serializable]
    public class ItemData
    { 
        public Dictionary<ItemType, List<Data.Item>> MakeDict()
        {
            Dictionary<ItemType, List<Data.Item>> itemDict = new Dictionary<ItemType, List<Data.Item>>();

            WeaponData weaponData = Main.Data.LoadJsonList<WeaponData>("weaponData");
            PickupData pickupData = Main.Data.LoadJsonList<PickupData>("pickupData");
            PassiveData passiveData = Main.Data.LoadJsonList<PassiveData>("passiveData");

            AddListToDict(itemDict, weaponData.weapons, ItemType.Weapon);
            AddListToDict(itemDict, pickupData.pickups, ItemType.Pickup);
            AddListToDict(itemDict, passiveData.passives, ItemType.Passive);

            return itemDict;
        }

        public void AddListToDict<T>(Dictionary<ItemType, List<Data.Item>> itemDict, List<T> items, ItemType itemType) where T : Data.Item
        {
            foreach (T item in items)
            {
                if (!itemDict.ContainsKey(itemType))
                {
                    itemDict.Add(itemType, new List<Data.Item>());
                }
                itemDict[itemType].Add(item);
            }
        }
    }
}