using System.Runtime.CompilerServices;
using UnityEngine;

public class RangedMonster : Monster
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
    protected float _attackCooltime;

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
        _detectRange = 7f;
        _attackRange = 5f;
        _fsm.BindEvent(State.Idle, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, SetDestination);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToAttack);
        _fsm.BindEvent(State.Attack, StateEvent.Enter, () => { _pathFinder.Agent.ResetPath(); });
        _fsm.BindEvent(State.Attack, StateEvent.Stay, OnAttackStay);
        _fsm.BindEvent(State.Attack, StateEvent.Stay, DetectingPlayer);
        //_fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToIdle);
        _fsm.StateTransition(State.Idle);
        return true;
    }
    public override void SetInfo(Data.Creature data)
    {
        base.SetInfo(data);
        _pathFinder.SetInfo(this);
        Status[StatType.AttackSpeed].SetValue(data.attackSpeed);
    }

    private void DetectingPlayer()
    {
        if (_target == null)
            _target = Main.Object.Player.transform;

        Vector2 dir = _target.position - transform.position;
        if (_detectRange > dir.magnitude && _attackRange < dir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        _pathFinder.SetDestination(_target);
    }

    private void TransitionAggroToAttack()
    {
        var dir = _target.position - transform.position;
        if (dir.magnitude < _attackRange)
            _fsm.StateTransition(State.Attack);
    }

    private void OnAttackStay()
    {
        _attackCooltime += Time.fixedDeltaTime;
        if (_attackCooltime > Status[StatType.AttackSpeed].Value)
        {
            _attackCooltime = 0f;
            Debug.Log($"{name}: Attack");
        }
    }

    //private void TransitionAggroToIdle()
    //{
    //    if (Mathf.Approximately(_pathFinder.RemainingDistance, 0f))
    //        _fsm.StateTransition(State.Idle);
    //}
}