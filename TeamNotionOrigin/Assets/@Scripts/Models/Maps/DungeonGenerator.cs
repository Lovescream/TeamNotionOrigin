using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon {
    public class DungeonGenerator {

        public Vector2Int Origin => new(-Size.x / 2, -Size.y / 2);

        public TileMap Background { get; private set; }
        public TileMap Room { get; private set; }
        public TileMap Road { get; private set; }
        public TileMap Wall { get; private set; }
        public TileMap Obstacle { get; private set; }

        public Vector2Int Size { get; private set; }
        public int Margin { get; private set; }
        public int MaxDepth { get; private set; }
        public float MinDivideRate { get; private set; }
        public float MaxDivideRate { get; private set; }

        public List<Room> Rooms { get; private set; } = new();

        public DungeonGenerator(Vector2Int size, int margin, int maxDepth, float minDivideRate, float maxDivideRate) {
            Size = size;
            Margin = margin;
            MaxDepth = maxDepth;
            MinDivideRate = minDivideRate;
            MaxDivideRate = maxDivideRate;

            Grid grid = new GameObject("Grid").AddComponent<Grid>();
            grid.cellSize = new(1, 1, 0);

            Background = new("Background", grid, Origin, -10, false, "outTile_2");
            Room = new("Room", grid, Origin, -9, true, "roomTile_2");
            Road = new("Road", grid, Origin, -8, true, "roomTile_2");
            Wall = new("Wall", grid, Origin, -7, true, "wallTile_2");
            Obstacle = new("Obstacle", grid, Origin, -7, true, null);
            Obstacle.SetTile(new Tile[] {
                Main.Resource.Load<Tile>("obstacle_4"),
                Main.Resource.Load<Tile>("obstacle_5"),
                Main.Resource.Load<Tile>("obstacle_7"),
                Main.Resource.Load<Tile>("obstacle_8"),
                Main.Resource.Load<Tile>("obstacle_9"),
                Main.Resource.Load<Tile>("obstacle_10"),
            });
        }


        public void GenerateMap() {
            FillBackground();

            Node root = new(new(0, 0, Size.x, Size.y), Origin);
            Divide(root, 0);

            GenerateRoom(root, 0);
            GenerateLoadTile(root, 0);

            FillWall();
        }


        private void FillBackground() {
            for (int x = -Margin; x <= Size.x + Margin; x++) {
                for (int y = -Margin; y <= Size.y + Margin; y++) {
                    Background.SetTile(x, y);
                }
            }
        }

        private void FillRoom(Room room) {
            for (int x = room.X; x < room.X + room.Width; x++) {
                for (int y = room.Y; y < room.Y + room.Height; y++) {
                    Room.SetTile(x, y);
                    if (Random.Range(0f, 1f) < 0.03f) {
                        Obstacle.SetTile(x, y);
                    }
                }
            }
        }
        private void FillWall() {
            for (int x = 0; x < Size.x; x++) {
                for (int y = 0; y < Size.y; y++) {
                    if (!IsBackground(x, y)) continue;
                    for (int xOffset = -1; xOffset <= 1; xOffset++) {
                        for (int yOffset = -1; yOffset <= 1; yOffset++) {
                            if (xOffset == 0 && yOffset == 0) continue;
                            if (Room.GetTile(x + xOffset, y + yOffset) == null &&
                                Road.GetTile(x + xOffset, y + yOffset) == null)
                                continue;
                            Wall.SetTile(x, y);
                            break;
                        }
                    }
                }
            }
        }


        private void Divide(Node node, int n) {
            if (n >= MaxDepth) return;

            if (node.LongWidth) node.SplitHorizontal(MinDivideRate, MaxDivideRate);
            else node.SplitVertical(MinDivideRate, MaxDivideRate);

            Divide(node.Left, n + 1);
            Divide(node.Right, n + 1);
        }

        private Room GenerateRoom(Node node, int n) {
            Room room;
            if (n == MaxDepth) {
                room = node.GenerateRoom();
                FillRoom(room);
                Rooms.Add(room);
            }
            else {
                node.Left.SetRoom(GenerateRoom(node.Left, n + 1));
                node.Right.SetRoom(GenerateRoom(node.Right, n + 1));
                room = node.Left.Room;
            }
            return room;
        }
        private void GenerateLoadTile(Node node, int n) {
            if (n == MaxDepth) return;
            Vector2Int leftCenter = node.Left.Room.Center;
            Vector2Int rightCenter = node.Right.Room.Center;

            int start = Mathf.Min(leftCenter.x, rightCenter.x);
            int end = Mathf.Max(leftCenter.x, rightCenter.x);
            for (int x = start; x <= end; x++) {
                for (int k = 0; k < 3; k++) {
                    Road.SetTile(x, leftCenter.y + k);
                }
            }
            start = Mathf.Min(leftCenter.y, rightCenter.y);
            end = Mathf.Max(leftCenter.y, rightCenter.y);
            for (int y = start; y <= end; y++) {
                for (int k = 0; k < 3; k++) {
                    Road.SetTile(rightCenter.x + k, y);
                }
            }

            GenerateLoadTile(node.Left, n + 1);
            GenerateLoadTile(node.Right, n + 1);
        }

        private bool IsBackground(int x, int y) {
            if (Room.GetTile(x, y) != null) return false;
            if (Road.GetTile(x, y) != null) return false;
            if (Wall.GetTile(x, y) != null) return false;
            return true;
        }
    }
}
