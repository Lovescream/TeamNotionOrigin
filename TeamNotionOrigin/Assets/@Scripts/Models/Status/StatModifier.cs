using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {

    #region Properties

    public StatType Stat { get; private set; }
    public StatModifierType Type { get; private set; }
    public float Value { get; private set; }

    #endregion

    public StatModifier(StatType stat, StatModifierType type, float value) {
        this.Stat = stat;
        this.Type = type;
        this.Value = value;
    }
}