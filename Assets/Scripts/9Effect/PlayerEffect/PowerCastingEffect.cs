using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PowerCastingEffect : MonoBehaviour
{
    private VisualEffect m_effect;
    private TrailRenderer m_trail;

    public void ShowEffect()
    {
        if (!m_effect.enabled) { m_effect.enabled = true; }
        m_effect.Play();
        m_trail.enabled = true;
    }

    public void HideEffect()
    {
        m_effect.Stop();
        m_trail.enabled = false;
    }



    public void SetComps()
    {
        m_effect = GetComponentInChildren<VisualEffect>();
        m_trail = GetComponentInChildren<TrailRenderer>();
    }
}
