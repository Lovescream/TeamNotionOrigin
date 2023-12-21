using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Popup_Reward : UI_Popup
{
    #region Enum
    enum Texts
    {
        FirstItemCost,
        SecondItemCost,
        ThirdItemCost,
        CurrentGoldTxt,
        ATK,
        ATKSpeed,
        ReloadTime,
        Critical,
        MaxBulletAmount,
        MagazineCapacity,
        RerollCostTxt,
    }

    enum Images
    {
        FirstItemIcon,
        SecondItemIcon,
        ThirdItemIcon,
        InfoPanel,
        Block
    }

    enum Buttons
    {
        ReloadItemBtn,
        NextStageBtn
    }
    #endregion

    public List<Data.Item> items = new List<Data.Item>();
    List<Data.Weapon> selectedValues = new List<Data.Weapon>();
    public Player p1;
    private int basicRerollCost = 10;
    private Image firstItemImage;
    private Image secondItemImage;
    private Image thirdItemImage;
    private Image itemInfoTooltip;
    private Image alertModal;
    private TextMeshProUGUI attackTxt;
    private TextMeshProUGUI attackSpeedTxt;
    private TextMeshProUGUI reloadSpeedTxt;
    private TextMeshProUGUI criticalTxt;
    private TextMeshProUGUI maxBulletTxt;
    private TextMeshProUGUI magazineCapacityTxt;
    private TextMeshProUGUI firstItemCostTxt;
    private TextMeshProUGUI secondItemCostTxt;
    private TextMeshProUGUI thirdItemCostTxt;
    private TextMeshProUGUI rerollCostTxt;
    private TextMeshProUGUI currentGoldTxt;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        firstItemImage = GetImage((int)Images.FirstItemIcon);
        secondItemImage = GetImage((int)Images.SecondItemIcon);
        thirdItemImage = GetImage((int)Images.ThirdItemIcon);
        alertModal = GetImage((int)Images.Block);
        itemInfoTooltip = GetImage((int)Images.InfoPanel);

        attackTxt = GetText((int)Texts.ATK);
        attackSpeedTxt = GetText((int)Texts.ATKSpeed);
        reloadSpeedTxt = GetText((int)Texts.ReloadTime);
        criticalTxt = GetText((int)Texts.Critical);
        maxBulletTxt = GetText((int)Texts.MaxBulletAmount);
        magazineCapacityTxt = GetText((int)Texts.MagazineCapacity);

        firstItemCostTxt = GetText((int)Texts.FirstItemCost);
        secondItemCostTxt = GetText((int)Texts.SecondItemCost);
        thirdItemCostTxt = GetText((int)Texts.ThirdItemCost);
        rerollCostTxt = GetText((int)Texts.RerollCostTxt);

        p1 = Main.Object.Player;
        //Status status = p1.Status;
        rerollCostTxt.text = $"Reroll\n{basicRerollCost * 1}";

        for (int i = 1; i <= Main.Data.ItemDict[Data.ItemType.Weapon].Count; i++)
        {
            ////기본 무기 제외
            //if (Main.Data.ItemDict[Data.ItemType.Weapon][i - 1].id == 6) continue;
            //items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][i-1]);
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][10]);
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][11]);
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][12]);
        }


        SelectThreeRandomValue(selectedValues);

        AddUIEvent(firstItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(firstItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);
        firstItemImage.GetComponent<Button>().onClick.AddListener(() => SelectItem(1));
        AddUIEvent(secondItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(secondItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);
        secondItemImage.GetComponent<Button>().onClick.AddListener(() => SelectItem(2));
        AddUIEvent(thirdItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(thirdItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);
        thirdItemImage.GetComponent<Button>().onClick.AddListener(() => SelectItem(3));

        //AddUIEvent(firstItemImage.gameObject,)

        GetButton((int)Buttons.ReloadItemBtn).onClick.AddListener(() => ReloadItem(basicRerollCost));
        return true;
    }

    private void SelectThreeRandomValue(List<Data.Weapon> selectedValues)
    {
        selectedValues.Clear();
        List<int> possibleValues = new List<int>();
        for (int i = 0; i < items.Count; i++)
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

        firstItemImage.name = $"{selectedValues[0].id}";
        secondItemImage.name = $"{selectedValues[1].id}";
        thirdItemImage.name = $"{selectedValues[2].id}";
        firstItemImage.sprite = Main.Resource.Load<Sprite>($"{selectedValues[0].itemType}_{selectedValues[0].id}.sprite");
        secondItemImage.sprite = Main.Resource.Load<Sprite>($"{selectedValues[1].itemType}_{selectedValues[1].id}.sprite");
        thirdItemImage.sprite = Main.Resource.Load<Sprite>($"{selectedValues[2].itemType}_{selectedValues[2].id}.sprite");
        firstItemCostTxt.text = selectedValues[0].cost.ToString() + " G";
        secondItemCostTxt.text = selectedValues[1].cost.ToString() + " G";
        thirdItemCostTxt.text = selectedValues[2].cost.ToString() + " G";
    }

    private void ReloadItem(int cost)
    {
        //currentGoldTxt.text = $"{p1.Data.gold - cost} G";
        SelectThreeRandomValue(selectedValues);
    }

    private void ShowItemInfo(PointerEventData eventData)
    {
        GameObject go = itemInfoTooltip.gameObject;
        RectTransform rt = go.GetComponent<RectTransform>();
        float halfWidth = go.GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.5f;
        go.SetActive(true);
        go.transform.position = new Vector2(eventData.position.x, eventData.position.y);

        if (rt.anchoredPosition.y + rt.sizeDelta.y - 100 > halfWidth)
            rt.pivot = new Vector2(-0.1f, 1.1f);
        else
            rt.pivot = new Vector2(-0.1f, 0.1f);
        GameObject hoveredObject = eventData.pointerEnter;
        SetInfoItemTooltip(hoveredObject);
    }
    
    private void CloseItemInfo(PointerEventData eventData)
    {
        itemInfoTooltip.gameObject.SetActive(false);
    }
    private void SelectItem(int index) {
        GameObject obj = index switch {
            1 => firstItemImage.gameObject,
            2 => secondItemImage.gameObject,
            3 => thirdItemImage.gameObject,
            _ => null
        };
        Data.Weapon weapon = selectedValues.FirstOrDefault(weapon => weapon.id.ToString() == obj.name);
        if (weapon == null) return;
        Main.Object.Player.Inventory.Add(Main.Object.Spawn<Weapon>(weapon.id, Vector2.zero, weapon.name));
        ClosePopup();
    }

    private void SetInfoItemTooltip(GameObject hoveredObject)
    {
        //Data.Weapon weapon = selectedValues.FirstOrDefault(weapon => weapon.itemSprite.name == hoveredObject.GetComponent<Image>().sprite.name);
        Data.Weapon weapon = selectedValues.FirstOrDefault(weapon => weapon.id.ToString() == hoveredObject.transform.name);
        attackTxt.text = $"공격력 : {weapon.damage}";
        attackSpeedTxt.text = $"공격속도 : {weapon.attackSpeed}";
        reloadSpeedTxt.text = $"장전속도 : {weapon.reloadTime}";
        criticalTxt.text = $"크리티컬 확률 : {weapon.critical}";
        maxBulletTxt.text = $"총 탄환 수 : {weapon.maxBulletAmount}";
        magazineCapacityTxt.text = $"탄창 수용력 : {weapon.magazineCapacity}";
    }

    public override void ClosePopup() {
        base.ClosePopup();
        Time.timeScale = 1;
    }
}
