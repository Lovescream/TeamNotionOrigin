using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status {
    private Dictionary<StatType, Stat> _stats;
    public Stat this[StatType type] => _stats[type];

    public Status(Data.Creature data) {
        _stats = new() {
            [StatType.Hp] = new(StatType.Hp, 0, 1000, data.maxHp),
            [StatType.Damage] = new(StatType.Damage, 0, 1000, data.damage),
            [StatType.Defense] = new(StatType.Defense, 0, 1000, data.defence),
            [StatType.Speed] = new(StatType.Speed, 0, 1000, data.speed),
            [StatType.Critical] = new(StatType.Critical, 0, 100, (data is Data.Player player) ? player.critical : 0),
            [StatType.AttackSpeed] = new(StatType.AttackSpeed, 0, 100, data.attackSpeed),
            [StatType.MagazineCapacity] = new(StatType.MagazineCapacity, 0, 1000),
            [StatType.MaxBulletAmount] = new(StatType.MaxBulletAmount, 0, 1000),
            [StatType.ReloadTime] = new(StatType.ReloadTime, 0, 100),
            [StatType.BulletSizeX] = new(StatType.BulletSizeX, 0, 100),
            [StatType.BulletSizeY] = new(StatType.BulletSizeY, 0, 100),
            [StatType.BulletSizeZ] = new(StatType.BulletSizeZ, 0, 0),
            [StatType.COUNT] = new(StatType.COUNT, 0, 100, Enum.GetValues(typeof(StatType)).Length - 1),
        };
    }
    public Status(Data.Weapon data) {
        _stats = new() {
            [StatType.Damage] = new(StatType.Damage, 0, 1000, data.damage),
            [StatType.AttackSpeed] = new(StatType.AttackSpeed, 0, 100, data.attackSpeed),
            [StatType.BulletSizeX] = new(StatType.BulletSizeX, 0, 100, data.bulletSizeX),
            [StatType.BulletSizeY] = new(StatType.BulletSizeY, 0, 100, data.bulletSizeY),
            [StatType.BulletSizeZ] = new(StatType.BulletSizeZ, 0, 100, data.bulletSizeZ),
            [StatType.ReloadTime] = new(StatType.ReloadTime, 0, 100, data.reloadTime),
            [StatType.Critical] = new(StatType.Critical, 0, 100, data.critical),
            [StatType.MaxBulletAmount] = new(StatType.MaxBulletAmount, 0, 1000, data.maxBulletAmount),
        };
    }

    public void AddModifiers(List<StatModifier> modifiers) {
        for (int i = 0; i < modifiers.Count; i++) {
            this[modifiers[i].Stat].AddModifier(modifiers[i]);
        }
    }
    public void RemoveModifiers(List<StatModifier> modifiers) {
        for (int i = 0; i < modifiers.Count; i++) {
            this[modifiers[i].Stat].RemoveModifier(modifiers[i]);
        }
    }
}

public enum StatType {
    Hp,                     // 생명력
    Damage,                 // 피해량
    Defense,                // 방어력
    Speed,                  // 이동속도: 초당 이동할 수 있는 거리
    Critical,               // 치명타 확률: 0 ~ 100 (%)
    AttackSpeed,            // 발사 속도: 초당 발사할 수 있는 총알 수
    MagazineCapacity,       // 탄창 수: 한 탄창에 들어갈 수 있는 총알 수
    MaxBulletAmount,        // 최대 탄환 수: 보유할 수 있는 최대 총알 수
    ReloadTime,             // 장전 시간: 장전하는 데 걸리는 시간
    BulletSizeX,            //탄 x사이즈
    BulletSizeY,            //탄 y사이즈
    BulletSizeZ,
    COUNT                   // Stat 개수
}

public enum StatModifierType {
    Add,
    Multiple,
}