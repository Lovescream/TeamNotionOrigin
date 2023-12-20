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

    public Room GetRandomRoom() {
        if (Rooms == null || Rooms.Count == 0) return null;
        return Rooms[Random.Range(0, Rooms.Count)];
    }
}