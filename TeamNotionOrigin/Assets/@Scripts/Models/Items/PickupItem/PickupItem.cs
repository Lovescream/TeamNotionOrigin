using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : Item {
    [SerializeField] private LayerMask canBePickupBy;

    public float playerHP = 10;
    public PickupItem(ItemData data) : base(data) {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(canBePickupBy.value == (canBePickupBy.value | (1<<other.gameObject.layer)))
        {
            if (other.tag == "HealingPotion" && playerHP == 10)
                return;
            else
            {
                OnPickedUp(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
    protected abstract void OnPickedUp(GameObject receiver);
}