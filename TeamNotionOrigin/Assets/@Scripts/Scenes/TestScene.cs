using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        //PlayerTemp player = Main.Resource.Instantiate("(Temp)Player.prefab").GetComponent<PlayerTemp>();
        //player.SetInfo(new());

        //Player player = Main.Object.Spawn<Player>(1, new(0, 0));
        //for (int i = 1; i <= 6; i++) {
        //    Main.Object.Spawn<Monster>(i, new(i, i));
        //}
        //for (int i = 10; i < 20; i++) {
        //    Main.Object.Spawn<Projectile>(i, new(i, i));
        //}
        //MapGenerator mapGenerator = Main.Resource.Instantiate("MapGenerate.prefab").GetComponent<MapGenerator>();
        //Debug.Log(Main.Data.ItemDict[Data.ItemType.Weapon][1].name);
        Player player = Main.Object.Spawn<Player>(1, Vector2.zero);

        return true;
    }

}