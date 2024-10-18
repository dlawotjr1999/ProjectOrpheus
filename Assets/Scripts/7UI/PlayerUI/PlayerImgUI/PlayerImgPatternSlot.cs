using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImgPatternSlot : MonoBehaviour
{
    private PlayerImgUIScript m_parent;
    public void SetParent(PlayerImgUIScript _parent) { m_parent = _parent; }

    private PlayerImgPatternElm[] m_elms;


    public void UpdateUI()
    {
        EPatternName[] healSlot = PlayManager.HealPatternList;
        for (int i = 0; i<ValueDefine.MAX_HEAL_ITEM; i++)
        {
            if(i >= healSlot.Length) { m_elms[i].SetPattern(EPatternName.LAST); continue; }
            m_elms[i].SetPattern(healSlot[i]);
        }
    }

    public void ShowInfo(EPatternName _pattern)
    {
        m_parent.ShowItemInfo(_pattern);
    }
    public void HideInfo()
    {
        m_parent.HideItemInfo();
    }
    public void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetItemInfoPos(_pos);
    }



    public void SetComps()
    {
        m_elms = GetComponentsInChildren<PlayerImgPatternElm>();
        if (m_elms.Length != ValueDefine.MAX_HEAL_ITEM) { Debug.LogError("회복 슬롯 개수 다름"); return; }
        foreach (PlayerImgPatternElm elm in m_elms)
        {
            elm.SetParent(this);
            elm.SetComps();
        }
    }
}
