using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupItem
{
    public PickupAmmo(Data.Item data) : base(data)
    {
    }

    protected override void OnPickedUp()
    {
        Debug.Log("총알 획득");
    }
}