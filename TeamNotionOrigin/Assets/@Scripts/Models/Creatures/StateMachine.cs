using System.Collections.Generic;
using System;
using UnityEngine;

public class StateMachine<T> : IStateMachine<T> where T : Enum
{
    private T _state;
    public T CurrentState => _state;

    private Dictionary<StateEvent, Dictionary<T, Action>> _events = new();

    public StateMachine()
    {
        for (int i = 0; i < Enum.GetValues(typeof(StateEvent)).Length; i++)
        {
            _events[(StateEvent)i] = new Dictionary<T, Action>();
            var enums = Enum.GetValues(typeof(T)) as T[];
            for (int j = 0; j < enums.Length; j++)
                _events[(StateEvent)i].Add(enums[j], null);
        }
    }

    public void OnStateEnter() => _events[StateEvent.Enter][_state]?.Invoke();
    public void OnStateExit() => _events[StateEvent.Exit][_state]?.Invoke();
    public void OnStateStay() => _events[StateEvent.Stay][_state]?.Invoke();

    public void StateTransition(T state)
    {
        if (state.Equals(_state))
            return;

        OnStateExit();
        _state = state;
        OnStateEnter();
    }

    public void BindEvent(T state, StateEvent @event, Action action)
    {
        _events[@event][state] += action;
    }
}