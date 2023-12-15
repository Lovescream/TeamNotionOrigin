using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature, IStateMachine<Monster.MonsterState>
{
    private Player _player;
    private float _detectRange;
    private Dictionary<MonsterState, Action> onStateEnter = new();
    private Dictionary<MonsterState, Action> onStateExit = new();
    private Dictionary<MonsterState, Action> onStateStay = new();
    private MonsterState _state;
    public enum MonsterState
    {
        Idle,
        Aggro,
        Attack,
        Dead,
    }

    public float DetectRange { get => _detectRange; set => _detectRange = value; }
    public MonsterState CurrentState => _state;

    public void OnStateEnter(MonsterState state) => onStateEnter[state]?.Invoke();
    public void OnStateExit(MonsterState state) => onStateExit[state]?.Invoke();
    public void OnStateStay(MonsterState state) => onStateStay[state]?.Invoke();

    public void StateTransition(MonsterState state)
    {
        OnStateExit(_state);
        _state = state;
        OnStateEnter(_state);
    }
}