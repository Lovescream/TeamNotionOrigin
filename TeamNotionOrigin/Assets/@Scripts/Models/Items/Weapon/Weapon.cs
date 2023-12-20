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
            if (value >= Owner.Status[StatType.MaxBulletAmount].Value) {
                _currentAmmo = (int)Owner.Status[StatType.MaxBulletAmount].Value;
            }
            else
                _currentAmmo = value;
            OnChangedCurrentAmmo?.Invoke();
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
    protected float currentFireRate;
    protected float currentReload;

    private bool _isFirstEquip;

    public event Action OnChangedCurrentAmmo;
    public event Action<int> OnReload;
    public event Action OnShot;

    #endregion

    #region MonoBehaviours
    private void Start()
    {
        currentFireRate = 0;
        currentReload = 0;
    }

    private void Update()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
        if(currentReload > 0)
        {
            currentReload -= Time.deltaTime;
        }
    }

    private void OnDisable() {
        if (Owner == null) return;
        (Owner.Inventory as PlayerInventory).OnEquip -= OnEquip;
        (Owner.Inventory as PlayerInventory).OnUnEquip -= OnUnEquip;
    }

    #endregion

    #region Initialize / Set

    public override void SetInfo(Data.Item data) {
        base.SetInfo(data);
    }

    protected override void SetModifiers() {
        Data.Weapon data = Data as Data.Weapon;

        Modifiers = new() {
            new(StatType.Damage, StatModifierType.Multiple, data.damage),
            new(StatType.AttackSpeed, StatModifierType.Multiple, data.attackSpeed),
            new(StatType.BulletSizeX, StatModifierType.Override, data.bulletSizeX),
            new(StatType.BulletSizeY, StatModifierType.Override, data.bulletSizeY),
            new(StatType.BulletSizeZ, StatModifierType.Override, data.bulletSizeZ),
            new(StatType.ReloadTime, StatModifierType.Add, data.reloadTime),
            new(StatType.Critical, StatModifierType.Add, data.critical),
            new(StatType.MaxBulletAmount, StatModifierType.Add, data.maxBulletAmount),
            new(StatType.MagazineCapacity, StatModifierType.Add, data.magazineCapacity)
        };
    }

    public override void SetOwner(Creature creature) {
        if (creature == null) {
            (Owner.Inventory as PlayerInventory).OnEquip -= OnEquip;
            (Owner.Inventory as PlayerInventory).OnUnEquip -= OnUnEquip;
        }
        base.SetOwner(creature);
        if (Owner == null) return;
        (Owner.Inventory as PlayerInventory).OnEquip += OnEquip;
        (Owner.Inventory as PlayerInventory).OnUnEquip += OnUnEquip;
    }

    #endregion 

    private void OnEquip(Weapon weapon) {
        if (weapon != this) return;
        if (!_isFirstEquip) {
            CurrentMag = (int)Owner.Status[StatType.MagazineCapacity].Value;
            CurrentAmmo = (int)Owner.Status[StatType.MaxBulletAmount].Value;
            Debug.Log($"[Weapon: {Name}] OnEquip({Name}): 첫 장착. (CurrentMag = {CurrentMag}) (CurrentAmmo = {CurrentAmmo})");
            _isFirstEquip = true;
        }
        (Main.Scene.CurrentScene.UI as UI_GameScene).GunInfo.SetInfo(this);
    }
    private void OnUnEquip(Weapon weapon) {
        if (weapon != this) return;
    }

    public virtual void Shoot()
    {
        if (currentFireRate > 0 || currentReload > 0) return;
        if (CurrentMag > 0 && currentFireRate <= 0)
        {
            CurrentMag--;
            currentFireRate = Owner.Status[StatType.AttackSpeed].Value;
            Main.Object.SpawnProjectile<Bullet>(_bulletPivot.position, Owner.LookDirection, 10f, Owner.gameObject.layer, Owner.Status[StatType.Damage].Value);
            OnShot?.Invoke();
        }
        else if(CurrentMag == 0)
        {
            TryReload();
        }
    }

    protected void TryReload()
    {
        if (currentReload>0)
            return;
        if (CurrentAmmo > 0)
        {
            currentReload = Owner.Status[StatType.ReloadTime].Value;
            Reload();
        }
        else
        {
            Debug.Log("탄환이 부족합니다.");
        }
    }
    protected virtual void Reload()
    {
        if (CurrentAmmo >= (int)Owner.Status[StatType.MagazineCapacity].Value)
        {
            CurrentAmmo -= (int)Owner.Status[StatType.MagazineCapacity].Value;
            CurrentMag = (int)Owner.Status[StatType.MagazineCapacity].Value;

        }
        else
        {
            CurrentMag = CurrentAmmo;
            CurrentAmmo = 0;
        }
        OnReload?.Invoke(CurrentMag);
    }

    public void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _spriter.flipY = Mathf.Abs(rotZ) > 90f;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }
}