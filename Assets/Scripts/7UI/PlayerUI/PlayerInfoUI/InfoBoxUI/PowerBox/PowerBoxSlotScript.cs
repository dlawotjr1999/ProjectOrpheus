using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoxSlotScript : MonoBehaviour
{
    private PowerBoxSlotElmScript[] m_slots;
    public RectTransform[] ElmTrans { get; set; } = new RectTransform[ValueDefine.MAX_POWER_SLOT];

    public void UpdateSlot(EPowerName[] _powerSlot)
    {
        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            m_slots[i].SetPower(_powerSlot[i]);
        }
    }


    public void SetComps()
    {
        m_slots = GetComponentsInChildren<PowerBoxSlotElmScript>();
        for(int i=0;i<ValueDefine.MAX_POWER_SLOT;i++)
        {
            ElmTrans[i] = m_slots[i].GetComponent<RectTransform>();
        }
    }
}
