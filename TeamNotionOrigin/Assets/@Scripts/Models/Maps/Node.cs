using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon {
    public class Node {
        public Node Parent { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }
        
        public Vector2Int Origin { get; private set; }
        public RectInt Rect { get; private set; }
        public Room Room { get; private set; }
        
        public int Width => Rect.width;
        public int Height => Rect.height;
        public bool LongWidth => Rect.width >= Rect.height;
        
        public Node(RectInt rect, Vector2Int origin) {
            Rect = rect;
            Origin = origin;
        }
        public Node(Node parent, RectInt rect) {
            Parent = parent;
            Rect = rect;
            Origin = parent.Origin;
        }
        
        public void SplitHorizontal(float minDivideRate, float maxDivideRate) {
            int split = Mathf.RoundToInt(Random.Range(Width * minDivideRate, Width * maxDivideRate));
            Left = new(this, new(Rect.x, Rect.y, split, Height));
            Right = new(this, new(Rect.x + split, Rect.y, Width - split, Height));
        }
        public void SplitVertical(float minDivideRate, float maxDivideRate) {
            int split = Mathf.RoundToInt(Random.Range(Height * minDivideRate, Height * maxDivideRate));
            Left = new(this, new(Rect.x, Rect.y, Width, split));
            Right = new(this, new(Rect.x, Rect.y + split, Width, Height - split));
        }

        public Room GenerateRoom() {
            Room = new Room(this, Origin);
            return Room;
        }
        public Room SetRoom(Room room) {
            Room = room;
            return Room;
        }
    }
}