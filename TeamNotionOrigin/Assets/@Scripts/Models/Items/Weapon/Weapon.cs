using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : Item {

    #region Properties

    public BulletType BulletType => (Data as Data.Weapon).bulletType;

    public int CurrentAmmo {
        get => _currentAmmo;
        set {
            _currentAmmo = value;
        }
    }
    public int CurrentMag {
        get => _currentMag;
        set {
            _currentMag = value;
        }
    }

    #endregion

    #region Fields

    protected int _currentAmmo;
    protected int _currentMag;
    public Transform _bulletPivot;
    protected bool isReloading = false;
    #endregion

    #region MonoBehaviours

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) //플레이어에서 실행하는 걸로 옮길 예정
        {
            Debug.Log(CurrentMag);
            Shoot();
            Debug.Log("Shoot");
        }
    }

    #endregion

    #region Initialize / Set

    public override void SetInfo(Data.Item data) {
        base.SetInfo(data);
        CurrentMag = (int)Owner.Status[StatType.MagazineCapacity].Value;
        CurrentAmmo = (int)Owner.Status[StatType.MaxBulletAmount].Value;
    }

    protected override void SetModifiers() {
        Data.Weapon data = Data as Data.Weapon;

        Modifiers = new() {
            new(StatType.Damage, StatModifierType.Add, data.damage),
            new(StatType.AttackSpeed, StatModifierType.Add, data.attackSpeed),
            new(StatType.BulletSizeX, StatModifierType.Override, data.bulletSizeX),
            new(StatType.BulletSizeY, StatModifierType.Override, data.bulletSizeY),
            new(StatType.BulletSizeZ, StatModifierType.Override, data.bulletSizeZ),
            new(StatType.ReloadTime, StatModifierType.Add, data.reloadTime),
            new(StatType.Critical, StatModifierType.Add, data.critical),
            new(StatType.MaxBulletAmount, StatModifierType.Add, data.maxBulletAmount),
            new(StatType.MagazineCapacity, StatModifierType.Add, data.magazineCapacity)
        };
    }

    #endregion 

    protected virtual void Shoot()
    {
        if (isReloading)
            return;
        if (CurrentMag > 0)
        {
            CurrentMag--;
            Main.Object.Spawn<Projectile>(1, _bulletPivot.position);
            Task.Delay((int)Owner.Status[StatType.AttackSpeed].Value);
        }
        else if(CurrentMag == 0)
        {
            TryReload();
        }
    }

    protected void TryReload()
    {
        if (isReloading)
            return;
        if (CurrentAmmo > 0)
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
        if (CurrentAmmo >= (int)Owner.Status[StatType.MagazineCapacity].Value)
        {
            Task.Delay((int)Owner.Status[StatType.ReloadTime].Value);
            CurrentAmmo -= (int)Owner.Status[StatType.MagazineCapacity].Value;
            CurrentMag = (int)Owner.Status[StatType.MagazineCapacity].Value;

        }
        else
        {
            Task.Delay((int)Owner.Status[StatType.ReloadTime].Value);
            CurrentMag = CurrentAmmo;
            CurrentAmmo = 0;
        }

        isReloading = false;
    }
}