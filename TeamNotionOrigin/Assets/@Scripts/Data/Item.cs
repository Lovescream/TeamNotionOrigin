using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public enum ItemType
    {
        Pickup = 0,//단순 소모성, 재화
        Weapon = 1,//상점, 방 클리어 보상, 보스
        Passive = 2,//상점, 방 클리어 보상, 보스
    }
    public enum ItemRarity
    {
        Normal = 0,
        Rare = 1,
        Unique = 2,
        Legendary = 3,
        Basic = 4,
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
        public Sprite itemSprite;
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

            AddDictToItemDict(itemDict, weaponData.weapons, ItemType.Weapon);
            AddDictToItemDict(itemDict, pickupData.pickups, ItemType.Pickup);
            AddDictToItemDict(itemDict, passiveData.passives, ItemType.Passive);

            return itemDict;
        }

        public void AddDictToItemDict<T>(Dictionary<ItemType, Dictionary<int, Data.Item>> itemDict, List<T> items, ItemType itemType) where T : Data.Item
        {
            foreach (T item in items)
            {
                if (!itemDict.ContainsKey(itemType))
                {
                    itemDict.Add(itemType, new Dictionary<int, Data.Item>());
                }
                if (item.itemType == ItemType.Weapon)
                {
                    Debug.Log(Main.Resource.Load<Sprite>($"Weapons_{item.id-1}").name);
                    item.itemSprite = Main.Resource.Load<Sprite>($"Weapons_{item.id-1}");
                }
                itemDict[itemType].Add(item.id, item);
            }
        }
    }
}