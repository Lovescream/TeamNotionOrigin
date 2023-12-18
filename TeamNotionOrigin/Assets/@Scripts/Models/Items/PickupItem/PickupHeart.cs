using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeart : PickupItem
{
    public PickupHeart(Data.Item data) : base(data)
    {
    }

    protected override void OnPickedUp()
    {
        Main.Game.Player.hp += 10;
    }
}
