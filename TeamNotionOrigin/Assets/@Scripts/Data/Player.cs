using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    public enum PlayerType
    {
        Warrior,
        Mage,
    }

    [System.Serializable]
    public class Player : Creature
    {
        public PlayerType playerType;
        public float critical;
        public int level;
    }

    [System.Serializable]
    public class PlayerData : ILoader<int, Player>
    {
        public List<Player> players = new List<Player>();

        public Dictionary<int, Player> MakeDict()
        {
            Dictionary<int, Player> playerDict = new Dictionary<int, Player>();

            foreach (Player player in players)
            {
                playerDict.Add(player.id, player);
            }

            return playerDict;
        }
    }
}