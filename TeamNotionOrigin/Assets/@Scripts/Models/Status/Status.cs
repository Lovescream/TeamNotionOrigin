using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status {
    private Dictionary<StatType, Stat> _stats;

    public Stat this[StatType type] => _stats[type];

    public Status() {
        // TODO:: NO HARDCODING!
        _stats = new() {
            [StatType.Hp] = new(StatType.Hp, 0, 1000, 100),
            [StatType.Damage] = new(StatType.Damage, 0, 1000, 10),
            [StatType.Defense] = new(StatType.Defense, 0, 1000, 2),
            [StatType.MoveSpeed] = new(StatType.MoveSpeed, 0, 1000, 2),
            [StatType.CriticalChance] = new(StatType.CriticalChance, 0, 100, 0),
            [StatType.CriticalBonus] = new(StatType.CriticalBonus, 0, 1000, 50),
            [StatType.DamageMultiplier] = new(StatType.DamageMultiplier, 0, 100, 1),
            [StatType.AttackSpeed] = new(StatType.AttackSpeed, 0, 100, 1),
            [StatType.MagazineCapacity] = new(StatType.MagazineCapacity, 0, 1000, 5),
            [StatType.MaxAmmo] = new(StatType.MaxAmmo, 0, 1000, 100),
            [StatType.ReloadTime] = new(StatType.ReloadTime, 0, 100, 1),
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
    MoveSpeed,              // 이동속도: 초당 이동할 수 있는 거리
    CriticalChance,         // 치명타 확률: 0 ~ 100 (%)
    CriticalBonus,          // 치명타 보너스
    DamageMultiplier,       // 공격 계수
    AttackSpeed,            // 발사 속도: 초당 발사할 수 있는 총알 수
    MagazineCapacity,       // 탄창 수: 한 탄창에 들어갈 수 있는 총알 수
    MaxAmmo,                // 최대 탄환 수: 보유할 수 있는 최대 총알 수
    ReloadTime,             // 장전 시간: 장전하는 데 걸리는 시간
    COUNT // Stat 개수
}

public enum StatModifierType {
    Add,
    Multiple,
}