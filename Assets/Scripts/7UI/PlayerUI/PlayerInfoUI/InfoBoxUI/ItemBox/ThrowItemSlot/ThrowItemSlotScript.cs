using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItemSlotScript : MonoBehaviour
{
    private ItemBoxUIScript m_parent;
    public void SetParent(ItemBoxUIScript _parent) { m_parent = _parent; }

    public Transform MoveTrans { get { return m_parent.transform; } }

    private ThrowItemElmScript[] m_elms;


    public void UpdateUI()
    {
        List<EThrowItemName> throwItemList = PlayManager.ThrowItemList;
        for (int i = 0; i<ValueDefine.MAX_THROW_ITEM; i++)
        {
            if (throwItemList.Count <= i) { m_elms[i].HideItem(); continue; }

            m_elms[i].SetItem(i, throwItemList[i]);
        }
    }

    public void SimulateChange(int _target, int _origin)
    {
        if (_target == -1 || _target == _origin) { ResetChanges(); return; }
        if (!m_elms[_target].HasItem) { return; }
        ThrowItemImgScript img1 = m_elms[_target].ItemImg, img2 = m_elms[_origin].ItemImg;
        m_elms[_origin].SetChild(img1); m_elms[_target].SetChild(img2);
        img1.SetParent(m_elms[_origin]); img2.SetParent(m_elms[_target]);
        img1.transform.SetParent(m_elms[_origin].transform);
        img1.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        img2.ChangeParentTrans(m_elms[_target].transform);
        PlayManager.SwapThrowItem(_target, _origin);
    }
    public void ResetChanges() { UpdateUI(); }


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

    public int CheckThrowItemPos(RectTransform _trans)
    {
        Vector2 compPos = _trans.anchoredPosition;
        for (int i = 0; i<m_elms.Length; i++)
        {
            ThrowItemElmScript elm = m_elms[i];
            Vector2 elmPos = MoveTrans.InverseTransformPoint(elm.transform.position);
            float dist = Vector2.Distance(elmPos, compPos);
            if(dist < ItemBoxUIScript.ElmCloseRange) { return i; }
        }
        return -1;
    }
    public int CheckAllItemPos(RectTransform _trans)
    {
        return m_parent.CheckAllItemPos(_trans);
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<ThrowItemElmScript>();
        if(m_elms.Length != ValueDefine.MAX_THROW_ITEM) { Debug.LogError("던지기 아이템 UI 개수 다름"); }
        for (int i=0;i<m_elms.Length;i++)
        {
            ThrowItemElmScript elm = m_elms[i];
            elm.SetParent(this); 
            elm.SetComps();
        }
    }
}
