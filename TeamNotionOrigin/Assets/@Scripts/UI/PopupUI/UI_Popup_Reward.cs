using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Reward : UI_Popup
{
    enum Texts
    {
        FirstItemConst,
        SecondItemCost,
        ThirdItemCost
    }

    enum Images
    {
        FirstItemIcon,
        SecondItemIcon,
        ThirdItemIcon
    }

    enum Buttons
    {
        ReloadItemBtn,
        NextStageBtn
    }
    public List<Data.Item> items = new List<Data.Item>();
    public Player p1;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        p1 = Main.Object.Player;

        for (int i = 1; i <= Main.Data.ItemDict[Data.ItemType.Weapon].Count; i++)
        {
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][i]);
        }
        List<Data.Weapon> selectedValues = new List<Data.Weapon>();
        SelectThreeRandomValue(selectedValues);


        GetImage((int)Images.FirstItemIcon).sprite = selectedValues[0].itemSprite;
        GetImage((int)Images.SecondItemIcon).sprite = selectedValues[1].itemSprite;
        GetImage((int)Images.ThirdItemIcon).sprite = selectedValues[2].itemSprite;

        GetButton((int)Buttons.ReloadItemBtn).onClick.AddListener(() =>  ReloadItem()); 
        return true;
    }

    private void SelectThreeRandomValue(List<Data.Weapon> selectedValues)
    {
        List<int> possibleValues = new List<int>();
        for (int i = 1; i <= items.Count; i++)
        {
            possibleValues.Add(i);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, possibleValues.Count);
            int selectedValue = possibleValues[randomIndex];
            selectedValues.Add(items[selectedValue] as Data.Weapon);

            possibleValues.RemoveAt(randomIndex);
        }
    }

    private void ReloadItem()
    {
        Debug.Log("Reload");
    }
}
