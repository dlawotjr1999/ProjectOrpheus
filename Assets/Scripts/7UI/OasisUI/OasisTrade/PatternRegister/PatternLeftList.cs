using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternLeftList : MonoBehaviour
{
    private PatternRegisterScript m_parent;
    public void SetParent(PatternRegisterScript _parent) { m_parent = _parent; }


    private PatternLeftElm[] m_elms;
    
    public void UpdateUI()
    {
        foreach (PatternLeftElm elm in m_elms)
        {
            elm.UpdateElm();
        }
    }

    public void RegisterPattern(EPatternName _pattern)
    {
        PlayManager.RegisterHealPattern(_pattern);

        m_parent.UpdateUI();
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<PatternLeftElm>();
        if(m_elms.Length != (int)EPatternName.LAST) { Debug.LogError("패턴 종류 틀림"); return; }
        for (int i = 0; i<(int)EPatternName.LAST; i++)
        {
            m_elms[i].SetParent(this);
            m_elms[i].SetComps(i);
        }
    }
}
