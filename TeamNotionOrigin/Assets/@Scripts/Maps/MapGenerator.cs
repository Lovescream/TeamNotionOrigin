using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize; //원하는 맵의 크기
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; //공간이 나눠지는 최대 비율
    [SerializeField] private int maximumDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile;
    [SerializeField] Tile wallTile;
    [SerializeField] Tile outTile;
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer monsterSprite_1;
    [SerializeField] private SpriteRenderer monsterSprite_2;
    private RectInt smallestNode;

    private void Start()
    {
        FillBackground();
        Node root = new Node(new RectInt(0,0,mapSize.x, mapSize.y)); // 전체 맵 크기의 루트노드를 만듬
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall();
        PlaceMainSpriteInRoom(smallestNode);
    }

    private void Divide(Node tree, int n)
    {
        if (n == maximumDepth) // 원하는 깊이에 도착하면, 더이상 나누지 않는다.
            return;

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height); // 가로 세로 중 긴것을 나누기 위한 변수
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate)); // 쪼갤 지점을 랜덤으로 저장
        
        if(tree.nodeRect.width >= tree.nodeRect.height) // 가로가 더 긴 경우,
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
            PlaceMonsterInRoom(rect);
            FindSmallestRoom(rect);
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }
    private RectInt FindSmallestRoom(RectInt rectInt)
    {
        if (rectInt.width * rectInt.height > smallestNode.width * smallestNode.height)
        {
            smallestNode = rectInt;
        }
        return smallestNode;
    }

    private void PlaceMainSpriteInRoom(RectInt roomRect)
    {
        int x = roomRect.x + roomRect.width / 2;
        int y = roomRect.y + roomRect.height / 2;

        Instantiate(mainSprite, new Vector3(x - mapSize.x / 2, y - mapSize.y / 2, 0), Quaternion.identity);
    }

    private void GenerateLoad(Node tree, int n)
    {
        if (n == maximumDepth)
            return;
        Vector2Int leftNodeCenter = tree.leftNode.center;
        Vector2Int rightNodeCenter = tree.rightNode.center;

        for (int i = Mathf.Min(leftNodeCenter.x, rightNodeCenter.x); i <= Mathf.Max(leftNodeCenter.x, rightNodeCenter.x); i++)
        {
            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, leftNodeCenter.y - mapSize.y / 2, 0), roomTile);
        }

        for (int j = Mathf.Min(leftNodeCenter.y, rightNodeCenter.y); j <= Mathf.Max(leftNodeCenter.y, rightNodeCenter.y); j++)
        {
            tileMap.SetTile(new Vector3Int(rightNodeCenter.x - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
        }
        //이전에 선으로 만들었던 부분을 room tile로 채우는 과정

        GenerateLoad(tree.leftNode, n + 1); //자식 노드 탐색
        GenerateLoad(tree.rightNode, n + 1);
    }

    void FillBackground() // 배경을 채우는 함수
    {
        for (int i = -10; i <= mapSize.x + 10; i++) // 맵 크기보다 넓게
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
   

    private void PlaceMonsterInRoom(RectInt roomRect)
    {
        int x1 = Random.Range(roomRect.x + 3, roomRect.x + roomRect.width - 3);
        int y1 = Random.Range(roomRect.y + 3, roomRect.y + roomRect.height - 3);

        int x2 = Random.Range(roomRect.x + 3, roomRect.x + roomRect.width - 3);
        int y2 = Random.Range(roomRect.y + 3, roomRect.y + roomRect.height - 3);

        if (x1 == x2 || y1 == y2)
        {
            PlaceMonsterInRoom(roomRect); // 두 몬스터가 동일한 위치에 생성되는 경우, 위치를 다시 생성
            return;
        }

        Instantiate(monsterSprite_1, new Vector3(x1 - mapSize.x / 2, y1 - mapSize.y / 2, 0), Quaternion.identity);
        Instantiate(monsterSprite_2, new Vector3(x2 - mapSize.x / 2, y2 - mapSize.y / 2, 0), Quaternion.identity);
    }
   
}