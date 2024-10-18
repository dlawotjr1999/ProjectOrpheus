using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBuffType
{
    MAX_HP,
    STATUS,

    LAST
}

public class BuffEffectList : MonoBehaviour
{
    private BuffEffectScript[] m_effects;



    public void SetEffect(EBuffType _buff, bool _on) 
    {
        if (_on) { m_effects[(int)_buff].EffectOn(); }
        else { m_effects[(int)_buff].EffectOff(); }
    }



    public void SetComps()
    {
        m_effects = GetComponentsInChildren<BuffEffectScript>();
        foreach(BuffEffectScript effect in m_effects) { effect.SetComps(); }
    }
}
