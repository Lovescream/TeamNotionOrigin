using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Pause : UI_Popup {

    #region Enums

    enum Buttons {
        Background,
    }

    #endregion

    #region Fields

    private GameScene _scene;

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Background).onClick.AddListener(OnBtnBackground);

        _scene = Main.Scene.CurrentScene as GameScene;
        _scene.SetPause(true);

        return true;
    }

    private void OnBtnBackground() {
        _scene.SetPause(false);
        ClosePopup();
    }
}