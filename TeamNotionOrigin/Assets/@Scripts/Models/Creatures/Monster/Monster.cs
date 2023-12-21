using Data;
using UnityEngine;

public class Monster : Creature
{

    public override string AnimatorName => "Monster";

    protected Transform _target;
    protected Vector2 _targetDir;
    [SerializeField] protected float _detectRange;
    [SerializeField] protected float _attackRange;

    // TODO: 얘네도 Status나 MosnterData에 있어야할듯.
    public float DetectRange { get => _detectRange; set => _detectRange = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        return true;
    }

    public override void SetInfo(Data.Creature data)
    {
        base.SetInfo(data);
        if (data is Data.Monster monsterData)
        {
            _detectRange = monsterData.detectRange;
            _attackRange = monsterData.attackRange;
        }
    }

    public override void Dead()
    {
        base.Dead();
        // TODO: 아이템 드랍 로직

        Invoke(nameof(Despawn), 0.2f);
    }

    public void Despawn() 
    {
        if (this.gameObject.IsValid())
            Main.Object.Despawn(this);
    }
}