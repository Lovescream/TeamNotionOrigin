using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour 
{
    public float HpMax { get; protected set; }
    public float Attack { get; protected set; }
    public float Defense { get; protected set; }
    public float Speed { get; protected set; }

    public virtual float Hp 
    {
        get => _hp;
        set => _hp = value;
    }

    private float _hp;
}