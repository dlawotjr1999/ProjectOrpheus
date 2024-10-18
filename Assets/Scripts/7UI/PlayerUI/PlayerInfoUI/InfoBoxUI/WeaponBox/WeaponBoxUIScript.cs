using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBoxUIScript : PlayerInfoBoxScript
{
    public Transform ParentTrans { get { return m_parent.transform; } }

    private WeaponBoxElmScript[] m_elms;

    [SerializeField]
    private WeaponInfoUIScript m_infoUI;
    [SerializeField]
    private Button[] m_pageBtns = new Button[2];

    private static int CurPage { get; set; } = 0;
    private int MaxPage { get { return ((int)EWeaponName.LAST - 1) / 8; } }
    private const int ElmPerPage = 8;

    public override void InitUI()
    {
        UpdateUI();
    }

    private void SetPageBtn()
    {
        m_pageBtns[0].interactable = (CurPage > 0);
        m_pageBtns[1].interactable = (CurPage < MaxPage);
    }

    private void Changepage(bool _toNext)
    {
        if (_toNext) { CurPage++; }
        else { CurPage--; }
        UpdateUI();
    }


    public void EquipWeapon(EWeaponName _weapon)
    {
        if(_weapon == PlayManager.CurWeapon) { return; }

        PlayManager.EquipWeapon(_weapon);
        UpdateUI();
        m_parent.UpdatePlayerModelWeapon();
    }


    public override void UpdateUI()
    {
        int start = CurPage * ElmPerPage;
        for (int i = 0; i<ElmPerPage; i++)
        {
            int idx = start + i;
            if (idx >= (int)EWeaponName.LAST) { m_elms[i].HideElm(); continue; }
            if (!m_elms[i].gameObject.activeSelf) { m_elms[i].gameObject.SetActive(true); }
            m_elms[i].SetWeaponInfo(idx);
        }
        SetPageBtn();
    }

    public void ShowInfoUI(EWeaponName _weapon)
    {
        SItem weapon = new(EItemType.WEAPON, (int)_weapon);
        m_parent.ShowItemInfoUI(weapon);
    }
    public void SetInfoUIPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }
    public void HideInfoUI()
    {
        m_parent.HideItemInfoUI();
    }


    private void SetBtns()
    {
        m_pageBtns[0].onClick.AddListener(delegate { Changepage(false); });
        m_pageBtns[1].onClick.AddListener(delegate { Changepage(true); });
    }

    public override void SetComps()
    {
        m_elms = GetComponentsInChildren<WeaponBoxElmScript>();
        if (m_elms.Length != ElmPerPage) { Debug.LogError("UI 개수 틀림"); }
        foreach (WeaponBoxElmScript elm in m_elms) { elm.SetParent(this); }
        SetBtns();

    }
}
