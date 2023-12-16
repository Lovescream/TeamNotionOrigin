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
public class Player : CreatureStat
{
    private PlayerType _playerType;
    private float _damage;
    private float _defence;
    private float _speed;
    private float _critical;
    private float _level;
    private float _gold;

}
}