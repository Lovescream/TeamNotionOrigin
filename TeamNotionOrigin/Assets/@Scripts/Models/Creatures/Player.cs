using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature {

    public PlayerInventory Inventory { get; private set; }

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        return true;
    }

    public override void SetInfo(CreatureData data) {
        base.SetInfo(data);

        Inventory = new();
    }

}