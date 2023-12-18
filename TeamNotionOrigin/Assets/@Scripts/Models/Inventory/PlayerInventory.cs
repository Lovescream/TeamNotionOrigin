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
                GameObject.FindObjectOfType<Player>().Status.AddModifiers(value.Modifiers);     // TODO:: Find 제거
                _equippedWeapon = value;
                OnEquipChanged?.Invoke(_equippedWeapon);
            }
            else {
                GameObject.FindObjectOfType<Player>().Status.RemoveModifiers(value.Modifiers);  // TODO:: Find 제거
                OnEquipChanged.Invoke(_equippedWeapon);
                _equippedWeapon = null;
            }
        }
    }

    private Weapon _equippedWeapon;

}