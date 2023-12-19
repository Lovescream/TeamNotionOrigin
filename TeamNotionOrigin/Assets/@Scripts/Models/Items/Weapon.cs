using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : Item {
    
    #region Fields

    private Status _stat = new();
    private int _currentAmmo;    // 현재 탄환 수
    private int _currentMag;     // 현재 탄창에 탄환 수
    public Transform _bulletPivot;
    private bool isReloading = false;

    #endregion

    public Weapon(Data.Item data) : base(data) {
        _currentAmmo = (int)_stat[StatType.MaxBulletAmount].Value;
        _currentMag = (int)_stat[StatType.MagazineCapacity].Value;
    }

    public void Shoot(Vector2 startPosition, Vector2 direction)
    {
        if (isReloading)
            return;
        if (_currentMag > 0)
        {
            Projectile bullet = Main.Object.Spawn<Projectile>(1, _bulletPivot.position);
            bullet.transform.position = startPosition;
            Bullet _bullet = bullet.GetComponent<Bullet>();
            _bullet.InitializeAttack(direction, bullet);
        }
        else
        {
            TryReload();
        }
    }

    private void TryReload()
    {
        if (isReloading)
            return;
        if (_currentAmmo > (int)_stat[StatType.MagazineCapacity].Value)
        {
            Reload();
        }
        else
        {
            Debug.Log("탄환이 부족합니다.");
        }
    }
    private void Reload()
    {
        isReloading = true;
        Task.Delay((int)_stat[StatType.ReloadTime].Value);
        _currentAmmo -= (int)_stat[StatType.MagazineCapacity].Value;
        _currentMag = (int)_stat[StatType.MaxBulletAmount].Value;
        isReloading = false;
    }
}