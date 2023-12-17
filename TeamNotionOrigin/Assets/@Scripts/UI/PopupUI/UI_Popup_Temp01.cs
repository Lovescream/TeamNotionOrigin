using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Temp01 : UI_Popup {

    #region Enums

    enum Buttons {
        btnCancel,
        btnConfirm,
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.btnCancel).onClick.AddListener(OnBtnCancel);
        GetButton((int)Buttons.btnConfirm).onClick.AddListener(OnBtnConfirm);

        return true;
    }

    #region OnButtons

    private void OnBtnCancel() {
        Debug.Log($"[UI_Popup_Temp01] Click a cancel button.");
        ClosePopup();
    }
    private void OnBtnConfirm() {
        Debug.Log($"[UI_Popup_Temp01] Click a confirm button.");
        ClosePopup();
    }

    #endregion

}