using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : PickupItem
{
    protected override void OnPickedUp()
    {
        if (Owner.Inventory is not PlayerInventory inventory) return;
        inventory.EquippedWeapon.CurrentAmmo += (int)NumericalRatio;
    }
}