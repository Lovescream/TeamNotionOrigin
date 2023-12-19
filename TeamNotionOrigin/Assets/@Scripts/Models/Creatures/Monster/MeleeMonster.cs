using System.Runtime.CompilerServices;
using UnityEngine;

public class MeleeMonster : Monster
{
    public enum State
    {
        Idle,
        Aggro,
        Attack,
        Dead,
    }

    public State CurrentState => _fsm.CurrentState;

    protected StateMachine<State> _fsm;
    protected MonsterPathFinder _pathFinder;

    // test code
    [SerializeField] private Vector2 _testTarget;

    protected override void FixedUpdate()
    {
        _fsm.OnStateStay();

        LookDirection = _pathFinder.Agent.velocity.normalized;
        if (!Mathf.Approximately(LookDirection.magnitude, 0f))
            _spriter.flipX = LookDirection.x < 0;
        //_rigidbody.velocity = Velocity;
        _animator.SetFloat(AnimatorParameterHash_Speed, _pathFinder.Agent.velocity.magnitude);
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        _fsm = new();
        _pathFinder = GetComponent<MonsterPathFinder>();

        //TODO: SetInfo에서 아래 내용 설정하도록 변경해야함 ..
        _detectRange = 5f;
        _attackRange = 0.3f;
        _fsm.BindEvent(State.Idle, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, SetDestination);
        //_fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToIdle);
        _fsm.StateTransition(State.Idle);
        return true;
    }
    public override void SetInfo(Data.Creature data)
    {
        base.SetInfo(data);
    }

    private void DetectingPlayer()
    {
        // TODO: 현재 테스트용으로 마우스를 쫓게 해놨음.

        //Vector2 dir = _target.position - transform.position;
        //Vector2 dir = _target.position - transform.position;
        _testTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = _testTarget - (Vector2)transform.position;
        if (_detectRange > dir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        //_pathFinder.SetDestination(_target);
        _pathFinder.SetDestination(_testTarget);
    }

    //private void TransitionAggroToIdle()
    //{
    //    if (Mathf.Approximately(_pathFinder.RemainingDistance, 0f))
    //        _fsm.StateTransition(State.Idle);
    //}
}