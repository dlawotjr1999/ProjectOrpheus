using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUIScript : MonoBehaviour
{
    private RectTransform m_rect;
    private TextMeshProUGUI m_nameTxt;
    private TextMeshProUGUI m_descTxt;
    private TextMeshProUGUI m_elseTxt;

    private bool IsCompsSet { get; set; }

    private readonly float[] UIHeight = new float[4] { 310, 310, 270, 238 };
    private readonly float UIWidth = 360;

    public void ShowUI(SItem _item)
    {
        if (!IsCompsSet) { SetComps(); }
        ItemInfo info = GameManager.GetItemInfo(_item);
        gameObject.SetActive(true);
        m_nameTxt.text = info.ItemName;
        m_descTxt.text = info.ItemDescription;
        m_elseTxt.text = SetItemInfo(info);
        SetUIHeight(info.ItemType);
    }
    public string SetItemInfo(ItemInfo _info)
    {
        string itemTxt = "";

        switch (_info.ItemType)
        {
            case EItemType.WEAPON:
                WeaponScriptable weapon = _info.ItemData as WeaponScriptable;
                itemTxt = $"공격력 : {weapon.Attack.Min} ~ {weapon.Attack.Max}\n주술력 : {weapon.Magic.Min} ~ {weapon.Magic.Max}\n공격속도 : {weapon.AttackSpeed}";
                break;
            case EItemType.THROW:
                ThrowItemScriptable throwItem = _info.ItemData as ThrowItemScriptable;
                itemTxt = $"데미지 : {throwItem.ThrowDamage}\n비행속도 : {throwItem.ThrowSpeed}\n폭발 딜레이 : {throwItem.ExplodeTime}";
                break;
            case EItemType.PATTERN:
                PatternScriptable pattern = _info.ItemData as PatternScriptable;
                itemTxt = $"회복량  : {pattern.HealAmount}";
                break;
            case EItemType.OTHERS:
                OtherItemScriptable other = _info.ItemData as OtherItemScriptable;
                itemTxt = "";
                break;
        }

        return itemTxt;
    }
    private void SetUIHeight(EItemType _type)
    {
        m_rect.sizeDelta = new(UIWidth, UIHeight[(int)_type]);
    }
    public void SetPos(Vector2 _pos)
    {
        m_rect.anchoredPosition = _pos;
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_nameTxt = txts[0];
        m_descTxt = txts[1];
        m_elseTxt = txts[2];
        IsCompsSet = true;
    }

    private void Awake()
    {
        if(!IsCompsSet) { SetComps(); }
    }
}
