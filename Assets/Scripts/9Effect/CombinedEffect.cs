using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CombinedEffect : MonoBehaviour
{
    [SerializeField]
    private bool m_hasVfx;
    [SerializeField]
    private bool m_hasParticle;
    [SerializeField]
    private bool m_hasTrail;

    private VisualEffect[] m_vfxs;
    private ParticleSystem[] m_particles;
    private TrailRenderer[] m_trails;


    public virtual void EffectOn()
    {
        if (m_hasVfx) { foreach(VisualEffect vfx in m_vfxs) { vfx.Play(); } }
        if (m_hasParticle) { foreach(ParticleSystem particle in m_particles) { particle.Play(); } }
        if (m_hasTrail) { foreach(TrailRenderer trail in m_trails) { trail.enabled = true; } }
    }

    public virtual void EffectOff()
    {
        if (m_hasVfx) { foreach (VisualEffect vfx in m_vfxs) { vfx.Stop(); } }
        if (m_hasParticle) { foreach (ParticleSystem particle in m_particles) { particle.Stop(); } }
        if (m_hasTrail) { foreach (TrailRenderer trail in m_trails) { trail.enabled = false; } }
    }



    private void SetComps()
    {
        if (m_hasVfx) { m_vfxs = GetComponentsInChildren<VisualEffect>(); }
        if (m_hasParticle) { m_particles = GetComponentsInChildren<ParticleSystem>(); }
        if (m_hasTrail) { m_trails = GetComponentsInChildren<TrailRenderer>(); }
    }
    private void Awake()
    {
        SetComps();
    }
}
