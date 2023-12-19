using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : Item {

    #region Fields
    Player player;
    protected int _currentAmmo = 100;    // 현재 탄환 수
    protected int _currentMag = 10;     // 현재 탄창에 탄환 수
    public Transform _bulletPivot;
    protected bool isReloading = false;

    #endregion

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        _currentMag = 10;//Player에서 데이터 획득;
        _currentAmmo = 100;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) //플레이어에서 실행하는 걸로 옮길 예정
        {
            Debug.Log(_currentMag);
            Shoot();
            Debug.Log("Shoot");
        }
    }
    protected virtual void Shoot()
    {
        if (isReloading)
            return;
        if (_currentMag > 0)
        {
            _currentMag --;
            Main.Object.Spawn<Projectile>(1, _bulletPivot.position);
        }
        else
        {
            TryReload();
        }
    }

    protected void TryReload()
    {
        if (isReloading)
            return;
        if (_currentAmmo > 10)
        {
            Reload();
        }
        else
        {
            Debug.Log("탄환이 부족합니다.");
        }
    }
    protected void Reload()
    {
        isReloading = true;
        Task.Delay(1000);
        _currentAmmo -= 10;
        _currentMag = 10;
        isReloading = false;
    }
}