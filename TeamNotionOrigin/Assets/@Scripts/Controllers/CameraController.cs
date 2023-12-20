using Dungeon;
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
        Camera.main.orthographicSize = 6f;
        CamHeight = Camera.main.orthographicSize;
        CamWidth = CamHeight * ScreenRatio;
    }

    void LateUpdate() {
        Follow();
    }

    #endregion

    public void SetTarget(Transform target) {
        this.target = target;
        this.transform.position = new(target.position.x, target.position.y, -10);
    }


    private void Follow() {
        if (target == null) return;

        Room room = target.GetComponent<Creature>().CurrentRoom;
        float x, y, z;
        if (room == null) {
            x = target.position.x;
            y = target.position.y;
            z = -10;
        }
        else {
            float limitX = room.Width * 0.5f - CamWidth;
            float limitY = room.Height * 0.5f - CamHeight;
            x = Mathf.Clamp(target.position.x, room.CenterPosition.x - limitX - 1, room.CenterPosition.x + limitX + 1);
            y = Mathf.Clamp(target.position.y, room.CenterPosition.y - limitY - 1, room.CenterPosition.y + limitY + 1);
            z = -10;
        }
        Vector3 newPosition = new(x, y, z);
        this.transform.position = Vector3.Lerp(this.transform.position, newPosition, Time.deltaTime * 2f);
    }
}