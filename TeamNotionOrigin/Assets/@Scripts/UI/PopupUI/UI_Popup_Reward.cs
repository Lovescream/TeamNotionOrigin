using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Reward : UI_Popup
{
    enum Texts
    {
        FirstItemConst,
        SecondItemCost,
        ThirdItemCost
    }

    enum Images
    {
        FirstItemIcon,
        SecondItemIcon,
        ThirdItemIcon
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Data.Player p1 = Main.Game.Player;
        //Data.Weapon weapons = Main.Game.

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        //GetImage((int)Images.FirstItemIcon).sprite
        


        return true;
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
