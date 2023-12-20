using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpInfo : UI_Base {

    #region Enums

    enum Texts {
        txtHp,
    }
    enum Objects {
        hpSlider,
    }

    #endregion

    #region Properties

    public Player Player { get; private set; }
    public float Max { get; private set; }

    #endregion

    #region Fields

    // Components.
    private TextMeshProUGUI _txtHp;
    private Slider _hpBar;

    public event Action<Stat> OnChangedMax;
    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));
        BindObject(typeof(Objects));
        _txtHp = GetText((int)Texts.txtHp);
        _hpBar = GetObject((int)Objects.hpSlider).GetComponent<Slider>();

        return true;
    }

    public void SetInfo(Player player) {
        this.Player = player;
        this.Max = this.Player.Status[StatType.Hp].Value;
        this.Player.Status[StatType.Hp].OnChanged += SetMax;
        this.Player.OnChangeHp += Refresh;
        Refresh(this.Player.Hp);
    }

    private void SetMax(Stat stat) {
        this.Max = stat.Value;
    }

    private void Refresh(float currentHp) {
        _txtHp.text = $"{currentHp:F0}/{Max:F0}";
        _hpBar.value = currentHp / Max;
    }
}