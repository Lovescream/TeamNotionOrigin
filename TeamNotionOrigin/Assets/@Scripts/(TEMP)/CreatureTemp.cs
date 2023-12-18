using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTemp : MonoBehaviour {

    #region Properties

    public Data.Creature Data { get; protected set; }
    public CreatureState State { get; protected set; }
    public Status Status { get; protected set; }

    public float Hp {
        get => _hp;
        set {
            if (value <= 0) {
                _hp = 0;
                if (State != CreatureState.Dead) {
                    State = CreatureState.Dead;
                    _animator.SetBool(AnimatorParameterHash_Dead, true);
                }
            }
            else if (value >= Status[StatType.Hp].Value) {
                _hp = Status[StatType.Hp].Value;
            }
            _hp = value;
        }
    }

    public Vector2 Velocity { get; protected set; }
    public Vector2 LookDirection { get; protected set; }
    public float LookAngle => Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;

    #endregion

    #region Fields

    private static readonly int AnimatorParameterHash_Speed = Animator.StringToHash("Speed");
    private static readonly int AnimatorParameterHash_Dead = Animator.StringToHash("Dead");

    // State.
    private float _hp;

    // Components.
    private SpriteRenderer _spriter;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    // Callbacks.

    
    private bool _initialized;

    #endregion

    #region MonoBehaviours

    protected virtual void FixedUpdate() {
        if (!_initialized) return;
        _spriter.flipX = LookDirection.x < 0;
        _rigidbody.velocity = Velocity;
        _animator.SetFloat(AnimatorParameterHash_Speed, Velocity.magnitude);
    }

    #endregion

    public virtual bool Initialize() {
        if (_initialized) return false;
        _initialized = true;

        _spriter = this.GetComponent<SpriteRenderer>();
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody2D>();

        return true;
    }

    public virtual void SetInfo(Data.Creature data) {
        Initialize();

        Data = data;
        _animator.runtimeAnimatorController = Main.Resource.Load<RuntimeAnimatorController>($"Character00.animController");
        _animator.SetBool(AnimatorParameterHash_Dead, false);
        Status = new();
        State = CreatureState.Idle;
        Hp = Status[StatType.Hp].Value;
    }

}