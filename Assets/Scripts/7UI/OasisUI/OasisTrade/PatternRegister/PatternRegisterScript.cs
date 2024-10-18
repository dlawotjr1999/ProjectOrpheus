using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternRegisterScript : MonoBehaviour
{
    private PatternRegisteredList m_registered;
    private PatternLeftList m_left;


    public void UpdateUI()
    {
        m_registered.UpdateUI();
        m_left.UpdateUI();
    }


    public void SetComps()
    {
        m_registered = GetComponentInChildren<PatternRegisteredList>();
        m_registered.SetComps();
        m_left = GetComponentInChildren<PatternLeftList>();
        m_left.SetParent(this); m_left.SetComps();
    }
}
