using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{


    protected override void OnPickedUp()
    {
        Main.Object.Player.Inventory.Gold += 100;
    }
}
