using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

    public float Attack { get; protected set; }
    public float Interval { get; protected set; }
    public float MaxBullet { get; protected set; }
    public float ReloadTime { get; protected set; }

}