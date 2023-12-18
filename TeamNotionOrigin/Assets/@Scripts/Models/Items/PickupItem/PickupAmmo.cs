using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupItem
{
    [SerializeField] private int _ammo;
    public PickupAmmo(ItemData data) : base(data)
    {
        
    }

    protected override void OnPickedUp(GameObject receiver)
    {
        //탄약 회복;
    }
}
