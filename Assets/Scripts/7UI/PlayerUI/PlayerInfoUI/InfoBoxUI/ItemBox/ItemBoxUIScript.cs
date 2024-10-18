using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxUIScript : PlayerInfoBoxScript
{
    private ThrowItemSlotScript m_throwItemSlot;
    private AllItemBoxScript m_allItemBox;

    public static readonly float ElmCritY = 100;
    public static readonly float ElmCloseRange = 50;

    public override void InitUI()
    {
        UpdateUI();
    }

    public override void UpdateUI()
    {
        m_throwItemSlot.UpdateUI();
        m_allItemBox.UpdateUI();
    }

    public void ShowItemInfoUI(SItem _item)
    {
        m_parent.ShowItemInfoUI(_item);
    }
    public void HideItemInfoUI()
    {
        m_parent.HideItemInfoUI();
    }

    public void SetItemInfoUIPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }


    public int CheckThrowItemPos(RectTransform _trans)
    {
        return m_throwItemSlot.CheckThrowItemPos(_trans);
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        return m_allItemBox.CheckAllItemPos(_trans);
    }



    public override void SetComps()
    {
        m_throwItemSlot = GetComponentInChildren<ThrowItemSlotScript>();
        m_throwItemSlot.SetParent(this);
        m_throwItemSlot.SetComps();
        m_allItemBox = GetComponentInChildren<AllItemBoxScript>();
        m_allItemBox.SetParent(this);
        m_allItemBox.SetComps();
    }
}
