using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EPlayerInfoType
{
    STAT,
    TRAIT,
    SKILL,
    WEAPON,
    ITEM,
    MONSTER,

    LAST
}


public class PlayerInfoUIScript : MonoBehaviour
{
    private PlayerUIScript m_parent;
    public void SetParent(PlayerUIScript _parent) { m_parent = _parent; }

    [SerializeField]
    private GameObject[] m_infoBoxObjs = new GameObject[(int)EPlayerInfoType.LAST];

    private readonly PlayerInfoBoxScript[] m_infoUIs = new PlayerInfoBoxScript[(int)EPlayerInfoType.LAST];
    private Button[] m_infoSelectBtns;

    private static EPlayerInfoType m_typeSetting = EPlayerInfoType.LAST;                    // 기존 설정된 스탯
    public EPlayerInfoType CurType { get; private set; } = EPlayerInfoType.LAST;            // 보여주고 있는 거

    public void OpenUI()                                    // 열기
    {
        if (m_typeSetting == EPlayerInfoType.LAST) { SetInfoBox(EPlayerInfoType.STAT); }
        else { CurType = EPlayerInfoType.LAST; SetInfoBox(m_typeSetting); }
    }

    public void SetInfoBox(EPlayerInfoType _type)           // 항목 설정
    {
        if (CurType == _type) { return; }
        if (CurType != EPlayerInfoType.LAST) { m_infoUIs[(int)CurType].CloseUI(); }
        CurType = _type;
        m_typeSetting = _type;
        m_infoUIs[(int)CurType].OpenUI();
        m_parent.InfoBoxSet(_type);
    }

    public void UpdatePlayerModelWeapon()
    {
        m_parent.UpdatePlayerModelWeapon();
    }

    public void UpdateUI()
    {
        m_infoUIs[(int)CurType].UpdateUI();
    }

    public void ShowItemInfoUI(SItem _item)
    {
        m_parent.ShowItemInfoUI(_item);
    }
    public void SetItemInfoUIPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }
    public void HideItemInfoUI()
    {
        m_parent.HideItemInfoUI();
    }

    private void SetBtns()
    {
        m_infoSelectBtns[(int)EPlayerInfoType.STAT].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.STAT); });
        m_infoSelectBtns[(int)EPlayerInfoType.TRAIT].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.TRAIT); });
        m_infoSelectBtns[(int)EPlayerInfoType.SKILL].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.SKILL); });
        m_infoSelectBtns[(int)EPlayerInfoType.WEAPON].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.WEAPON); });
        m_infoSelectBtns[(int)EPlayerInfoType.ITEM].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.ITEM); });
        m_infoSelectBtns[(int)EPlayerInfoType.MONSTER].onClick.AddListener(delegate { SetInfoBox(EPlayerInfoType.MONSTER); });
    }
    public void SetComps()
    {
        m_infoSelectBtns = GetComponentsInChildren<Button>();
        SetBtns();
        for (int i = 0; i<(int)EPlayerInfoType.LAST; i++)
        {
            m_infoUIs[i] = m_infoBoxObjs[i].GetComponent<PlayerInfoBoxScript>();
            m_infoUIs[i].SetParent(this);
            m_infoUIs[i].SetComps();
        }
    }
}
