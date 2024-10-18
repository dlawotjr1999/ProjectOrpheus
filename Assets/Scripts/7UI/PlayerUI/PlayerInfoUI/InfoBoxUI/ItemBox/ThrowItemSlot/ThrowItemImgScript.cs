using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowItemImgScript : DragMouseOverInfoUI
{
    private ThrowItemElmScript m_parent;
    public void SetParent(ThrowItemElmScript _parent) { m_parent = _parent; }

    private Image m_itemImg;

    public override Transform MoveTrans => m_parent.MoveTrans;


    public void SetImg(Sprite _img) { m_itemImg.sprite = _img; }

    public override void StartDrag(PointerEventData _data)
    {
        if (_data.button == PointerEventData.InputButton.Right) { m_parent.ItemElmClick(); return; }
        base.StartDrag(_data);
    }

    public override bool CheckPos()
    {
        int idx = GetIdx();
        return idx != -1;
    }

    private int GetIdx()
    {
        int idx;
        if (m_rect.anchoredPosition.y >= ItemBoxUIScript.ElmCritY)
        {
            idx = m_parent.CheckThrowItemPos(m_rect);
            if (idx == -1) { return idx; }
            m_parent.SimulateChange(idx);
            return idx;
        }
        else
        {
            return 0;
        }
    }

    public override void DropAction()
    {
        bool isThrow = m_rect.anchoredPosition.y >= ItemBoxUIScript.ElmCritY;
        int idx = GetIdx();
        if (isThrow) { m_parent.ChangeItem(idx); }
        else { m_parent.ItemElmClick(); }
    }


    public override void ShowInfo()
    {
        m_parent.ShowInfo();
    }
    public override void HideInfo()
    {
        m_parent.HideInfo();
    }
    public override void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_itemImg = GetComponent<Image>();
    }
}
