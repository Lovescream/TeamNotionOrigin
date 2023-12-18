using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Data;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
public interface ILoaderList<Value>
{
    List<Value> MakeList();
}
public class DataManager
{
    public Dictionary<ItemType, List<Data.Item>> ItemDict { get; private set; } = new Dictionary<ItemType, List<Data.Item>>();
    public Dictionary<string, Data.Monster> MonsterDict { get; private set; } = new Dictionary<string, Data.Monster>();
    public Dictionary<PlayerType, Data.Player> PlayerDict { get; private set; } = new Dictionary<PlayerType, Data.Player>();

    public List<Data.Weapon> Weapons { get; private set; } = new List<Data.Weapon>();
    public void Init()
    {
        Data.ItemData itemData = new Data.ItemData();
        ItemDict = itemData.MakeDict();
        MonsterDict = LoadJson<Data.MonsterData, string, Data.Monster>("monsterData").MakeDict();
        PlayerDict = LoadJson<Data.PlayerData, PlayerType, Data.Player>("playerData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string address) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Main.Resource.Load<TextAsset>(address);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }


    public T LoadJsonList<T>(string address)
    {
        TextAsset textAsset = Main.Resource.Load<TextAsset>(address);
        return JsonUtility.FromJson<T>(textAsset.text);
    }
}