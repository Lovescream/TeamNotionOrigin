using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatInfo : UI_Popup
{
    #region Enum
    enum Texts
    {
        Hp,
        Damage,
        Defence,
        MoveSpeed,
        Critical,
        AttackSpeed,
        BulletSizeX,
        BulletSizeY,
    }

    enum Images
    {

    }

    enum Buttons
    {

    }
    #endregion

    private TextMeshProUGUI _hp;
    private TextMeshProUGUI _damage;
    private TextMeshProUGUI _defence;
    private TextMeshProUGUI _moveSpeed;
    private TextMeshProUGUI _critical;
    private TextMeshProUGUI _attackSpeed;
    private TextMeshProUGUI _bulletSizeX;
    private TextMeshProUGUI _bulletSizeY;
    private Status status;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));

        //GetText((int)Texts.Hp).text = $"체력 : {status[StatType.Hp].Value}";
        //GetText((int)Texts.Damage).text = $"공격력 : {status[StatType.Damage].Value}";
        //GetText((int)Texts.Defence).text = $"방어력 : {status[StatType.Defense].Value}";
        //GetText((int)Texts.MoveSpeed).text = $"이동속도 : {status[StatType.Speed].Value}";
        //GetText((int)Texts.Critical).text = $"크리티컬 확률 : {status[StatType.Critical].Value}";
        //GetText((int)Texts.AttackSpeed).text = $"공격속도 : {status[StatType.AttackSpeed].Value}";
        //GetText((int)Texts.BulletSizeX).text = $"탄환X크기 : {status[StatType.BulletSizeX].Value}";
        //GetText((int)Texts.BulletSizeY).text= $"탄환Y크기 : {status[StatType.BulletSizeY].Value}";

        GetText((int)Texts.Hp).text = $"체력 : 10";
        GetText((int)Texts.Damage).text = $"공격력 : 1";
        GetText((int)Texts.Defence).text = $"방어력 : 1";
        GetText((int)Texts.MoveSpeed).text = $"이동속도 : 1";
        GetText((int)Texts.Critical).text = $"크리티컬 확률 : 1";
        GetText((int)Texts.AttackSpeed).text = $"공격속도 : 1";
        GetText((int)Texts.BulletSizeX).text = $"탄환X크기 : 1";
        GetText((int)Texts.BulletSizeY).text = $"탄환Y크기 : 1";

        return true;
    }
}
