using System;

public interface IStateMachine<T> where T : Enum
{
    void OnStateEnter();
    void OnStateStay();
    void OnStateExit();
    void StateTransition(T state);
    void BindEvent(T state, StateEvent @event, Action action);
}

public enum StateEvent
{
    Enter,
    Stay,
    Exit,
}