using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class SkillVFXController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect m_visualEffect;

    [Range(0.1f, 10f)]
    private float m_size = 1f;

    private void OnValidate()
    {
        if(m_visualEffect == null)
        {
            m_visualEffect=GetComponent<VisualEffect>();
        }
        if(m_visualEffect != null)
        {
            m_visualEffect.SetFloat("size", m_size);
        }
    }
}
