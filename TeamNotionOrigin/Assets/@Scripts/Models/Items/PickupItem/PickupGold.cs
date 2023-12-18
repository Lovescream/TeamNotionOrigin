using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    public PickupGold(Data.Item data) : base(data)
    {
    }

    protected override void OnPickedUp()
    {
        Main.Game.Player.gold += 100;
    }
}
