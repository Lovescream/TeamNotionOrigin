using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    public float HpMax { get; protected set; }
    public float Attack { get; protected set; }
    public float Defense { get; protected set; }
    public float Speed { get; protected set; }

    public CreatureState State { get; protected set; }

    public float Hp {
        get => _hp;
        set {
            if (value <= 0) {
                _hp = 0;
                if (State != CreatureState.Dead) State = CreatureState.Dead;
            }
            _hp = value;
        }
    }

    private float _hp;

}

public enum CreatureState {
    Idle,
    Dead,
}