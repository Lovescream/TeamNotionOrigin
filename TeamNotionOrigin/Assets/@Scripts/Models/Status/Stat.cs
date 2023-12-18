using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {

    #region Properties

    public StatType Type { get; private set; }
    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Value { get; private set; }
    public float OriginValue { get; private set; }

    #endregion

    #region Fields

    // Collections.
    private List<StatModifier> _modifiers = new();

    // Callbacks.
    public event Action<Stat> OnChanged;

    #endregion

    public Stat(StatType type, float min = 0, float max = float.MaxValue, float value = 0) {
        this.Type = type;
        this.Min = min;
        this.Max = max;
        this.OriginValue = value;
    }

    public void SetValue(float value) {
        OriginValue = value;
        Value = GetModifyValue();
        OnChanged?.Invoke(this);
    }
    public void AddModifier(StatModifier modifier) {
        _modifiers.Add(modifier);
        Value = GetModifyValue();
        OnChanged?.Invoke(this);
    }
    public void RemoveModifier(StatModifier modifier) {
        _modifiers.Remove(modifier);
        Value = GetModifyValue();
        OnChanged?.Invoke(this);
    }



    private float GetModifyValue() {
        float value = OriginValue;
        for (int i = 0; i < _modifiers.Count; i++) {
            if (_modifiers[i].Type == StatModifierType.Add) value += _modifiers[i].Value;
            else if (_modifiers[i].Type == StatModifierType.Multiple) value *= _modifiers[i].Value;
        }
        value = Mathf.Clamp(value, Min, Max);
        return value;
    }
}