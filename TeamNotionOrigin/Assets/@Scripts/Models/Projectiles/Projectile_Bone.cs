using UnityEngine;

public class Projectile_Bone : Projectile
{
    private float _rotateSpeed;

    private Transform _spriteTransform;

    protected override void Awake() { }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        _spriteTransform = _spriter.transform;
        _spriter.sprite = Main.Resource.Load<Sprite>("Projectile_Bone.sprite");
        _rotateSpeed = Random.Range(0, 2) == 0 ? -1080f : 1080f;
        _spriteTransform.Rotate(new(0, 0, Random.value * 360f));
        return true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _spriteTransform?.Rotate(_rotateSpeed * Time.fixedDeltaTime * Vector3.forward);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Main.Object.Despawn(this);
    }
}