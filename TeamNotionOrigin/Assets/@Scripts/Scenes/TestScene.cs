using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        PlayerTemp player = Main.Resource.Instantiate("(Temp)Player.prefab").GetComponent<PlayerTemp>();
        player.SetInfo(new());

        return true;
    }

}