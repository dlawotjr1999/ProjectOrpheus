using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIScript : BaseUI
{
    private QuestUIElmScript[] m_elms;
    private QuestUIDescScript m_desc;

    public override void OpenUI()
    {
        base.OpenUI();
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        GameManager.PlaySE(ESystemSE.OPEN_UI);
    }

    public override void UpdateUI()
    {

    }

    public override void CloseUI()
    {
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        base.CloseUI();
    }

    public override void SetComps()
    {
        base.SetComps();
        //m_elms = GetComponentsInChildren<QuestUIElmScript>();
        //foreach(QuestUIElmScript elm in m_elms) elm.SetComps();
        //m_desc = GetComponentInChildren<QuestUIDescScript>();
        //m_desc.SetComps();
    }
}
