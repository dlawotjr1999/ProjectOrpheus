using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AllItemElmScript : MonoBehaviour
{
    private AllItemBoxScript m_parent;
    public AllItemBoxScript Box { get { return m_parent; } }
    public void SetParent(AllItemBoxScript _parent) { m_parent = _parent; }
    public void SetChild(AllItemImgScript _child) { ItemImg = _child; }

    public Transform MoveTrans { get { return m_parent.MoveTrans; } }

    public Transform ImgParent { get; private set; }

    public AllItemImgScript ItemImg { get; private set; }
    private TextMeshProUGUI m_itemNumTxt;

    public int CurIdx { get; private set; }
    public SItem CurItem { get; set; }
    public bool HasItem { get; private set; }

    public void SetItem(int _idx, InventoryElm _item)
    {
        CurIdx = _idx;

        if (!ItemImg.gameObject.activeSelf) { ItemImg.gameObject.SetActive(true); }

        CurItem = _item.Item;
        Sprite itemSprite = GameManager.GetItemSprite(CurItem);
        ItemImg.SetImg(itemSprite);

        m_itemNumTxt.text = _item.Num.ToString();
        HasItem = true;
    }

    public void HideItem()
    {
        HasItem = false;
        ItemImg.gameObject.SetActive(false);
        m_itemNumTxt.text = "";
    }


    public void ItemElmClick()
    {
        switch (CurItem.Type)
        {
            case EItemType.THROW:
                RegisterThrowItem();
                break;
        }
    }

    public void ItemUse()
    {
        // 아이템 사용 로직 필요 시 작성
        Debug.Log("아이템 사용");
    }

    public void RegisterThrowItem()
    {
        if(CurItem.Type != EItemType.THROW) { return; }
        PlayManager.AddThrowItem((EThrowItemName)CurItem.Idx);
        HideInfo();
    }

    public void SimulateChange(int _target, int _origin)
    {
        m_parent.SimulateChange(_target, _origin);
    }
    public void DeliverReset(int _idx)
    {
        m_parent.DeliverReset(_idx);
    }
    public void ResetImgPos()
    {
        ItemImg.ResetPos();
    }

    public int CheckThrowItemPos(RectTransform _trans)
    {
        return m_parent.CheckThrowItemPos(_trans);
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        return m_parent.CheckAllItemPos(_trans);
    }


    public void ResetItem()
    {
        m_parent.ResetChanges();
    }

    public void ChangeItem(int _target)
    {
        if (_target == -1 || CurIdx == _target) { ResetItem(); return; }
        PlayManager.SwapItemInven(CurIdx, _target);
        ItemImg.ResetPos();
    }


    public void ShowInfo()
    {
        if (CurItem.IsEmpty) { return; }
        m_parent.ShowInfo(CurItem);
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
        m_itemNumTxt = GetComponentInChildren<TextMeshProUGUI>();
        ItemImg = GetComponentInChildren<AllItemImgScript>();
        ItemImg.SetParent(this);
        ItemImg.SetComps();
        ImgParent = transform.GetChild(0);
    }
}
