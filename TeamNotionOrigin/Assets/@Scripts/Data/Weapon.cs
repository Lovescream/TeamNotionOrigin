using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public enum BulletType
    {
        Arrow,
        Cannon,
        Gun,
        Wand,
        Basic,
    }

    [System.Serializable]
    public class Weapon : Data.Item
    {
        public int damage;
        public float attackSpeed;
        public float bulletSizeX;
        public float bulletSizeY;
        public float bulletSizeZ;
        //장전시간
        public float reloadTime;
        public BulletType bulletType;
        public float critical;
        public float maxBulletAmount;
        public int magazineCapacity;
    }

    [System.Serializable]
    public class WeaponData
    {
        public List<Weapon> weapons = new List<Weapon>();
    }
}