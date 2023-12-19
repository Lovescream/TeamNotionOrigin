using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    #region Properties

    public Creature Owner { get; set; }
    public Data.Item Data { get; private set; }
    public int ID => Data.id;
    public Data.ItemType Type => Data.itemType;
    public Data.ItemRarity Rarity => Data.itemRarity;
    public string Name => Data.name;
    public string Description => Data.description;
    public float Cost => Data.cost; // TODO::

    public List<StatModifier> Modifiers { get; protected set; }

    #endregion

    #region Fields

    private bool _isInitialized;

    #endregion

    #region MonoBehaviours

    protected virtual void Awake() {
        Initialize();
    }

    #endregion

    #region Initialize / Set

    public virtual bool Initialize() {
        if (_isInitialized) return false;

        return true;
    }

    public virtual void SetInfo(Data.Item data) {
        Initialize();

        this.Data = data;
        SetModifiers();
    }

    protected virtual void SetModifiers() {
        Modifiers = new();
    }

    #endregion
}