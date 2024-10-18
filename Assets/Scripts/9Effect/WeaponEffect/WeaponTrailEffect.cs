using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EPowerTrailType
{
    NONE,
    POWER1,
    POWER2,
    POWER3,

    HAND1,
    HAND2,

    LAST
}



public class WeaponTrailEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject normalObj;
    [SerializeField]
    private GameObject powerObj;

    private TrailRenderer[] m_normalTrails;
    private CombinedEffect[] m_powerTrails;

    private int CurIdx { get; set; }

    public void SetNormalTrail(bool _on)
    {
        foreach (TrailRenderer trail in m_normalTrails) { trail.enabled = _on; }
    }

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
        m_normalTrails = normalObj.GetComponentsInChildren<TrailRenderer>();
        SetNormalTrail(false);
        m_powerTrails = powerObj.GetComponentsInChildren<CombinedEffect>();
    }
}
