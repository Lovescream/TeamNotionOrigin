using System;

public interface IStateMachine<T> where T : Enum
{
    void OnStateEnter(T state);
    void OnStateStay(T state);
    void OnStateExit(T state);
    void StateTransition(T state);
}