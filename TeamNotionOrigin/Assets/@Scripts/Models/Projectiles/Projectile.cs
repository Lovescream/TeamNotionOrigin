using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    #region Properties

    // TODO:: Data.

    public Vector2 Velocity { get; protected set; }
    public Vector2 Direction { get; protected set; }

    #endregion

    #region fields

    private SpriteRenderer _spriter;
    private Rigidbody2D _rigidbody;



    private bool _initialized;

    #endregion

    #region MonoBehaviours

    protected virtual void Start() {
        Initialize();
    }
    protected virtual void FixedUpdate() {
        if (!_initialized) return;
        _rigidbody.velocity = Velocity;
    }

    #endregion

    public virtual bool Initialize() {
        if (!_initialized) return false;
        _initialized = true;

        _spriter = this.GetComponent<SpriteRenderer>();
        _rigidbody = this.GetComponent<Rigidbody2D>();

        return true;
    }

    public virtual void SetInfo() {
        Initialize();

        // #1. Data 설정.
        // TODO::
    }
}