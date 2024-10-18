using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHellcatScript : HellcatScript
{
    [SerializeField]
    private float m_healPercent = 0.1f;

    public override void GaveDamage(ObjectScript _target, float _damage)
    {
        HealObj(_damage * m_healPercent);
    }
}
