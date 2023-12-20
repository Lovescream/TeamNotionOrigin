using Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene {

    public Player Player { get; private set; }

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        // ==================================== 씬 진입 시 처리 ====================================

        // #1. 씬 UI 띄우기.
        UI = Main.UI.ShowSceneUI<UI_GameScene>();

        // #2. 던전 생성.
        Main.Dungeon.GenerateAsync(op => 
        {
            // #3. Player 설정
            Room room = Main.Dungeon.GetRandomRoom();
            Player = Main.Object.Spawn<Player>(1, room.CenterPosition);
            Player.Inventory.Add(Main.Object.Spawn<DefaultGun>(6, new(0, 0)));

            // =========================================================================================

            // ============================= 테스트 코드 (지우셔도 됩니다) =============================

            Main.Object.Spawn<PickupHeart>(1, room.CenterPosition + new Vector2(3, 3));
            Main.Object.Spawn<PickupAmmo>(2, room.CenterPosition + new Vector2(-3, 3));
            Main.Object.Spawn<PickupGold>(100, room.CenterPosition + new Vector2(-3, -3));

            // =========================================================================================
        });

        return true;
    }

    public void SetPause(bool isPause) {
        Time.timeScale = isPause ? 0 : 1;
    }
}