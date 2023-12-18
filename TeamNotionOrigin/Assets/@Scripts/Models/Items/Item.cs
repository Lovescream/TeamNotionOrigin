using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public ItemData Data { get; private set; }
    public int ID => Data.id;
    public ItemType Type => Data.type;
    public string Name => Data.name;
    public List<StatModifier> Modifiers { get; private set; }

    public Item(ItemData data) {
        this.Data = data;
        Modifiers = Data.modifiers.ConvertAll(x => x);
    }

}