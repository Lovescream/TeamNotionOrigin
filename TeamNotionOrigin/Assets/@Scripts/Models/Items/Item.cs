using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public Data.Item Data { get; private set; }
    public int ID => Data.id;
    public Data.ItemType Type => Data.itemType;
    public string Name => Data.name;
    public List<StatModifier> Modifiers { get; private set; }

    public Item(Data.Item data) {
        this.Data = data;
        Modifiers = Data.modifiers.ConvertAll(x => x);
    }

}