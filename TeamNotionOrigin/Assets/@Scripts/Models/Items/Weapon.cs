using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : Item {

    #region Fields

    private Status _stat;
    private int _currentAmmo = 100;    // 현재 탄환 수
    private int _currentMag = 10;     // 현재 탄창에 탄환 수
    public Transform _bulletPivot;
    private bool isReloading = false;

    #endregion

    public override void SetInfo(Data.Item data) {
        base.SetInfo(data);
        _stat = new(Data as Data.Weapon);
        _currentAmmo = 100;
        _currentMag = 10;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Shoot(transform.position, Vector2.right);
            Debug.Log("Shoot");
        }
    }
    public void Shoot(Vector2 startPosition, Vector2 direction)
    {
        _currentMag = 10;
        if (isReloading)
            return;
        if (_currentMag > 0)
        {
            Main.Object.Spawn<Projectile>(1, _bulletPivot.position);
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