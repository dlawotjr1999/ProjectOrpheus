using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CCEffects : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] m_effects;

    private int CC2Idx(ECCType _type)
    {
        return _type switch 
        {
            ECCType.FATIGUE => 0,
            ECCType.MELANCHOLY => 1,
            ECCType.EXTORTION => 2,
            ECCType.WEAKNESS => 3,
            ECCType.BIND => 4,
            ECCType.VOID => 5,

            _ => -1
        };
    }
    public void SetCCEffect(ECCType _cc, bool _on)
    {
        int idx = CC2Idx(_cc);
        if (idx == -1) { return; }
        
        VisualEffect effect = m_effects[idx];

        if (_on) { effect.Play(); }
        else { effect.Stop(); }
    }
    public void AllOff()
    {
        foreach(VisualEffect effect in m_effects) { effect.Stop(); }
    }
}
