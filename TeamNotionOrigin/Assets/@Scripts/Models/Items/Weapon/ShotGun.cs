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
        if(_currentMag > 0)
        {
            _currentMag--;
            float angle = 1.0f;
            float randomSpread = Random.Range(-5, 5);
            angle += randomSpread;

            for(int i = 0; i < 8; i++)
            {
                Main.Object.Spawn<Projectile>(1, _bulletPivot.position); //샷건 산탄 조정 추가 예정
            }        
        }
    }

    private Vector2 RotateVector2(Vector2 vector, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * vector;
    }
}
