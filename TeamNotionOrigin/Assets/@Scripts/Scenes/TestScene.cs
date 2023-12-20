using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;
        MapGenerator mapGenerator = Main.Resource.Instantiate("MapGenerate.prefab").GetComponent<MapGenerator>();
        return true;
    }

}