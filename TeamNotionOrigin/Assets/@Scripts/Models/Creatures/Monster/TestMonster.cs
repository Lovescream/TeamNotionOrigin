using System.Runtime.CompilerServices;
using UnityEngine;

public class TestMonster : Monster
{
    public enum State
    {
        Idle,
        Aggro,
        Attack,
        Dead,
    }

    public State CurrentState => _fsm.CurrentState;

    private StateMachine<State> _fsm;
    private MonsterPathFinder _pathFinder;

    // test code
    [SerializeField] private Vector2 _testTarget;

    private void Update()
    {
        _fsm.OnStateStay();
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        Status[StatType.Speed].SetValue(5f);
        _detectRange = 5f;
        _fsm = new();
        _pathFinder = GetComponent<MonsterPathFinder>();
        _fsm.BindEvent(State.Idle, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Enter, SetDestination);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToIdle);
        _fsm.StateTransition(State.Idle);
        return true;
    }

    private void DetectingPlayer()
    {
        //Vector2 dir = _target.position - transform.position;
        //Vector2 dir = _target.position - transform.position;
        _testTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = _testTarget - (Vector2)transform.position;
        if (DetectRange > dir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        //_pathFinder.SetDestination(_target);
        _pathFinder.SetDestination(_testTarget);
    }

    private void TransitionAggroToIdle()
    {
        if (Mathf.Approximately(_pathFinder.RemainingDistance, 0f))
            _fsm.StateTransition(State.Idle);
    }
}