using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPoolEffectScript : EffectScript
{
    [SerializeField]
    protected float m_destroyTime = 1;

    public virtual void OnEnable()
    {
        SetDestroyTime(m_destroyTime);
    }
}
