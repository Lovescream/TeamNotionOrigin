using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ObjectManager {
    //public Player Player { get; private set; }
    //public List<Enemy> Enemies { get; set; } = new();
    //private List<Projectile> Projectiles { get; set; } = new();

    public Player Player { get; private set; }
    public List<Monster> Monsters { get; private set; } = new();
    public List<Creature> Creatures { get; private set; } = new();
    public List<Projectile> Projectiles { get; private set; } = new();
    public List<PickupItem> Pickups { get; private set; } = new();
    public List<PassiveItem> Passives { get; private set; } = new();
    public List<Weapon> Weapons { get; private set; } = new();

    public Transform EnemyParent {
        get {
            GameObject root = GameObject.Find("@Enemies");
            if (root == null) root = new("@Enemies");
            return root.transform;
        }
    }

    public T Spawn<T>(int key, Vector2 position, string name = null) where T : Component {
        Type type = string.IsNullOrEmpty(name) ? typeof(T) : Type.GetType(name);
        string prefabName = GetPrefabName(type) ?? "Monster";

        GameObject obj = Main.Resource.Instantiate($"{prefabName}.prefab", pooling: true);
        obj.transform.position = position;

        T component = obj.GetOrAddComponent<T>();
        if (component is Player player) {
            Player = player;
            Player.SetInfo(Main.Data.PlayerDict[key]);
        }
        //else if (component is Monster monster) {
        //    if (obj.TryGetComponent<Monster>(out monster))
        //        UnityEngine.Object.Destroy(monster);
        //    monster = obj.AddComponent<T>() as Monster;
        //    Monsters.Add(monster);
        //    monster.SetInfo(Main.Data.MonsterDict[key]);
        //}
        else if (component is Creature creature) {
            Creatures.Add(creature);
            //creature.SetInfo(Main.Data.)
        }
        //else if (component is Projectile projectile) {
        //    Projectiles.Add(projectile);
        //    projectile.SetInfo();
        //}
        else if (component is PickupItem pickup) {
            Pickups.Add(pickup);
            if (pickup is PickupGold gold) {
                gold.SetInfo(key);
            }
            else {
                pickup.SetInfo(Main.Data.ItemDict[Data.ItemType.Pickup][key]);
            }
        }
        else if (component is PassiveItem passive) {
            Passives.Add(passive);
            passive.SetInfo(Main.Data.ItemDict[Data.ItemType.Passive][key]);
        }
        else if (component is Weapon weapon) {
            Weapons.Add(weapon);
            weapon.SetInfo(Main.Data.ItemDict[Data.ItemType.Weapon][key]);
        }
        //else if (component is SpreadBullet spreadbullet)
        //{
        //    Projectiles.Add(spreadbullet);
        //    spreadbullet.SetInfo();
        //}
        //else if (component is CanonBullet canonBullet)
        //{
        //    Projectiles.Add(canonBullet);
        //    canonBullet.SetInfo();
        //}

        return component;
    }

    public T SpawnMonster<T>(int key, Vector2 position) where T : Monster
    {
        Type type = typeof(T);
        string prefabName = GetPrefabName(type) ?? "Monster";
        GameObject obj = Main.Resource.Instantiate($"{prefabName}.prefab", pooling: true);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.transform.position = position;
        obj.GetComponent<NavMeshAgent>().enabled = true;
        if (obj.TryGetComponent<Monster>(out var old))
            UnityEngine.Object.Destroy(old);
        var newComponent = obj.AddComponent<T>() as Monster;
        Monsters.Add(newComponent);
        newComponent.SetInfo(Main.Data.MonsterDict[key]);
        return newComponent as T;
    }

    public T SpawnProjectile<T>(Vector2 triggerPosition, Vector2 dir, float velocity, int layer, float damage) where T : Projectile
    {
        Type type = typeof(T);
        string prefabName = GetPrefabName(type) ?? "Projectile";
        GameObject obj = Main.Resource.Instantiate($"{prefabName}.prefab", pooling: true);
        obj.transform.position = triggerPosition;
        if (obj.TryGetComponent<Projectile>(out var old))
            UnityEngine.Object.Destroy(old);
        var newComponent = obj.AddComponent<T>() as Projectile;
        Projectiles.Add(newComponent);
        newComponent.SetInfo(triggerPosition, dir, velocity, layer, damage);
        return newComponent as T;
    }

    public void Despawn<T>(T obj) where T : Component {
        if (!obj.gameObject.IsValid()) return;
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
        else if (obj is PickupItem pickup) {
            Pickups.Remove(pickup);
        }
        else if (obj is PassiveItem passive) {
            Passives.Remove(passive);
        }
        else if (obj is Weapon weapon) {
            Weapons.Remove(weapon);
        }

        Main.Resource.Destroy(obj.gameObject);
    }

    /// <summary>
    /// Type에 따른 프리팹 이름을 결정합니다.
    /// Ex) type = MeleeMonster라면,
    /// "MeleeMonster.prefab"이 존재한다면 => return "MeleeMonster";
    /// 그렇지 않다면 부모 클래스 이름인 "Monster.prefab"이 존재한다면 => return "Monster";
    /// 그렇지 않다면 부모 클래스 이름인 "Creature.prefab"이 존재한다면 => return "Creature";
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetPrefabName(Type type) {
        string prefabName;
        while (type != null) {
            prefabName = type.Name;
            if (Main.Resource.IsExist($"{prefabName}.prefab")) return prefabName;
            type = type.BaseType;
        }
        return null;
    }
}

