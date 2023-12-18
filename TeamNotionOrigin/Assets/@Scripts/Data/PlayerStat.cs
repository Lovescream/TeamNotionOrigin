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
        public float exp;
    }

    [System.Serializable]
    public class PlayerData : ILoader<PlayerType, Player>
    {
        public List<Player> players = new List<Player>();

        public Dictionary<PlayerType, Player> MakeDict()
        {
            Dictionary<PlayerType, Player> playerDict = new Dictionary<PlayerType, Player>();

            foreach (Player player in players)
            {
                playerDict.Add(player.playerType, player);
            }

            return playerDict;
        }
    }
}