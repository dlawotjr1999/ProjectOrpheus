using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisTradeUIScript : BaseUI, IOasisUI
{
    private OasisUIScript m_parent;

    private ProductListScript m_productList;
    private PatternRegisterScript m_patternRegister;
    private OasisSoulInfoScript m_soulInfo;


    private OasisNPC Oasis { get { return m_parent.Oasis; } }

    [SerializeField]
    private Button m_closeBtn;

    public void OpenUI(OasisUIScript _parent)
    {
        if (m_parent == null) { m_parent = _parent; }
        base.OpenUI();
    }

    public override void UpdateUI()
    {
        m_productList.UpdateUI(Oasis);
        m_patternRegister.UpdateUI();
        m_soulInfo.UpdateUI();
    }


    public override void CloseUI()
    {
        m_parent.FunctionDone();
        base.CloseUI();
    }


    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_productList = GetComponentInChildren<ProductListScript>();
        m_productList.SetParent(this); m_productList.SetComps();
        m_patternRegister = GetComponentInChildren<PatternRegisterScript>();
        m_patternRegister.SetComps();
        m_soulInfo = GetComponentInChildren<OasisSoulInfoScript>();

        SetBtns();
    }
}
