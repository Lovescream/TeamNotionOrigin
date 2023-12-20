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
        if (_detectRange > _targetDir.magnitude)
            _fsm.StateTransition(State.Aggro);
    }

    private void SetDestination()
    {
        _pathFinder.SetDestination(_target);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentState == State.Dead)
            return;

        if (collision.gameObject.TryGetComponent<Creature>(out var creature))
        {
            creature.Hp -= Status[StatType.Damage].Value;
            transform.position += -0.3f * (Vector3)_targetDir.normalized;
        }
    }

    public override void Dead()
    {
        base.Dead();
        _pathFinder.Agent.ResetPath();
        _fsm.StateTransition(State.Dead);
    }
}