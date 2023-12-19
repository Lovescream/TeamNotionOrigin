using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ItemData/WeaponData", order = 0)]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Info")]
    public float damage;
    public int maxAmmo;
    public int maxMag;
    public float reloadDelay;
    public float attackSpeed;
    public LayerMask target;
}
