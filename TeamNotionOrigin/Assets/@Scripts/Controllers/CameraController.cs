using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Properties

    public float CamWidth { get; private set; }
    public float CamHeight { get; private set; }
    public float ScreenRatio => (float)Screen.width / Screen.height;

    #endregion

    #region Fields

    private Transform target;

    #endregion

    #region MonoBehaviours

    void Awake() {
        Camera.main.orthographicSize = 10f;
        CamHeight = Camera.main.orthographicSize;
        CamWidth = CamHeight * ScreenRatio;
    }

    void LateUpdate() {
        Follow();
    }

    #endregion

    public void SetTarget(Transform target) {
        this.target = target;
    }


    private void Follow() {
        if (target == null) return;

        float x = target.position.x;
        float y = target.position.y;

        this.transform.position = new(x, y, -10);
    }
}