using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Item {

    #region Properties

    public float DurationTime => (Data as Data.Pickup).durationTime;
    public float NumericalValue => (Data as Data.Pickup).numericalValue;
    public float NumericalRatio => (Data as Data.Pickup).numericalRatio;

    #endregion

    //[SerializeField] private LayerMask canBePickupBy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            SetOwner(other.GetComponent<Player>());
            OnPickedUp();
            Main.Object.Despawn(this);
        }
        //if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        //{
        //    if (other.tag == "HealingPotion" && Main.Object.Player.Hp == Main.Object.Player.Status[StatType.Hp].Value)
        //        return;
        //    else
        //    {
        //        OnPickedUp();
        //        Destroy(gameObject);
        //    }
        //}
    }
    protected virtual void OnPickedUp() { }
}