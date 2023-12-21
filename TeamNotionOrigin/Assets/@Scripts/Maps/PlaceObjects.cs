using System.Collections.Generic;
using UnityEngine;

public class PlaceObjects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer portalSprite;
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer monsterSprite_1;
    [SerializeField] private SpriteRenderer monsterSprite_2;


    public void PlaceSpritesInRoom(List<RectInt> roomRect)
    {
        foreach (RectInt rect in roomRect)
        {
            MonstersInRoom(rect);
        }
        MainSpriteInRoom(roomRect[0]);
        PortalSpriteInRoom(roomRect[roomRect.Count - 1]);
    }


    private void MonstersInRoom(RectInt roomRect)
    {
        float radius1 = Mathf.Max(monsterSprite_1.size.x, monsterSprite_1.size.y) / 2;
        float radius2 = Mathf.Max(monsterSprite_2.size.x, monsterSprite_2.size.y) / 2;
        float minDistance = radius1 + radius2; // 두 스프라이트의 최소 거리 설정

        Vector2 pos1, pos2;

        do
        {
            pos1 = new Vector2(Random.Range(roomRect.x + 3, roomRect.x + roomRect.width - 3), Random.Range(roomRect.y + 3, roomRect.y + roomRect.height - 3));
            pos2 = new Vector2(Random.Range(roomRect.x + 3, roomRect.x + roomRect.width - 3), Random.Range(roomRect.y + 3, roomRect.y + roomRect.height - 3));
        }
        while (Vector2.Distance(pos1, pos2) < minDistance); // 두 몬스터의 위치가 너무 가까우면 다시 생성

        Instantiate(monsterSprite_1, new Vector3(pos1.x, pos1.y, 0), Quaternion.identity);
        Instantiate(monsterSprite_2, new Vector3(pos2.x, pos2.y, 0), Quaternion.identity);
    }

    private void MainSpriteInRoom(RectInt roomRect)
    {
        Vector2 center = new Vector2(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);

        Instantiate(mainSprite, new Vector3(center.x, center.y, 0), Quaternion.identity);
    }

    private void PortalSpriteInRoom(RectInt roomRect)
    {
        Vector2 position = new Vector2(roomRect.x + roomRect.width, roomRect.y + roomRect.height);

        Instantiate(portalSprite, new Vector3(position.x, position.y, 0), Quaternion.identity);
    }
}