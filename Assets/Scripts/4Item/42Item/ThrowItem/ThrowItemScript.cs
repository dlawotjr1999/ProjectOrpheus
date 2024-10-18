using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemScript : PooledItem
{
    [SerializeField]
    private ThrowItemScriptable m_scriptable;
    public bool IsScriptableSet { get { return m_scriptable != null; } }
    public void SetScriptable(ThrowItemScriptable _scriptable) { m_scriptable = _scriptable; }

    private Rigidbody m_rigid;

    public void SetFlying(Vector3 _force)
    {
        m_rigid.AddForce(_force);
    }


    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

}
