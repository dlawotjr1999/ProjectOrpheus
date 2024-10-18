using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject powerObj;

    private CombinedEffect[] m_powerTrails;

    private int CurIdx { get; set; }

    public void PowerTrailOn(EPowerTrailType _type)
    {
        CurIdx = (int)_type - 1;
        m_powerTrails[CurIdx].EffectOn();
    }
    public void PowerTrailOff()
    {
        m_powerTrails[CurIdx].EffectOff();
    }


    public void SetComps()
    {
        m_powerTrails = powerObj.GetComponentsInChildren<CombinedEffect>();
    }
}
