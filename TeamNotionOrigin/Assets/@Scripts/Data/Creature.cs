using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    [System.Serializable]
    public class Creature
    {
        public int id;
        public int hp;
        public int maxHp;
        public int damage;
        public int defence;
        public float speed;
        public int gold;
        public float attackSpeed;
        public int exp;
    }
}