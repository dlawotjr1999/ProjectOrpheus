using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public enum EEffectName
{
    MONSTER_DISSOLVE,
    HEAL,
    HIT_SLASH,
    HIT_BLOW,
    HIT_POWER,
    HIT_FATIGUE,
    HIT_MELANCHOLY,
    
    LAST
}

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_effects = new GameObject[(int)EEffectName.LAST];
    public GameObject[] EffectArray { get { return m_effects; } }


    public GameObject GetEffectObj(EEffectName _effect) 
    {
        return PoolManager.GetObject(m_effects[(int)_effect]);
    }




    public void SetManager()
    {

    }
}
