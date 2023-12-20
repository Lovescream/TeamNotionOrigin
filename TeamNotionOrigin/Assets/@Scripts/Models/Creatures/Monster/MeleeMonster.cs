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
        _attackRange = 0.5f;
        _fsm.BindEvent(State.Idle, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, SetDestination);
        _fsm.StateTransition(State.Idle);
        return true;
    }
    public override void SetInfo(Data.Creature data)
    {
        base.SetInfo(data);
        _pathFinder.SetInfo(this);
    }

    private void DetectingPlayer()
    {
        if (_target == null)
            _target = Main.Object.Player.transform;

        Vector2 dir = _target.position - transform.position;
        if (_detectRange > dir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        _pathFinder.SetDestination(_target);
    }
}