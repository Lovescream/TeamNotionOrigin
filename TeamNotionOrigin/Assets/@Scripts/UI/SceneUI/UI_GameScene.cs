using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene {

    #region Enums

    enum Texts {
        txtKillCount,
    }
    enum Buttons {
        btnPause,
        btnOpenTempPopup01,
        btnOpenTempPopup02,
    }
    enum Objects {
        UI_HpInfo,
        UI_GunInfo,
    }

    #endregion

    public UI_HpInfo HpInfo { get; private set; }
    public UI_GunInfo GunInfo { get; private set; }

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        this.HpInfo = GetObject((int)Objects.UI_HpInfo).GetComponent<UI_HpInfo>();
        this.HpInfo.Initialize();
        this.GunInfo = GetObject((int)Objects.UI_GunInfo).GetComponent<UI_GunInfo>();
        this.GunInfo.Initialize();
        GetButton((int)Buttons.btnPause).onClick.AddListener(OnBtnPause);
        GetButton((int)Buttons.btnOpenTempPopup01).onClick.AddListener(OnBtnOpenTempPopup01);
        GetButton((int)Buttons.btnOpenTempPopup02).onClick.AddListener(OnBtnOpenTempPopup02);

        return true;
    }

    #region OnButtons

    private void OnBtnPause() {
        Main.UI.ShowPopupUI<UI_Popup_Pause>();
    }
    private void OnBtnOpenTempPopup01() {
        Main.UI.ShowPopupUI<UI_Popup_Temp01>();
    }
    private void OnBtnOpenTempPopup02() {
        Main.UI.ShowPopupUI<UI_Popup_Temp02>();
    }

    #endregion

}