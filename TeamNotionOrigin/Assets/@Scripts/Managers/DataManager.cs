using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
    public Dictionary<string, Data.Item> ItemDict { get; private set; } = new Dictionary<string, Data.Item>();
    public void Init()
    {
        ItemDict = LoadJson<Data.ItemData, string, Data.Item>("item").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string address) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Main.Resource.Load<TextAsset>(address);
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}