using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum EOasisFunctionName
{
    REST,
    TRANSPORT,
    TRADE,
    RESET,
    LAST
}

public interface IOasisUI
{
    public void OpenUI(OasisUIScript _parent);
    public void CloseUI();
}

public class OasisUIScript : BaseUI
{
    private OasisNPC m_oasis;
    public void SetNPC(OasisNPC _oasis) { m_oasis = _oasis; }
    public OasisNPC Oasis { get { return m_oasis; } }

    [SerializeField]
    private Button m_closeBtn;
    [SerializeField]
    private Button[] m_functionBtns;
    [SerializeField]
    private GameObject[] m_uis = new GameObject[(int)EOasisFunctionName.LAST];


    private bool IsButtonClicked { get; set; }


    public void OpenUI(OasisNPC _npc)
    {
        base.OpenUI();
        IsButtonClicked = false;
        SetNPC(_npc);
    }

    public override void CloseUI()                       // ´Ý±â
    {
        m_oasis.StopInteract();
        base.CloseUI();
    } 


    private void ClickButton(EOasisFunctionName _function)
    {
        if(IsButtonClicked == true) { return; }

        GameObject ui = m_uis[(int)_function];
        ui.GetComponent<IOasisUI>().OpenUI(this);
        IsButtonClicked = true;
    }
    public void FunctionDone()
    {
        IsButtonClicked = false;
    }


    public override void SetComps()
    {
        base.SetComps();
        SetBtns();
    }
    private void SetBtns()
    {
        for (int i = 0; i<(int)EOasisFunctionName.LAST; i++)
        {
            EOasisFunctionName function = (EOasisFunctionName)i;
            m_functionBtns[i].onClick.AddListener(delegate { ClickButton(function); });
        }
        m_closeBtn.onClick.AddListener(CloseUI);
    }
}
