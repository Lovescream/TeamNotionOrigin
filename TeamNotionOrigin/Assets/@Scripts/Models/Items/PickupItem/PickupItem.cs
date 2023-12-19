using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : Item {

    [SerializeField] private LayerMask canBePickupBy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            if (other.tag == "HealingPotion" && Main.Object.Player.Hp == Main.Object.Player.Status[StatType.Hp].Value)
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