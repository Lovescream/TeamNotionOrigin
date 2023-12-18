using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        // ==================================== 씬 진입 시 처리 ====================================

        CheckLoadData();
        UI = Main.UI.ShowSceneUI<UI_GameScene>();


        // =========================================================================================
        return true;
    }

    public void SetPause(bool isPause) {
        Time.timeScale = isPause ? 0 : 1;
    }


    /// <summary>
    /// 디버깅: 데이터 로드가 잘 이루어졌는지 확인합니다!
    /// </summary>
    private void CheckLoadData() {
        if (!Main.Data.MonsterDict.TryGetValue(1, out Data.Monster monster)) return;
        Debug.Log(monster.monsterType);
        if (!Main.Data.PlayerDict.TryGetValue(1, out Data.Player player)) return;
        Debug.Log(player.defence);
        if (!Main.Data.ItemDict[Data.ItemType.Passive].TryGetValue(1, out Data.Item item1)) return;
        if (item1 is not Data.Passive passiveItem) return;
        Debug.Log(passiveItem.name);
    }
}