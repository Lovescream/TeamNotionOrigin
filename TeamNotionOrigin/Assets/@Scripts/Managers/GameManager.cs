using Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GameManager
{
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

        foreach (Data.Weapon weapon in Main.Data.Weapons )
        {
            Debug.Log(weapon.name);
        }

        foreach (Data.Passive item in Main.Data.ItemDict[ItemType.Passive])
        {
            Debug.Log(item.stackable);
        }
    }
}