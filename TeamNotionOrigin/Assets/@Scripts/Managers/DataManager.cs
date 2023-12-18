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
    public Dictionary<ItemType, Dictionary<int, Data.Item>> ItemDict { get; private set; } = new Dictionary<ItemType, Dictionary<int, Data.Item>>();
    public Dictionary<int, Data.Monster> MonsterDict { get; private set; } = new Dictionary<int, Data.Monster>();
    public Dictionary<int, Data.Player> PlayerDict { get; private set; } = new Dictionary<int, Data.Player>();
    public void Init()
    {
        Data.ItemData itemData = new Data.ItemData();
        ItemDict = itemData.MakeDict();
        MonsterDict = LoadJson<Data.MonsterData, int, Data.Monster>("monsterData").MakeDict();
        PlayerDict = LoadJson<Data.PlayerData, int, Data.Player>("playerData").MakeDict();
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