using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeart : PickupItem
{
    [SerializeField] private int _heart;
    public PickupHeart(ItemData data) : base(data)
    {

    }

    protected override void OnPickedUp(GameObject receiver)
    {
        //회복
    }
}
