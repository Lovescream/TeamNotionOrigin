using Data;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager
{
    private static Data.Player _player;
    public Data.Player Player { get { return _player; } }
    public void Initialize()
    {
        SetGameData();
    }

    private void SetGameData()
    {
        if (Main.Data.PlayerDict.TryGetValue(1, out Data.Player player))
        {
            _player = player;
        }

        Debug.Log(player.defence);

        if (Main.Data.ItemDict[ItemType.Passive].TryGetValue(1, out Data.Item item1) && item1 is Data.Passive passiveItem)
        {
            Debug.Log(passiveItem.name);
        }

    }
}