using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon {
    public class TileMap {

        public Tilemap Map { get; private set; }
        public Vector2Int Origin { get; private set; }
        public Tile Tile { get; private set; }
        public Tile[] TileSet { get; private set; }
        public TileMap(string name, Grid grid, Vector2Int origin, int order, bool needCollider, string tileName = null) {
            Map = new GameObject(name).AddComponent<Tilemap>();
            Map.gameObject.AddComponent<TilemapRenderer>().sortingOrder = order;
            if (needCollider) Map.gameObject.AddComponent<TilemapCollider2D>();
            Map.transform.SetParent(grid.transform);
            Origin = origin;
            if (!string.IsNullOrEmpty(tileName))
                Tile = Main.Resource.Load<Tile>(tileName);
        }
        public void SetTile(Tile[] tiles) => TileSet = tiles;

        public TileBase GetTile(int x, int y) {
            return Map.GetTile(new(Origin.x + x, Origin.y + y));
        }
        public void SetTile(int x, int y, Tile tile = null) {
            if (tile == null) {
                tile = Tile;
                if (tile == null) {
                    tile = TileSet[Random.Range(0, TileSet.Length)];
                }
            }
            Map.SetTile(new(Origin.x + x, Origin.y + y), tile);
        }

    }
}