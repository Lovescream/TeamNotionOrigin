using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Monster : Creature
{
    public enum MonsterState
    {
        Idle,
        Aggro,
        Attack,
        Dead,
    }

    private Transform _target;
    private float _detectRange;
    private StateMachine<MonsterState> _fsm;

    public MonsterState CurrentState => _fsm.CurrentState;
    public float DetectRange { get => _detectRange; set => _detectRange = value; }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        _fsm.OnStateStay();
    }

    public void Initialize()
    {
        _fsm = new();
    }
}