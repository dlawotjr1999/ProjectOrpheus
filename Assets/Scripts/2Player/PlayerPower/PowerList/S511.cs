using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class S511 : AroundPowerScript
{
    [SerializeField]
    VisualEffect m_visualEffect;
    [SerializeField]
    private float m_repeatDuration = 1f;
    [SerializeField]
    private float m_repeatInterval = 5f;

    public override void Start()
    {
        m_visualEffect=GetComponentInChildren<VisualEffect>();
        InvokeRepeating("StartVfx",0f,m_repeatInterval);
    }

    private void StartVfx()
    {
        if (m_visualEffect == null) { return; }
        m_visualEffect.Play();
        Invoke("StopVfx", m_repeatDuration);
    }

    private void StopVfx()
    {
        if(m_visualEffect == null) { return;}
        m_visualEffect.Stop();
    }
}
