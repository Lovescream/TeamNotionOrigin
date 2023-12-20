using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon {
    public class Room {

        public RectInt Rect { get; private set; }
        public Node Node { get; private set; }
        public Vector2Int Origin { get; private set; }

        public int X => Rect.x;
        public int Y => Rect.y;
        public int Width => Rect.width;
        public int Height => Rect.height;
        

        public Vector2Int Center => new(Rect.x + Rect.width / 2, Rect.y + Rect.height / 2);
        public Vector2 CenterPosition => Origin + new Vector2(Rect.x + Rect.width / 2f, Rect.y + Rect.height / 2f);

        public Room(Node node, Vector2Int origin) {
            this.Node = node;

            int width = Random.Range(Node.Width / 2, Node.Width - 1);
            int height = Random.Range(Node.Height / 2, Node.Height - 1);
            int x = Node.Rect.x + Random.Range(1, Node.Width - width);
            int y = Node.Rect.y + Random.Range(1, Node.Height - height);

            Rect = new(x, y, width, height);

            Origin = origin;
        }
    }
}