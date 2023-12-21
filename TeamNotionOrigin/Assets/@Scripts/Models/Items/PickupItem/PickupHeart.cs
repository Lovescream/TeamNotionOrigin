using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeart : PickupItem
{
    protected override void OnPickedUp()
    {
        Owner.Hp += NumericalRatio;
    }
}
