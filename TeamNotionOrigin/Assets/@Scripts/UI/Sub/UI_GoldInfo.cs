using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GoldInfo : UI_Base {

    #region Enums

    enum Images {
        imgIconGold,
    }
    enum Texts {
        txtGold,
    }

    #endregion

    public Player Player { get; private set; }

    private TextMeshProUGUI _txtGold;

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        _txtGold = GetText((int)Texts.txtGold);

        return true;
    }

    public void SetInfo(Player player) {
        Initialize();
        this.Player = player;

        this.Player.Inventory.OnGoldChanged += Refresh;
        Refresh(this.Player.Inventory.Gold);
    }

    private void Refresh(float gold) {
        _txtGold.text = $"{gold}";
    }

}