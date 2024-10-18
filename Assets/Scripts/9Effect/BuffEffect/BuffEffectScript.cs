using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BuffEffectScript : MonoBehaviour
{
    private VisualEffect m_effect;
    private Light m_light;


    public void EffectOn() 
    {
        m_effect.Play();
        m_light.enabled = true;
    }
    public void EffectOff() 
    {
        m_effect.Stop();
        m_light.enabled = false;
    }


    public void SetComps()
    {
        m_effect = GetComponentInChildren<VisualEffect>();
        m_light = GetComponentInChildren<Light>();
    }
}
