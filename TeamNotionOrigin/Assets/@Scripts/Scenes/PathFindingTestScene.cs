using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTestScene : BaseScene
{
    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        Main.Object.Spawn<MeleeMonster>(0, new(3, 0));
        Main.Object.Spawn<RangedMonster>(1, new(3.3f, 0.3f));
        return true;
    }
}