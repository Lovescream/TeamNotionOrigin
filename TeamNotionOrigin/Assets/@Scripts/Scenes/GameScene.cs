using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        // ==================================== 씬 진입 시 처리 ====================================

        UI = Main.UI.ShowSceneUI<UI_GameScene>();
        Main.Object.Spawn<Player>(1, new Vector2(0, 0));
        Main.Object.Player.Inventory.Add(Main.Object.Spawn<DefaultGun>(6, new(0, 0)));

        // =========================================================================================
        return true;
    }

    public void SetPause(bool isPause) {
        Time.timeScale = isPause ? 0 : 1;
    }

}