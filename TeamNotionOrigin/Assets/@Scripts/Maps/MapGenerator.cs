using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize; //원하는 맵의 크기
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; //공간이 나눠지는 최대 비율
    //[SerializeField] private GameObject line; //lineRenderer를 사용해서 공간이 나눠진걸 시작적으로 보여주기 위함
    //[SerializeField] private GameObject map; //lineRenderer를 사용해서 첫 맵의 사이즈를 보여주기 위함
    //[SerializeField] private GameObject roomLine; //lineRenderer를 사용해서 방의 사이즈를 보여주기 위함
    [SerializeField] private int maximumDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; //방을 구성하는 타일
    [SerializeField] Tile wallTile; //방과 외부를 구분지어줄 벽 타일
    [SerializeField] Tile outTile; //방 외부의 타일


    private void Start()
    {
        FillBackground(); // 씬 로드 시 전부다 바깥타일로 덮음
        Node root = new Node(new RectInt(0,0,mapSize.x, mapSize.y)); // 전체 맵 크기의 루트노드를 만듬
        //DrawMap(0, 0);
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall(); //바깥과 방이 만나는 지점을 벽으로 칠해주는 함수
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
            //DrawLine(new Vector2(tree.nodeRect.x + split, tree.nodeRect.y), new Vector2(tree.nodeRect.x + split, tree.nodeRect.y + tree.nodeRect.height));
        }
        else // 세로가 더 긴 경우,
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
            //DrawLine(new Vector2(tree.nodeRect.x, tree.nodeRect.y + split), new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y + split));
        }

        tree.leftNode.parentNode = tree;
        tree.rightNode.parentNode = tree;
        Divide(tree.leftNode, n + 1);
        Divide(tree.rightNode, n + 1);
    }

    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth) //해당 노드가 리프노드라면 방을 만들어 줄 것이다.
        {
            rect = tree.nodeRect;

            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);

            FillRoom(rect);
            //DrawRectangle(rect);
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
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

        GenerateLoad(tree.leftNode, n + 1); //자식 노드들도 탐색
        GenerateLoad(tree.rightNode, n + 1);
    }

    void FillBackground() // 배경을 채우는 함수
    {
        for (int i = -10; i < mapSize.x + 10; i++) // 맵 크기보다 넓게 채워준다.
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }
    void FillWall() //룸 타일과 바깥 타일이 만나는 부분
    {
        for (int i = 0; i < mapSize.x; i++) //타일 전체를 순회
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

    private void FillRoom(RectInt rect)
    { //room의 rect정보를 받아서 tile을 set해주는 함수
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }

    //private void DrawMap(int x, int y) // x y는 화면의 중앙위치
    //{
    //    // 인덱스에 각 점의 위치를 설정 + 이 점들이 연결되어 선을 형성
    //    LineRenderer lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
    //    lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //좌측 하단
    //    lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //우측 하단
    //    lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//우측 상단
    //    lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //좌측 상단
    //}

    //private void DrawLine(Vector2 from, Vector2 to) //from->to로 이어지는 선을 그리게 될 것이다.
    //{
    //    LineRenderer lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
    //    lineRenderer.SetPosition(0, from - mapSize / 2);
    //    lineRenderer.SetPosition(1, to - mapSize / 2);
    //}


    //private void DrawRectangle(RectInt rect)
    //{
    //    LineRenderer lineRenderer = Instantiate(roomLine).GetComponent<LineRenderer>();
    //    lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - mapSize / 2);
    //    lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - mapSize / 2);
    //    lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - mapSize / 2);
    //    lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - mapSize / 2);
    //}

   
}
