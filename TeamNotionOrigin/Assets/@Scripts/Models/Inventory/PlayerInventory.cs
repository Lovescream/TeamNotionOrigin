using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory {

    public event Action<Weapon> OnEquipChanged;

    public Weapon EquippedWeapon {
        get => _equippedWeapon;
        set {
            if (value == _equippedWeapon) return;
            else if (value != null) {
                Owner.Status.AddModifiers(value.Modifiers);
                _equippedWeapon = value;
                OnEquipChanged?.Invoke(_equippedWeapon);
            }
            else {
                Owner.Status.RemoveModifiers(value.Modifiers);
                OnEquipChanged.Invoke(_equippedWeapon);
                _equippedWeapon = null;
            }
        }
    }

    private readonly int MaxWeaponCount = 3;
    private List<Weapon> _weapons = new();
    private Weapon _equippedWeapon;

    public PlayerInventory(Creature creature) : base(creature) { }

    public bool AddWeapon(Weapon weapon) {
        if (_weapons.Count >= MaxWeaponCount) return false;
        _weapons.Add(weapon);
        if (EquippedWeapon == null) EquipWeapon(0);
        return true;
    }
    public bool RemoveWeapon(Weapon weapon) {
        if (_weapons.Count == 0 || !_weapons.Contains(weapon)) return false;
        return _weapons.Remove(weapon);
    }
    public void EquipWeapon(int index) {
        if (index >= _weapons.Count) return;

        EquippedWeapon = _weapons[index];
    }
    public void EquipNextWeapon() {
        if (EquippedWeapon == null) {
            if (_weapons.Count == 0) return;
            EquipWeapon(0);
            return;
        }
        int index = _weapons.IndexOf(EquippedWeapon) + 1;
        if (index >= _weapons.Count) index = 0;
        EquipWeapon(index);
    }
    public void EquipPrevWeapon() {
        if (EquippedWeapon == null) {
            if (_weapons.Count == 0) return;
            EquipWeapon(0);
            return;
        }
        int index = _weapons.IndexOf(EquippedWeapon) - 1;
        if (index <= 0) index = _weapons.Count - 1;
        EquipWeapon(index);
    }
}