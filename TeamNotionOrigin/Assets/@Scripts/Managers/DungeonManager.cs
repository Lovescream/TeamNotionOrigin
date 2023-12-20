using Dungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager {

    public DungeonGenerator DungeonGenerator { get; private set; }
    public NavmeshGenerator NavmeshGenerator { get; private set; }

    public List<Room> Rooms { get; private set; }

    public void Initialize() {
        DungeonGenerator = new(
            size: new(128, 128),
            margin: 10,
            maxDepth: 2,
            minDivideRate: 0.4f,
            maxDivideRate: 0.6f);
        NavmeshGenerator = new();
    }

    public void GenerateAsync(Action<AsyncOperation> callback = null) {
        DungeonGenerator.GenerateMap();
        Rooms = DungeonGenerator.Rooms;
        NavmeshGenerator.GenerateAsync(callback);
    }

    public Room GetRandomRoom() {
        if (Rooms == null || Rooms.Count == 0) return null;
        return Rooms[UnityEngine.Random.Range(0, Rooms.Count)];
    }
}