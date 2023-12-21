using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public Vector2 MinPosition => Origin + new Vector2(X, Y);
        public Vector2 MaxPosition => Origin + new Vector2(X + Width, Y + Height);

        public List<Monster> monsters = new();

        public bool IsClear {
            get => _isClear;
            set {
                if (_isClear == value) return;
                _isClear = value;
                if (_isClear) OnClear?.Invoke();
            }
        }

        private bool _isClear = false;

        public event Action OnClear;

        public Room(Node node, Vector2Int origin) {
            this.Node = node;

            int width = Random.Range(Node.Width / 2, Node.Width - 1);
            int height = Random.Range(Node.Height / 2, Node.Height - 1);
            int x = Node.Rect.x + Random.Range(1, Node.Width - width);
            int y = Node.Rect.y + Random.Range(1, Node.Height - height);

            Rect = new(x, y, width, height);

            Origin = origin;

            OnClear += () => Debug.Log("방 클리어!");
        }

        public bool IsInRoom(Vector2 position) {
            if (position.x < MinPosition.x || MaxPosition.x < position.x) return false;
            if (position.y < MinPosition.y || MaxPosition.y < position.y) return false;
            return true;
        }

        public T SpawnMonster<T>(int key) where T : Monster {
            IsClear = false;
            Vector2 position = new(Random.Range(MinPosition.x, MaxPosition.x), Random.Range(MinPosition.y, MaxPosition.y));
            Monster newMonster = Main.Object.SpawnMonster<T>(key, position);
            monsters.Add(newMonster);
            newMonster.OnDead += RemoveMonster;
            if (newMonster is BossMonster boss) {
                boss.OnDead += (x) => Main.UI.ShowPopupUI<UI_Popup_Reward>();
            }
            return newMonster as T;
        }

        private void RemoveMonster(Creature monster) {
            monsters.Remove(monster as Monster);
            if (monsters.Count == 0) IsClear = true;
        }
    }
}