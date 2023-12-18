using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public enum MonsterType
    {
        Normal,
        Boss
    }

    [System.Serializable]
    public class Monster : Creature
    {
        public string name;
        public MonsterType monsterType;
    }

    [System.Serializable]
    public class MonsterData : ILoader<int, Monster>
    {
        public List<Monster> monsters = new List<Monster>();

        public Dictionary<int, Monster> MakeDict()
        {
            Dictionary<int, Monster> monsterDict = new Dictionary<int, Monster>();

            foreach (Monster monster in monsters)
            {
                monsterDict.Add(monster.id, monster);
            }

            return monsterDict;
        }
    }
}