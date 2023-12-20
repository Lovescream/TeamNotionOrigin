using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    protected override void Shoot()
    {
        if (isReloading)
        {
            return;
        }
        if(CurrentMag > 0)
        {
            CurrentMag--;
            for(int i = 0; i < 8; i++)
            {
                Main.Object.Spawn<Projectile>(1, _bulletPivot.position); //샷건 산탄 조정 추가 예정
            }
        }
        else
        {
            TryReload();
        }
    }
}