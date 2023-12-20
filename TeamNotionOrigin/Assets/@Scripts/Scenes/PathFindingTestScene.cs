using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestScene : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        UI = Main.UI.ShowSceneUI<UI_GameScene>();
        var player = Main.Object.Spawn<Player>(1, new(-7, 2));
        //player.Inventory.Add(Main.Object.Spawn<DefaultGun>(6, new(-7, 2)));

        //Main.Object.SpawnMonster<MeleeMonster>(0, new(3, 0));
        //Main.Object.SpawnMonster<MeleeMonster>(0, new(3.3f, 0.3f));

        //Main.Object.Despawn(Main.Object.Monsters[0]);
        //Main.Object.Despawn(Main.Object.Monsters[0]);

        Main.Object.SpawnMonster<RangedMonster>(1, new(3.3f, 0.3f));
        //Main.Object.SpawnMonster<MeleeMonster>(0, new(3, 0));

        //Main.Object.SpawnMonster<BossMonster>(6, new(3, 0));

        return true;
    }
}