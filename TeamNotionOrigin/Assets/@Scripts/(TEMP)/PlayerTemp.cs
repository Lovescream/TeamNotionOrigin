using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTemp : CreatureTemp {

    public PlayerInventory Inventory { get; private set; }

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        return true;
    }

    public override void SetInfo(Data.Creature data) {
        base.SetInfo(data);

        Inventory = new();
    }

    #region Input

    protected void OnMove(InputValue value) {
        Vector2 direction = value.Get<Vector2>().normalized;
        Velocity = Status[StatType.Speed].Value * direction;
    }
    protected void OnLook(InputValue value) {
        LookDirection = (Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - this.transform.position).normalized;
    }
    protected void OnFire() {
        Debug.Log("OnFire!");
    }
    protected void OnRoll() {
        Debug.Log("OnRoll!");
    }
    protected void OnInteraction() {
        Debug.Log("OnInteraction!");
    }

    #endregion

}