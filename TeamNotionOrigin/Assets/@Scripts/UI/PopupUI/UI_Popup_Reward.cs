using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        AlertBlock,
        ConfirmBlock,
        SelectedItemImage,
        Slot3ItemImage,
        Slot2ItemImage,
        Slot1ItemImage,
        RandomItemPanel
    }

    enum Buttons
    {
        ReloadItemBtn,
        NextStageBtn,
        ConfirmBtn
    }
    #endregion

    public List<Data.Item> items = new List<Data.Item>();
    List<Data.Weapon> selectedValues = new List<Data.Weapon>();
    public Player p1;
    private int basicRerollCost = 10;
    private Data.Weapon purchageWeapon;
    private Image firstItemImage;
    private Image secondItemImage;
    private Image thirdItemImage;
    private Image itemInfoTooltip;
    private Image alertModal;
    private Image confirmModal;
    private Image selectedItemIcon;
    private Image slot3ItemIcon;
    private Image slot2ItemIcon;
    private Image slot1ItemIcon;
    private Image randomItemPanel;
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
    private Button itemPurchaseConfirmBtn;
    private Button reloadItemBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        firstItemImage = GetImage((int)Images.FirstItemIcon);
        secondItemImage = GetImage((int)Images.SecondItemIcon);
        thirdItemImage = GetImage((int)Images.ThirdItemIcon);
        alertModal = GetImage((int)Images.AlertBlock);
        itemInfoTooltip = GetImage((int)Images.InfoPanel);
        confirmModal = GetImage((int)Images.ConfirmBlock);
        selectedItemIcon = GetImage((int)Images.SelectedItemImage);
        slot3ItemIcon = GetImage((int)Images.Slot3ItemImage);
        slot2ItemIcon = GetImage((int)Images.Slot2ItemImage);
        slot1ItemIcon = GetImage((int)Images.Slot1ItemImage);
        randomItemPanel = GetImage((int)Images.RandomItemPanel);

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
        currentGoldTxt = GetText((int)Texts.CurrentGoldTxt);

        itemPurchaseConfirmBtn = GetButton((int)Buttons.ConfirmBtn);
        reloadItemBtn = GetButton((int)Buttons.ReloadItemBtn);

        //테스트를 위한 스폰
        Main.Object.Spawn<Player>(1, new Vector2(0,0));
        p1 = Main.Object.Player;
        Status status = new Status(p1.Data);

        
        rerollCostTxt.text = $"Reroll\n{basicRerollCost * 1} G";
        UpdatePlayerGold(0);

        for (int i = 1; i <= Main.Data.ItemDict[Data.ItemType.Weapon].Count; i++)
        {
            //기본 무기 제외
            if (Main.Data.ItemDict[Data.ItemType.Weapon][i - 1].id == 6) continue;
            items.Add(Main.Data.ItemDict[Data.ItemType.Weapon][i-1]);
        }


        SelectThreeRandomValue(selectedValues);

        AddUIEvent(firstItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(firstItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);
        AddUIEvent(secondItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(secondItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);
        AddUIEvent(thirdItemImage.gameObject, ShowItemInfo, Define.UIEvent.Hover);
        AddUIEvent(thirdItemImage.gameObject, CloseItemInfo, Define.UIEvent.Detach);

        AddUIEvent(firstItemImage.gameObject, ClickItem, Define.UIEvent.Click);
        AddUIEvent(secondItemImage.gameObject, ClickItem, Define.UIEvent.Click);
        AddUIEvent(thirdItemImage.gameObject, ClickItem, Define.UIEvent.Click);

        //컴포넌트 반환도
        Utilities.GetOrAddComponent<ItemDragAndDrop>(slot1ItemIcon.gameObject);
        Utilities.GetOrAddComponent<ItemDragAndDrop>(slot2ItemIcon.gameObject);
        Utilities.GetOrAddComponent<ItemDragAndDrop>(slot3ItemIcon.gameObject);

        reloadItemBtn.onClick.AddListener(() => ReloadItem(basicRerollCost));
        itemPurchaseConfirmBtn.onClick.AddListener(() => AddRewardItem());

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

        firstItemImage.sprite = selectedValues[0].itemSprite;
        secondItemImage.sprite = selectedValues[1].itemSprite;
        thirdItemImage.sprite = selectedValues[2].itemSprite;
        firstItemCostTxt.text = selectedValues[0].cost.ToString() + " G";
        secondItemCostTxt.text = selectedValues[1].cost.ToString() + " G";
        thirdItemCostTxt.text = selectedValues[2].cost.ToString() + " G";
    }

    private void ReloadItem(int cost)
    {
        if (!CheckGoldForBuyItem(cost)) return;

        UpdatePlayerGold(cost);
        SelectThreeRandomValue(selectedValues);
    }

    private void SetInfoItemTooltip(GameObject hoveredObject)
    {
        Data.Weapon weapon = selectedValues.FirstOrDefault(weapon => weapon.itemSprite.name == hoveredObject.GetComponent<Image>().sprite.name);
        attackTxt.text = $"공격력 : {weapon.damage}";
        attackSpeedTxt.text = $"공격속도 : {weapon.attackSpeed}";
        reloadSpeedTxt.text = $"장전속도 : {weapon.reloadTime}";
        criticalTxt.text = $"크리티컬 확률 : {weapon.critical}";
        maxBulletTxt.text = $"총 탄환 수 : {weapon.maxBulletAmount}";
        magazineCapacityTxt.text = $"탄창 수용력 : {weapon.magazineCapacity}";
    }

    private void UpdatePlayerGold(int cost)
    {
        p1.Data.gold -= cost;
        currentGoldTxt.text = $"{p1.Data.gold} G";
    }

    private bool CheckGoldForBuyItem(int cost)
    {
        if (p1.Data.gold < cost)
        {
            alertModal.gameObject.SetActive(true);
            return false;
        }
        else return true;
    }
    #region EventCallbackMethod
    private void CloseItemInfo(PointerEventData eventData)
    {
        itemInfoTooltip.gameObject.SetActive(false);
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

    private void ClickItem(PointerEventData eventData)
    {
        GameObject go = eventData.pointerClick;
        Image image = go.GetComponent<Image>();

        purchageWeapon = selectedValues.FirstOrDefault(value => value.itemSprite == image.sprite);
        
        if (purchageWeapon != null)
        {
            if (!CheckGoldForBuyItem((int)purchageWeapon.cost)) return;

            selectedItemIcon.sprite = purchageWeapon.itemSprite;
            confirmModal.gameObject.SetActive(true);
        }
    }

    private void AddRewardItem()
    {
        slot3ItemIcon.sprite = purchageWeapon.itemSprite;
        slot3ItemIcon.color = Color.white;
        UpdatePlayerGold((int)purchageWeapon.cost);
        confirmModal.gameObject.SetActive(false);
        randomItemPanel.gameObject.SetActive(false);
        reloadItemBtn.gameObject.SetActive(false);
    }

    #endregion
}
