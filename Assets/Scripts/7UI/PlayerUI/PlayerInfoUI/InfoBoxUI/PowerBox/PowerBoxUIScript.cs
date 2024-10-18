using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoxUIScript : PlayerInfoBoxScript
{
    private PowerBoxSlotScript m_slot;
    private PowerBoxElmScript[] m_elms;

    [SerializeField]
    private Button[] m_pageBtns = new Button[2];

    public RectTransform[] SlotTrans { get { return m_slot.ElmTrans; } }

    private static int CurPage { get; set; } = 0;
    private readonly int MaxPage = ((int)EPowerName.LAST - 1) / ElmPerPage;
    private const int ElmPerPage = 12;

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


    public override void UpdateUI()
    {
        EPowerName[] slot = PlayManager.PowerSlot;
        m_slot.UpdateSlot(slot);

        int start = CurPage * ElmPerPage;
        for (int i = 0; i<ElmPerPage; i++)
        {
            int idx = start + i;
            EPowerName skill = (EPowerName)idx;
            if(skill >= EPowerName.LAST) { m_elms[i].HideElm(); continue; }
            m_elms[i].SetSkillInfo(skill, slot.Contains(skill));
        }

        SetPageBtn();
    }



    private void SetBtns()
    {
        m_pageBtns[0].onClick.AddListener(delegate { Changepage(false); });
        m_pageBtns[1].onClick.AddListener(delegate { Changepage(true); });
    }

    public override void SetComps()
    {
        m_slot = GetComponentInChildren<PowerBoxSlotScript>();
        m_slot.SetComps();
        m_elms = GetComponentsInChildren<PowerBoxElmScript>();
        foreach (PowerBoxElmScript elm in m_elms) { elm.SetParent(this); }
        SetBtns();
    }
}
