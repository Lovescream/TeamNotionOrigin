using UnityEngine;

public class Monster : Creature
{
    private Transform _target;
    private float _detectRange;
    public float DetectRange { get => _detectRange; set => _detectRange = value; }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        return true;
    }

    public override void SetInfo(CreatureData data)
    {
        base.SetInfo(data);
    }
}