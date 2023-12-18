using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data { 
[System.Serializable]
    public class Pickup : Item
    {
        public float durationTime;
        //탄환같은 경우는 수량, 포션같은 경우는 수치
        public float numericalValue;
        public float numericalRatio;

    }

    [System.Serializable]
    public class PickupData
    {
        public List<Pickup> pickups = new List<Pickup>();
    }
}