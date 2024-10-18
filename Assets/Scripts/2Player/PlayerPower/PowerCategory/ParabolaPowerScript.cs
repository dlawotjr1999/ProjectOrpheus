using System.Collections;
using System.Data;
using UnityEngine;

public class ParabolaPowerScript : ProjectilePowerScript
{
    [SerializeField]
    private float m_upperForce = 1;

    public override float DistanceMultiplier => 2;

    [SerializeField]
    private float ForceMultiplier { get; set; } = 1;
    private float UpperForce { get { return m_upperForce * ForceMultiplier; } }

    public override void PowerCreated()
    {
        base.PowerCreated();
        ForceMultiplier = m_scriptable.MoveSpeed ;
        m_rigid.velocity = Vector3.up * UpperForce / 2;
    }

    public virtual void CollideGround()
    {
        CollideTarget();
    }

    public override void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag(ValueDefine.TERRAIN_TAG)) { CollideGround(); return; }
        base.OnTriggerStay(_other);
    }


    public override void FixedUpdate()
    {
        m_rigid.AddForce(Vector3.down * m_rigid.mass * ValueDefine.PARABOLA_GRAVITY); 
        base.FixedUpdate();
    }
}
