using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowItemElmScript : MonoBehaviour
{
    private ThrowItemSlotScript m_parent;
    public void SetParent(ThrowItemSlotScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.MoveTrans; } }

    public ThrowItemImgScript ItemImg { get; private set; }
    public void SetChild(ThrowItemImgScript _child) { ItemImg = _child; }


    public int CurIdx { get; private set; }
    private SItem CurItem { get; set; }
    public bool HasItem { get; private set; }

    public void SetItem(int _idx, EThrowItemName _item)
    {
        CurIdx = _idx;
        if (!ItemImg.gameObject.activeSelf) { ItemImg.gameObject.SetActive(true); }
        CurItem = new(EItemType.THROW, (int)_item);
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        ItemImg.SetImg(itemSprite);
        HasItem = true;
    }

    public void HideItem()
    {
        HasItem = false;
        ItemImg.gameObject.SetActive(false);
    }

    public void ShowInfo()
    {
        if (CurItem.IsEmpty) { return; }
        m_parent.ShowInfo(CurItem);
    }


    public void ItemElmClick()
    {
        PlayManager.RemoveThrowItem(CurIdx);
        HideInfo();
    }

    public void SimulateChange(int _target)
    {
        m_parent.SimulateChange(_target, CurIdx);
    }

    public void ResetItem()
    {
        m_parent.ResetChanges();
    }

    public void ChangeItem(int _target)
    {
        if (_target == -1 || CurIdx == _target) { ResetItem(); return; }

    }


    public int CheckThrowItemPos(RectTransform _trans)
    {
        return m_parent.CheckThrowItemPos(_trans);
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        return m_parent.CheckAllItemPos(_trans);
    }


    public void HideInfo()
    {
        m_parent.HideInfo();
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetInfoPos(_pos);
    }

    public void SetComps()
    {
        ItemImg = GetComponentInChildren<ThrowItemImgScript>();
        ItemImg.SetParent(this);
        ItemImg.SetComps();
    }
}
