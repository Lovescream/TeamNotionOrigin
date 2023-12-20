using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GunInfo : UI_Base {

    #region Enums

    enum Texts {
        txtGun,
        txtAmmo,
    }
    enum Images {
        imgGun,
    }
    enum Objects {
        CurrentMag,
    }

    #endregion

    public Weapon CurrentWeapon { get; private set; }

    private List<UI_Ammo> _ammos = new();

    private Transform _magTransform;

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(Objects));

        _magTransform = GetObject((int)Objects.CurrentMag).transform;
        
        return true;
    }

    public void SetInfo(Weapon weapon) {
        Initialize();

        if (CurrentWeapon != null) {
            CurrentWeapon.OnReload -= RefreshOnReload;
            CurrentWeapon.OnShot -= RefreshOnShot;
            CurrentWeapon.OnChangedCurrentAmmo -= RefreshAmmo;
            CurrentWeapon.Owner.Status[StatType.MaxBulletAmount].OnChanged -= RefreshAmmo;
        }

        CurrentWeapon = weapon;
        if (weapon == null) return;
        GetImage((int)Images.imgGun).sprite = Main.Resource.Load<Sprite>($"{weapon.Type}_{weapon.ID}.sprite");
        GetText((int)Texts.txtGun).text = weapon.Name;

        CurrentWeapon.OnReload += RefreshOnReload;
        CurrentWeapon.OnShot += RefreshOnShot;
        CurrentWeapon.OnChangedCurrentAmmo += RefreshAmmo;
        CurrentWeapon.Owner.Status[StatType.MaxBulletAmount].OnChanged += RefreshAmmo;

        _magTransform.gameObject.DestroyChilds();
        _ammos.Clear();
        for (int i = 0; i < CurrentWeapon.CurrentMag; i++) {
            _ammos.Add(Main.UI.CreateSubItem<UI_Ammo>(_magTransform));
        }
    }
    public void RefreshAmmo(Stat ammo) {
        GetText((int)Texts.txtAmmo).text = $"{CurrentWeapon.CurrentAmmo}/{ammo.Value}";
    }
    public void RefreshAmmo() {
        GetText((int)Texts.txtAmmo).text = $"{CurrentWeapon.CurrentAmmo}/{CurrentWeapon.Owner.Status[StatType.MaxBulletAmount].Value}";
    }
    public void RefreshOnReload(int capacity) {
        _magTransform.gameObject.DestroyChilds();
        _ammos.Clear();
        for (int i = 0; i < capacity; i++) {
            _ammos.Add(Main.UI.CreateSubItem<UI_Ammo>(_magTransform));
        }
    }
    public void RefreshOnShot() {
        if (_ammos.Count == 0) return;
        UI_Ammo ammo = _ammos[_ammos.Count - 1];
        _ammos.Remove(ammo);
        Main.Resource.Destroy(ammo.gameObject);
    }
    
}