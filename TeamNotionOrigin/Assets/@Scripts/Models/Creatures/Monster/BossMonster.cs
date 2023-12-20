using System.Linq;
using UnityEngine;

public class BossMonster : Monster
{
    public enum State
    {
        Idle,
        Aggro,
        Charge,
        Attack,
        Groggy,
        Dead,
    }

    public State CurrentState => _fsm.CurrentState;

    protected StateMachine<State> _fsm;
    protected MonsterPathFinder _pathFinder;
    protected float _attackCooltime;
    protected float _chargeLapse;
    protected float _rushLapse;
    protected float _groggyLapse;
    protected int _reflectCount;
    protected readonly float _chargeTime = 2f;
    protected readonly float _rushTime = 2f;
    protected readonly float _groggyTime = 5f;
    protected readonly float _rushForce = 10f;
    protected Vector2 _rushDir;

    protected static readonly int AnimatorParameterHash_Attack = Animator.StringToHash("Attack");
    //protected static readonly int AnimatorParameterHash_Groggy = Animator.StringToHash("Groggy");

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
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, SetDestination);
        _fsm.BindEvent(State.Aggro, StateEvent.Stay, TransitionAggroToCharge);
        _fsm.BindEvent(State.Charge, StateEvent.Enter, OnChargeEnter);
        _fsm.BindEvent(State.Charge, StateEvent.Stay, OnChargeStay);
        _fsm.BindEvent(State.Attack, StateEvent.Enter, OnRushEnter);
        _fsm.BindEvent(State.Attack, StateEvent.Stay, OnRushStay);
        _fsm.BindEvent(State.Attack, StateEvent.Exit, OnRushExit);
        _fsm.BindEvent(State.Groggy, StateEvent.Enter, OnGroggyEnter);
        _fsm.BindEvent(State.Groggy, StateEvent.Stay, OnGroggyStay);
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
        if (_detectRange > _targetDir.magnitude && _attackRange < _targetDir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        var dir = _target.position - transform.position;
        if (dir.magnitude > _attackRange)
            _pathFinder.SetDestination(_target);
        else
            _pathFinder.Agent.ResetPath();
    }

    private void TransitionAggroToCharge()
    {
        _attackCooltime += Time.fixedDeltaTime;
        if (_attackCooltime > Status[StatType.AttackSpeed].Value && _targetDir.magnitude < _attackRange)
        {
            _fsm.StateTransition(State.Charge);
            _attackCooltime = 0f;
        }
    }

    private void OnChargeEnter()
    {
        _animator.SetBool(AnimatorParameterHash_Attack, true);
        _chargeLapse = 0f;
        Debug.Log("Charge Start");
    }

    private void OnChargeStay()
    {
        _chargeLapse += Time.fixedDeltaTime;
        if (_chargeLapse > _chargeTime)
        {
            _fsm.StateTransition(State.Attack);
        }
    }

    private void OnRushEnter()
    {
        _reflectCount = 0;
        _rushLapse = 0f;
        _rushDir = _targetDir;
        _pathFinder.Agent.enabled = false;
        Debug.Log("Rush Start");
    }

    private void OnRushStay()
    {
        _rushLapse += Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + _rushForce * Time.fixedDeltaTime * _rushDir.normalized);
        if (_rushLapse > _rushTime)
        {
            if (_reflectCount >= 3)
                _fsm.StateTransition(State.Groggy);
            else
                _fsm.StateTransition(State.Aggro);
        }
    }

    private void OnRushExit()
    {
        _pathFinder.Agent.enabled = true;
        _animator.SetBool(AnimatorParameterHash_Attack, false);
    }

    private void OnGroggyEnter()
    {
        _groggyLapse = 0f;
        _spriter.color = new(0.5f, 0.5f, 1f, 1f);
    }

    private void OnGroggyStay()
    {
        _groggyLapse += Time.fixedDeltaTime;
        if (_groggyLapse > _groggyTime)
        {
            _fsm.StateTransition(State.Aggro);
            _spriter.color = Color.white;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            _rushDir = Vector2.Reflect(_rushDir, collision.contacts[0].normal);
            _reflectCount++;
        }
    }
}