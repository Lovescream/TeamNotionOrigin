using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : Item {

    [SerializeField] private LayerMask canBePickupBy;

    public PickupItem(Data.Item data) : base(data) {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            if (other.tag == "HealingPotion" && Main.Game.Player.hp == Main.Game.Player.maxHp)
                return;
            else
            {
                OnPickedUp();
                Destroy(gameObject);
            }
        }
    }
    protected abstract void OnPickedUp();
}