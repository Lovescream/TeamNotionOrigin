using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory {

    #region Properties

    public float Gold {
        get => _gold;
        set {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);
        }
    }
    public int Count => _items.Count;
    public int MaxCount => 99;      // TODO:: NO HARDCODING

    #endregion

    #region Fields

    private float _gold;

    // Collections.
    private List<Item> _items = new();

    // Callbacks.
    public event Action<float> OnGoldChanged;
    public event Action OnChanged;

    #endregion

    public void Add(Item item) {
        _items.Add(item);
        OnChanged?.Invoke();
    }
    public void Remove(Item item) {
        _items.Remove(item);
        OnChanged?.Invoke();
    }
    public Item GetItemIndex(int index) => index < _items.Count ? _items[index] : null;
    public Item GetItem(int key) => _items.Where(x => x.ID == key).FirstOrDefault();
}