using Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager {

    public DungeonGenerator Generator { get; private set; }

    public List<Room> Rooms { get; private set; }

    public void Initialize() {
        Generator = new(
            size: new(128, 128),
            margin: 10,
            maxDepth: 2,
            minDivideRate: 0.4f,
            maxDivideRate: 0.6f);
    }

    public void Generate() {
        Generator.GenerateMap();
        Rooms = Generator.Rooms;
    }

    public void SpawnMonsters() {
        foreach (Room room in Rooms) {
            for (int i = 0; i < Random.Range(1, 3); i++)
                room.SpawnMonster<RangedMonster>(1);
            for (int i = 0; i < Random.Range(1, 3); i++)
                room.SpawnMonster<MeleeMonster>(1);
        }
    }

    public Room GetRandomRoom() {
        if (Rooms == null || Rooms.Count == 0) return null;
        return Rooms[Random.Range(0, Rooms.Count)];
    }
    public Room GetRoom(Vector2 position) {
        for (int i = 0; i < Rooms.Count; i++) {
            if (Rooms[i].IsInRoom(position)) return Rooms[i];
        }
        return null;
    }
}