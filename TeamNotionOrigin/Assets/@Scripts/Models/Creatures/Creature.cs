using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public CreatureData Data { get; protected set; }
    public CreatureState State { get; protected set; }
    public Status Status { get; protected set; } = new();

    public float Hp {
        get => _hp;
        set {
            if (value <= 0) {
                _hp = 0;
                if (State != CreatureState.Dead) State = CreatureState.Dead;
            }
            else if (value >= Status[StatType.Hp].Value) {
                _hp = Status[StatType.Hp].Value;
            }
            _hp = value;
        }
    }

    private float _hp;
    private bool _initialized;

    public virtual bool Initialize() {
        if (_initialized) return false;
        _initialized = true;

        return true;
    }

    public virtual void SetInfo(CreatureData data) {
        Data = data;
        Status = new();
        Hp = Status[StatType.Hp].Value;
    }

}

public enum CreatureState {
    Idle,
    Dead,
}