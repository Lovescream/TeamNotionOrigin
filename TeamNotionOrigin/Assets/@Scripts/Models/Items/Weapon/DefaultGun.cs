using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGun : Weapon
{
    protected override void Reload()
    {
        CurrentMag = (int)Owner.Status[StatType.MagazineCapacity].Value;
    }
}
