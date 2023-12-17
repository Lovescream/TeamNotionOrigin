using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parentNode;
    public RectInt nodeRect;
    public RectInt roomRect;

    public Vector2Int center
    {
        //방의 가운데 점. 방과 방을 이을 때 사용 / roomRect 는 항상 좌측아래 좌표를 나타낸다.
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
    }

    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }
}
