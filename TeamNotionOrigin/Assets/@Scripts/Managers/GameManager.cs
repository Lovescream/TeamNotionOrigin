using System.Collections;
using System.Collections.Generic;
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
    }
}