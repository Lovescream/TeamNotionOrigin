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
        if (_target == null)
            _target = Main.Object.Player.transform;

        _targetDir = _target.position - transform.position;
        LookDirection = _pathFinder.Agent.velocity.normalized;
        if (!Mathf.Approximately(LookDirection.magnitude, 0f))
            _spriter.flipX = LookDirection.x < 0;
        //_rigidbody.velocity = Velocity;
        _animator.SetFloat(AnimatorParameterHash_Speed, _pathFinder.Agent.velocity.magnitude);
        _fsm.OnStateStay();
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _fsm = new();
        _pathFinder = GetComponent<MonsterPathFinder>();

        _fsm.BindEvent(State.Idle, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, DetectingPlayer);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, SetDestination);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToAttack);
        _fsm.BindEvent(State.Attack, StateEvent.Enter, () => { _pathFinder.Agent.ResetPath(); });
        _fsm.BindEvent(State.Attack, StateEvent.Stay, OnAttackStay);
        _fsm.BindEvent(State.Attack, StateEvent.Stay, DetectingPlayer);
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
        if (_detectRange > _targetDir.magnitude)
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
            //Main.Object.Spawn<Projectile_Bone>(0, transform.position);
            Main.Object.SpawnProjectile<Projectile_Bone>(transform.position + new Vector3(0, 0.3f, 0), _targetDir, 5f, gameObject.layer, Status[StatType.Damage].Value);
            Debug.Log($"{name}: Attack");
        }
    }

    public override void Dead()
    {
        base.Dead();
        _pathFinder.Agent.ResetPath();
        _fsm.StateTransition(State.Dead);
    }
}