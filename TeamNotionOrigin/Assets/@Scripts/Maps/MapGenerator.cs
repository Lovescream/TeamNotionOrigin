using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour
{
    #region field
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; //공간이 나눠지는 최대 비율
    [SerializeField] private int maximumDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨
    [SerializeField] Grid grid;
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tilemap outTileMap;
    [SerializeField] Tilemap wallTileMap;
    [SerializeField] Tilemap obstacleTileMap;
    [SerializeField] Tile roomTile;
    [SerializeField] Tile wallTile;
    [SerializeField] Tile outTile;
    [SerializeField] Tile obstacleTile;
    //[SerializeField] private SpriteRenderer portalSprite;
    //[SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer monsterSprite_1;
    [SerializeField] private SpriteRenderer monsterSprite_2;
    private List<RectInt> roomRect;
    #endregion

    private void Start()
    {
        roomRect = new List<RectInt>();
        tileMap = Instantiate(grid).transform.GetChild(0).GetComponent<Tilemap>();
        outTileMap = grid.transform.GetChild(1).GetComponent<Tilemap>();
        wallTileMap = grid.transform.GetChild(2).GetComponent<Tilemap>();
        obstacleTileMap = grid.transform.GetChild(3).GetComponent<Tilemap>();
        tileMap.ClearAllTiles();
        outTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        obstacleTileMap.ClearAllTiles();
        GenerateMap();
        PlaceSpritesInRoom(roomRect);
        UpdateTilemaps();
    }
    private void GenerateMap()
    {
        FillBackground();
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); // 전체 맵 크기의 루트노드를 만듬
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoadTile(root, 0);
        FillWall();
    }

    #region Divide Node
    private void Divide(Node tree, int n)
    {
        if (n >= maximumDepth)
            return;

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height); // 가로 세로 중 긴것을 나누기 위한 변수
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate)); // 쪼갤 지점을 랜덤으로 저장

        if (tree.nodeRect.width >= tree.nodeRect.height) // 가로가 더 긴 경우,
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
        }
        else // 세로가 더 긴 경우,
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
        }

        tree.leftNode.parentNode = tree;
        tree.rightNode.parentNode = tree;
        Divide(tree.leftNode, n + 1);
        Divide(tree.rightNode, n + 1);
    }
    #endregion

    #region Generate Room
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth)
        {
            rect = tree.nodeRect;

            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);

            FillRoom(rect);
            roomRect.Add(rect);
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }
    #endregion

    #region Fill Tile
    private void GenerateLoadTile(Node tree, int n)
    {
        if (n == maximumDepth)
            return;
        Vector2Int leftNodeCenter = tree.leftNode.center;
        Vector2Int rightNodeCenter = tree.rightNode.center;

        //가로 load
        for (int i = Mathf.Min(leftNodeCenter.x, rightNodeCenter.x); i <= Mathf.Max(leftNodeCenter.x, rightNodeCenter.x); i++)
        {
            for (int k = 0; k < 3; k++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, leftNodeCenter.y + k - mapSize.y / 2, 0), roomTile);
            }
        }

        // 세로 load
        for (int j = Mathf.Min(leftNodeCenter.y, rightNodeCenter.y); j <= Mathf.Max(leftNodeCenter.y, rightNodeCenter.y); j++)
        {
            for (int k = 0; k < 3; k++)
            {
                tileMap.SetTile(new Vector3Int(rightNodeCenter.x + k - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }

        GenerateLoadTile(tree.leftNode, n + 1);
        GenerateLoadTile(tree.rightNode, n + 1);
    }


    void FillBackground()
    {
        for (int i = -10; i <= mapSize.x + 10; i++)
        {
            for (int j = -10; j <= mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }

    private void FillRoom(RectInt rect)
    { //room의 rect정보를 받아서 tile을 set
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                    tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }


    void FillWall() //roomTile과 outTile이 만나는 부분
    {
        for (int i = 0; i < mapSize.x; i++) // 타일 전체를 순회
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //바깥타일 일 경우
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;//바깥 타일 기준 8방향을 탐색해서 room tile이 있다면 wall tile로 바꿔준다.
                            if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateTilemaps()
    {
        //outTileMap.size = tileMap.size;
        wallTileMap.size = tileMap.size;
        //obstacleTileMap.size = tileMap.size;
        for (int i = -10; i <= mapSize.x + 10; i++)
        {
            for (int j = -10; j <= mapSize.y + 10; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0);
                Tile tile = tileMap.GetTile<Tile>(tilePosition);

                if (tile == wallTile)
                {
                    //wallTileMap.SetTile(tilePosition, wallTile);
                    tileMap.SetTile(tilePosition, null);
                    wallTileMap.SetTile(tilePosition, wallTile);
                }
                //else if (tile == obstacleTile)
                //{
                //    obstacleTileMap.SetTile(tilePosition, obstacleTile);
                //    tileMap.SetTile(tilePosition, null);
                //}
                //else if (tile == outTile)
                //{
                //    outTileMap.SetTile(tilePosition, outTile);
                //    tileMap.SetTile(tilePosition, null);
                //}
            }
        }
    }
    #endregion

    #region Place Objects
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

        Main.Object.Spawn<Monster>(1, new Vector3(pos1.x - mapSize.x / 2, pos1.y - mapSize.y / 2, 0));
        Main.Object.Spawn<Monster>(1, new Vector3(pos2.x - mapSize.x / 2, pos2.y - mapSize.y / 2, 0));
        //Monster monster = Main.Object.Spawn<Monster>(2, new Vector3(pos2.x - mapSize.x / 2, pos2.y - mapSize.y / 2, 0));
        //if(monster.gameObject.activeSelf == false)
        //{

        //}
    }

    private void MainSpriteInRoom(RectInt roomRect)
    {
        int x = roomRect.x;
        int y = roomRect.y;

        Main.Object.Spawn<Player>(1, new Vector3((x - mapSize.x / 2) + 1, (y - mapSize.y / 2) + 1, 0));
    }

    private void PortalSpriteInRoom(RectInt roomRect)
    {
        int x = roomRect.x + roomRect.width;
        int y = roomRect.y + roomRect.height;

        //Instantiate(portalSprite, new Vector3((x - mapSize.x / 2) - 1, (y - mapSize.y / 2) - 1, 0), Quaternion.identity);
    }

    private void PlaceSpritesInRoom(List<RectInt> roomRect)
    {
        foreach (RectInt rect in roomRect)
        {
            MonstersInRoom(rect);
        }
        MainSpriteInRoom(roomRect[0]);
        //PortalSpriteInRoom(roomRect[roomRect.Count - 1]);
    }
}
#endregion