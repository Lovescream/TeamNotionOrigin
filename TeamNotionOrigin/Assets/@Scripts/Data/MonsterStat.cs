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
    public class MonsterData : ILoader<string, Monster>
    {
        public List<Monster> monsters = new List<Monster>();

        public Dictionary<string, Monster> MakeDict()
        {
            Dictionary<string, Monster> monsterDict = new Dictionary<string, Monster>();

            foreach (Monster monster in monsters)
            {
                monsterDict.Add(monster.name, monster);
            }

            return monsterDict;
        }
    }
}