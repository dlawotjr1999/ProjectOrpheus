using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemBoxScript : MonoBehaviour
{
    private ItemBoxUIScript m_parent;
    public void SetParent(ItemBoxUIScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.transform; } }


    private const int ElmCount = 24;
    public int ElmNum { get { return ElmCount; } }

    private AllItemElmScript[] m_elms;


    public void UpdateUI()
    {
        InventoryElm[] inven = PlayManager.PlayerInventory;
        for (int i = 0; i<ElmCount; i++)
        {
            if (inven[i].IsEmpty) { m_elms[i].HideItem(); continue; }
            m_elms[i].SetItem(i, inven[i]);
        }
    }

    public void SimulateChange(int _target, int _origin)
    {
        if (_target == -1 || _target == _origin) { ResetChanges(); return; }
        if (!m_elms[_target].HasItem) { return; }

        AllItemElmScript elm1 = m_elms[_target], elm2 = m_elms[_origin];
        AllItemImgScript img1 = elm1.ItemImg, img2 = elm2.ItemImg;

        Vector2 offset = elm1.ImgParent.InverseTransformPoint(elm2.transform.position);
        img1.SetOffset(offset);
        img1.GetComponent<RectTransform>().anchoredPosition = offset;
    }
    public void DeliverReset(int _idx)
    {
        ReturnElm(_idx);
    }
    public void ReturnElm(int _idx)
    {
        m_elms[_idx].ResetImgPos();
    }

    public void ResetChanges() { UpdateUI(); }


    public int CheckThrowItemPos(RectTransform _trans)
    {
        return m_parent.CheckThrowItemPos(_trans);
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        Vector2 compPos = _trans.anchoredPosition;
        for (int i = 0; i<m_elms.Length; i++)
        {
            AllItemElmScript elm = m_elms[i];
            Vector2 elmPos = MoveTrans.InverseTransformPoint(elm.transform.position);
            float dist = Vector2.Distance(elmPos, compPos);
            if (dist < ItemBoxUIScript.ElmCloseRange) { return i; }
        }
        return -1;
    }


    public void ShowInfo(SItem _item)
    {
        m_parent.ShowItemInfoUI(_item);
    }
    public void HideInfo()
    {
        m_parent.HideItemInfoUI();
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<AllItemElmScript>();
        if (m_elms.Length != ElmCount) { Debug.LogError("아이템 UI 개수 다름"); return; }
        for (int i = 0; i < ElmCount; i++)
        {
            AllItemElmScript elm = m_elms[i];
            elm.SetComps();
            elm.SetParent(this);
        }
    }
}
