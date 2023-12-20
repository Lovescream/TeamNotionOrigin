using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    #region Properties

    // TODO:: Data.

    public Vector2 Velocity { get; protected set; }
    public Vector2 Direction { get; protected set; }
    public float Damage { get; protected set; }

    #endregion

    #region fields

    protected SpriteRenderer _spriter;
    protected Rigidbody2D _rigidbody;
    protected Vector3 mousePoint;


    [SerializeField] private bool _initialized;

    #endregion

    #region MonoBehaviours
    protected virtual void Awake()
    {
        Camera camera = Camera.main;
        mousePoint = camera.ScreenToWorldPoint(Input.mousePosition);
    }
    protected virtual void Start() {
        Initialize();
    }
    protected virtual void FixedUpdate() {
        if (!_initialized) return;
        _rigidbody.velocity = Velocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Creature>(out var creature))
        {
            Debug.Log(creature.name + " " + creature.Hp);
            creature.Hp -= Damage;
        }
    }

    #endregion

    public virtual bool Initialize() {
        if (_initialized) return false;
        _initialized = true;
        _spriter = this.GetComponentInChildren<SpriteRenderer>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _spriter.sprite = Main.Resource.Load<Sprite>("Bullet 5");

        return true;
    }

    public virtual void SetInfo() {
        Initialize();

        // #1. Data 설정.
        // TODO::
    }

    public virtual void SetInfo(Vector2 triggerPosition, Vector2 dir, float velocity, int layer, float damage)
    {
        Initialize();
        transform.position = triggerPosition;
        Direction = dir.normalized;
        Velocity = velocity * Direction;
        gameObject.layer = layer;
        Damage = damage;
    }
}