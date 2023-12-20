using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    public override void Shoot()
    {
        if (currentFireRate>0 || currentReload > 0)
        {
            return;
        }
        if(CurrentMag > 0 || currentFireRate == 0)
        {
            CurrentMag--;
            for(int i = 0; i < 4; i++)
            {
                Main.Object.Spawn<SpreadBullet>(1, _bulletPivot.position);
            }
        }
        else
        {
            TryReload();
        }
    }
}