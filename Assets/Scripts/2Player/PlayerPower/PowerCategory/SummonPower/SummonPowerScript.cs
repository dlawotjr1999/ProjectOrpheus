using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPowerScript : PlayerPowerScript
{
    [SerializeField]
    protected bool m_isInstantHit;
    [SerializeField]
    private EPowerSE m_summonSE;

    public override void PowerCreated()
    {
        AttackOff();
        if (m_isInstantHit)
        {
            PowerSummoned();
        }
    }
    public void SummonDoneTiming()
    {
        if (IsAttacking) { return; }
        PowerSummoned();
    }
    public virtual void PowerSummoned()
    {
        base.PowerCreated();
        if (PowerEffect != null) { PowerEffect.EffectOn(); }
        if(m_summonSE != EPowerSE.NONE) { GameManager.PlaySE(m_summonSE, transform.position); }
    }
    public override void ReleaseToPool()
    {
        if (PowerEffect != null) { PowerEffect.EffectOff(); }
        base.ReleaseToPool();
    }
}
