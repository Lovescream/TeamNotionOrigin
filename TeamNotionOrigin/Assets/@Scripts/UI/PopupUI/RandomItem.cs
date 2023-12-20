using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomItem : UI_Popup
{
    enum Texts
    {
        FirstItemCost,
        SecondItemCost,
        ThirdItemCost,
        CurrentGoldTxt,
        RerollCostTxt,
    }

    enum Images
    {
        FirstItemIcon,
        SecondItemIcon,
        ThirdItemIcon,
        AlertBlock,
    }
    enum Buttons
    {
        ReloadItemBtn,
    }


    private Image firstItemImage;
    private Image secondItemImage;
    private Image thirdItemImage;
    private Image alertModal;
    private TextMeshProUGUI currentGoldTxt;
    private TextMeshProUGUI firstItemCostTxt;
    private TextMeshProUGUI secondItemCostTxt;
    private TextMeshProUGUI thirdItemCostTxt;
    private TextMeshProUGUI rerollCostTxt;
    private Button reloadItemBtn;
    private int basicRerollCost = 10;

    public List<Data.Item> items = new List<Data.Item>();
    List<Data.Item> randomItems = new List<Data.Item>();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        firstItemImage = GetImage((int)Images.FirstItemIcon);
        secondItemImage = GetImage((int)Images.SecondItemIcon);
        thirdItemImage = GetImage((int)Images.ThirdItemIcon);

        firstItemCostTxt = GetText((int)Texts.FirstItemCost);
        secondItemCostTxt = GetText((int)Texts.SecondItemCost);
        thirdItemCostTxt = GetText((int)Texts.ThirdItemCost);
        currentGoldTxt = GetText((int)Texts.CurrentGoldTxt);
        rerollCostTxt = GetText((int)Texts.RerollCostTxt);

        SelectThreeRandomValue(randomItems);
        UpdatePlayerGold(0);

        for (int i = 1; i <= Main.Data.ItemDict[Data.ItemType.Weapon].Count; i++)
        {
            //기본 무기 제외
            if (Main.Data.ItemDict[Data.ItemType.Weapon][i - 1].id == 6) continue;
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][i - 1]);
        }

        for (int i = 1; i <= Main.Data.ItemDict[Data.ItemType.Passive].Count; i++)
        {
            items.Add(Main.Data.ItemDict[Data.ItemType.Passive][i]);
        }

        rerollCostTxt.text = $"Reroll\n{10 * 1} G";
        reloadItemBtn.onClick.AddListener(() => ReloadItem(basicRerollCost));

        return true;
    }

    private void UpdatePlayerGold(int cost)
    {
        //p1.Data.gold -= cost;
        //{p1.Data.gold}
        currentGoldTxt.text = $" G";
    }
    private void ReloadItem(int cost)
    {
        if (!CheckGoldForBuyItem(cost)) return;

        UpdatePlayerGold(cost);
        SelectThreeRandomValue(randomItems);
    }
    private void SelectThreeRandomValue(List<Data.Item> randomItems)
    {
        randomItems.Clear();
        List<int> possibleValues = new List<int>();
        for (int i = 0; i < items.Count; i++)
        {
            possibleValues.Add(i);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, possibleValues.Count);
            int selectedValue = possibleValues[randomIndex];
            randomItems.Add(items[selectedValue]);
            possibleValues.RemoveAt(randomIndex);
        }


        firstItemImage.sprite = randomItems[0].itemSprite;
        secondItemImage.sprite = randomItems[1].itemSprite;
        thirdItemImage.sprite = randomItems[2].itemSprite;
        firstItemCostTxt.text = randomItems[0].cost.ToString() + " G";
        secondItemCostTxt.text = randomItems[1].cost.ToString() + " G";
        thirdItemCostTxt.text = randomItems[2].cost.ToString() + " G";
    }

    private bool CheckGoldForBuyItem(int cost)
    {
        //p1.Data.gold
        if (100 < cost)
        {
            alertModal.gameObject.SetActive(true);
            return false;
        }
        else return true;
    }
}
