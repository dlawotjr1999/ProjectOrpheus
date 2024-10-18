using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectedNormalAttack : NormalAttackScript
{
    public override void AttackOn()
    {
        if (m_attackEffect == null) { return; }
        ((PowerEffect)m_attackEffect).EffectOn(transform);
    }
}
