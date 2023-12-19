using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupItem
{
    protected override void OnPickedUp()
    {
        Debug.Log("총알 획득");
    }
}