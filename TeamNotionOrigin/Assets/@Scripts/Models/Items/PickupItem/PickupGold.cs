using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    public int Amount { get; private set; }

    protected override void OnPickedUp()
    {
        Owner.Inventory.Gold += Amount;
        Debug.Log(Owner.Inventory.Gold);
    }

    public void SetInfo(int amount) {
        Initialize();
        this.Amount = amount;
        this._spriter.sprite = Main.Resource.Load<Sprite>($"Pickup_0.sprite");
    }
}
