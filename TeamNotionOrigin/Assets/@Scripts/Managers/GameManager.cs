using Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager
{
    private static Data.Player _player;

    public Data.Player Player {  get { return _player; } }

    public void Init()
    {
        Main.Resource.LoadAllAsync<Object>("Game", (key, count, totalCount) =>
        {
            if (count >= totalCount)
            {
                // 리소스 로드가 모두 완료됨.

                SetGameData();
            }
        }
        );
    }

    private void SetGameData()
    {
        Main.Data.Init();
        if (Main.Data.PlayerDict.TryGetValue(1, out Data.Player player))
        {
            _player = player;
        }
        

        Main.Data.MonsterDict.TryGetValue(1, out Data.Monster monster);

        Debug.Log(monster.monsterType);
        Debug.Log(player.defence);

        if (Main.Data.ItemDict[ItemType.Passive].TryGetValue(1, out Data.Item item1) && item1 is Data.Passive passiveItem)
        {
            Debug.Log(passiveItem.name);
        }

    }
}