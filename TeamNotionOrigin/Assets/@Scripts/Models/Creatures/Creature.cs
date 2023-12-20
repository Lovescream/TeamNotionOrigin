using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CreatureState {
    Idle,
    Dead,
}

public class Creature : MonoBehaviour {

    #region Properties

    public Data.Creature Data { get; protected set; }
    public virtual string AnimatorName => "Creature";
    public CreatureState State { get; protected set; }
    public Status Status { get; protected set; }
    public Inventory Inventory { get; protected set; }

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

    protected static readonly int AnimatorParameterHash_Speed = Animator.StringToHash("Speed");
    protected static readonly int AnimatorParameterHash_Dead = Animator.StringToHash("Dead");

    // State.
    protected float _hp;

    // Components.
    protected SpriteRenderer _spriter;
    protected Animator _animator;
    protected Rigidbody2D _rigidbody;

    // Callbacks.


    private bool _initialized;

    #endregion

    #region MonoBehaviours

    protected virtual void Start()
    {
        Initialize();
    }
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

        _spriter = this.GetComponentInChildren<SpriteRenderer>();
        _animator = this.GetComponentInChildren<Animator>();
        _rigidbody = this.GetComponentInChildren<Rigidbody2D>();

        return true;
    }

    public virtual void SetInfo(Data.Creature data) {
        Initialize();

        // #1. Data 설정.
        Data = data;

        // #2. Animator 설정.
        _animator.runtimeAnimatorController = Main.Resource.Load<RuntimeAnimatorController>($"{AnimatorName}_{Data.id:D2}.animController");
        _animator.SetBool(AnimatorParameterHash_Dead, false);

        // #3. Status 설정.
        Status = new(Data);
        Hp = Status[StatType.Hp].Value;

        // #4. State 설정.
        State = CreatureState.Idle;

        // #5. Inventory 설정.
        SetInventory();
    }

    public virtual void SetInventory() {
        Inventory = new(this);
        Inventory.Gold += Data.gold;
    }
}