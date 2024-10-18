using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllItemImgScript : DragMouseOverInfoUI
{
    private AllItemElmScript m_parent;
    public void SetParent(AllItemElmScript _parent) { m_parent = _parent; }

    private Image m_itemImg;
    public override Transform MoveTrans => m_parent.MoveTrans;


    public int OriginalIdx { get; private set; }
    public int TargetIdx { get; private set; }

    public void SetImg(Sprite _img) { m_itemImg.sprite = _img; }

    public override void StartDrag(PointerEventData _data)
    {
        if (_data.button == PointerEventData.InputButton.Right) { m_parent.ItemElmClick(); return; }
        OriginalIdx = m_parent.CurIdx;
        TargetIdx = -1;
        base.StartDrag(_data);
    }

    public override void OnDoubleClick(PointerEventData _data)
    {
        m_parent.ItemUse();
        return;
    }

    public override bool CheckPos()
    {
        int idx = GetIdx();
        return idx != -1;
    }

    private int GetIdx()
    {
        if (m_rect.anchoredPosition.y >= ItemBoxUIScript.ElmCritY)
        {
            return 0;
        }
        else
        {
            int curTarget = m_parent.CheckAllItemPos(m_rect);
            if (TargetIdx != -1 && curTarget != TargetIdx && TargetIdx != OriginalIdx) { m_parent.DeliverReset(TargetIdx); TargetIdx = curTarget; return -1; }
            TargetIdx = curTarget;
            if (TargetIdx == -1) { return TargetIdx; }
            m_parent.SimulateChange(TargetIdx, OriginalIdx);
            return TargetIdx;
        }
    }

    public void SetOffset(Vector2 _offset)
    {
        m_rect.anchoredPosition = _offset;
    }
    public void ResetPos()
    {
        m_rect.anchoredPosition = Vector2.zero;
    }

    public override void DropAction()
    {
        bool isThrow = m_rect.anchoredPosition.y >= ItemBoxUIScript.ElmCritY;
        int idx = m_parent.CheckAllItemPos(m_rect);
        if (isThrow) { m_parent.RegisterThrowItem(); }
        else if(idx != -1 && idx != OriginalIdx) { m_parent.ChangeItem(idx); m_parent.DeliverReset(idx); }
        transform.SetSiblingIndex(0);
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
