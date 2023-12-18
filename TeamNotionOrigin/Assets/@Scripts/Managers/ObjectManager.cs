using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {
    //public Player Player { get; private set; }
    //public List<Enemy> Enemies { get; set; } = new();
    //private List<Projectile> Projectiles { get; set; } = new();

    public Player Player { get; private set; }
    public List<Monster> Monsters { get; private set; } = new();
    public List<Creature> Creatures { get; private set; } = new();
    public List<Projectile> Projectiles { get; private set; } = new();

    public Transform EnemyParent {
        get {
            GameObject root = GameObject.Find("@Enemies");
            if (root == null) root = new("@Enemies");
            return root.transform;
        }
    }

    public T Spawn<T>(int key, Vector2 position) where T : Component {
        Type type = typeof(T);
        string prefabName = type.Name;

        GameObject obj = Main.Resource.Instantiate($"{prefabName}.prefab", pooling: true);
        obj.transform.position = position;

        T component = obj.GetOrAddComponent<T>();
        if (component is Player player) {
            Player = player;
            Player.SetInfo(Main.Data.PlayerDict[key]);
        }
        else if (component is Monster monster) {
            Monsters.Add(monster);
            monster.SetInfo(Main.Data.MonsterDict[key]);
        }
        else if (component is Creature creature) {
            Creatures.Add(creature);
            //creature.SetInfo(Main.Data.)
        }
        else if (component is Projectile projectile) {
            Projectiles.Add(projectile);
            projectile.SetInfo();
        }

        return null;
    }
    
    public void Despawn<T>(T obj) where T : Component {
        if (obj is Player player) {
            Player = null;
        }
        else if (obj is Monster monster) {
            Monsters.Remove(monster);
        }
        else if (obj is Creature creature) {
            Creatures.Remove(creature);
        }
        else if (obj is Projectile projectile) {
            Projectiles.Remove(projectile);
        }

        Main.Resource.Destroy(obj.gameObject);
    }
}

