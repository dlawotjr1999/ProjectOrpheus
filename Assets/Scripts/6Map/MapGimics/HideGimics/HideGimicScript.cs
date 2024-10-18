using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGimicScript : MonoBehaviour, IHidable
{
    [SerializeField]
    protected Collider[] m_colliders;

    protected float PlayerDistance { get { return Vector3.Distance(transform.position, PlayManager.PlayerPos2); } }

    public virtual void GetLight()
    {
        SetObjectHide(false);
    }

    public virtual void LoseLight()
    {
        SetObjectHide(true);
    }

    private void SetObjectHide(bool _hide)
    {
        foreach (Collider col in m_colliders)
        {
            col.enabled = !_hide;
        }
    }


    public virtual void SetComps()
    {
        m_colliders = GetComponentsInChildren<Collider>();
    }

    private void Awake()
    {
        SetComps();
    }
}
