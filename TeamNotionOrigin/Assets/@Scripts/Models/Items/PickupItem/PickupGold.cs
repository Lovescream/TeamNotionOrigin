using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGold : PickupItem
{
    [SerializeField] private int _gold;
    public PickupGold(ItemData data) : base(data)
    {

    }

    protected override void OnPickedUp(GameObject receiver)
    {
        //골드 획득;
    }
}
