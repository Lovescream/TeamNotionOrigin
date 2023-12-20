using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {

    #region Properties

    public override string AnimatorName => "Character";

    public Weapon CurrentWeapon => (Inventory as PlayerInventory).EquippedWeapon;

    #endregion

    #region MonoBehaviours

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Hp -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            PeoplesGun peoplesGun = Main.Object.Spawn<PeoplesGun>(10, new(0, 0));
            Inventory.Add(peoplesGun);
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            Magnum magnum = Main.Object.Spawn<Magnum>(11, new(0, 0));
            Inventory.Add(magnum);
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            ShotGun shotgun = Main.Object.Spawn<ShotGun>(12, new(0, 0));
            Inventory.Add(shotgun);
        }
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        FindObjectOfType<CameraController>().SetTarget(this.transform);

        return true;
    }

    public override void SetInfo(Data.Creature data) {
        base.SetInfo(data);
        if (Main.Scene.CurrentScene.UI != null)
        (Main.Scene.CurrentScene.UI as UI_GameScene).SetInfo(this);
    }

    public override void SetInventory() {
        Inventory = new PlayerInventory(this);
        Inventory.Gold += Data.gold;
    }

    #region Input

    protected void OnMove(InputValue value) {
        Vector2 direction = value.Get<Vector2>().normalized;
        Velocity = Status[StatType.Speed].Value * direction;
    }
    protected void OnLook(InputValue value) {
        LookDirection = (Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - this.transform.position).normalized;
        CurrentWeapon?.Rotate(LookDirection);
    }
    protected void OnFire() {
        if (CurrentWeapon == null) return;
        CurrentWeapon.Shoot();
        Debug.Log($"[Player] OnFire(): (CurrentWeapon: {CurrentWeapon.Name}) (CurrentMag: {CurrentWeapon.CurrentMag})");
    }
    protected void OnRoll() {
        Debug.Log("OnRoll!");
    }
    protected void OnInteraction() {
        Debug.Log("OnInteraction!");
    }
    protected void OnScroll(InputValue value) {
        float y = value.Get<Vector2>().normalized.y;
        if (y > 0)
            (Inventory as PlayerInventory).EquipPrevWeapon();
        else if (y < 0)
            (Inventory as PlayerInventory).EquipNextWeapon();
        if (CurrentWeapon == null) return;
        Debug.Log($"[Player] OnScroll(): (CurrentWeapon: {CurrentWeapon.Name})");
    }
    #endregion

}