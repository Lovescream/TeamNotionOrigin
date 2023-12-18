using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : Item {
    [SerializeField] private LayerMask canBePickupBy;

    public PickupItem(Data.Item data) : base(data)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            if (other.tag == "HealingPotion" && Main.Game.Player.hp == Main.Game.Player.maxHp)
                return;
            else
            {
                OnPickedUp(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
    private void OnPickedUp(GameObject receiver)
    {
        if (receiver.tag == "gold")
            Main.Game.Player.gold += 100;
        else if (receiver.tag == "hp")
            Main.Game.Player.hp += 1;
        else if (receiver.tag == "ammo")
            Debug.Log("총알 획득");           //현재 가진 총알 갯수+
    }
}