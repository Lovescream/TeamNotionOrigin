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
        if(CurrentMag > 0 && currentFireRate <= 0)
        {
            CurrentMag--;
            currentFireRate = Owner.Status[StatType.AttackSpeed].Value;
            for (int i = 0; i < 4; i++)
            {
                var dir = Quaternion.Euler(0, 0, Random.Range(-15f, 15f)) * Owner.LookDirection;
                Main.Object.SpawnProjectile<Bullet>(_bulletPivot.position, dir.normalized, 10f, Owner.gameObject.layer, Owner.Status[StatType.Damage].Value);
            }
        }
        else
        {
            TryReload();
        }
    }
}