using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSlotUIScript : MonoBehaviour
{
    private PowerSlotElmUIScript[] m_elms;


    public void UpdateUI()
    {
        EPowerName[] slot = PlayManager.PowerSlot;
        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            m_elms[i].SetPower(slot[i]);
        }
    }

    public void UsePower(int _idx, float _cooltime)
    {
        m_elms[_idx].UsePower(_cooltime);
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<PowerSlotElmUIScript>();
        foreach(PowerSlotElmUIScript elm in m_elms) { elm.SetComps(); }
    }
}
