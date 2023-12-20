using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    protected override void OnPickedUp()
    {
        Owner.Inventory.Gold += 100;
    }
}
