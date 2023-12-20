using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class PassiveItem : Item {

    #region Properties

    public bool Stackable => (Data as Passive).stackable;
    public int MaxStack => (Data as Passive).maxStack;

    #endregion

    #region Initialize / Set

    public override void SetInfo(Data.Item data) {
        base.SetInfo(data);
    }

    protected override void SetModifiers() {
        Passive data = Data as Passive;

        Modifiers = new();

        switch (data.effectType) {
            case EffectType.DamageBoost:
                Modifiers.Add(new(StatType.Damage, StatModifierType.Add, data.numericalValue)); break;
            case EffectType.SpeedBoost:
                Modifiers.Add(new(StatType.Speed, StatModifierType.Add, data.numericalValue)); break;
            case EffectType.BulletSizeBoost:
                Modifiers.Add(new(StatType.BulletSizeX, StatModifierType.Add, data.numericalValue));
                Modifiers.Add(new(StatType.BulletSizeY, StatModifierType.Add, data.numericalValue));
                Modifiers.Add(new(StatType.BulletSizeZ, StatModifierType.Add, data.numericalValue));
                break;
            case EffectType.MagazineBoost:
                Modifiers.Add(new(StatType.MagazineCapacity, StatModifierType.Add, data.numericalValue));
                break;
        }
    }

    #endregion

}