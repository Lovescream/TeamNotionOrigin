using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public enum EffectType
    {
        //데미지 증가
        DamageBoost,
        //이동속도 증가
        SpeedBoost,
        //탄환 크기 증가
        BulletSizeBoost,
        //탄창 증가
        MagazineBoost
    }

    [System.Serializable]
    public class Passive : Item
    {
        public bool stackable;
        public int maxStack;
        public EffectType effectType;
        //해당 효과들에 대한 수치
        public float numericalValue;
    }

    [System.Serializable]
    public class PassiveData
    {
        public List<Passive> passives = new List<Passive>();
    }
}