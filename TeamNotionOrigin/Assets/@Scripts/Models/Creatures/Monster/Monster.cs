using UnityEngine;

public class Monster : Creature
{

    public override string AnimatorName => "Monster";

    protected Transform _target;
    [SerializeField] protected float _detectRange;
    public float DetectRange { get => _detectRange; set => _detectRange = value; }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        return true;
    }

    public override void SetInfo(Data.Creature data)
    {
        base.SetInfo(data);
    }
}