using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParabolaScript : MonsterProjectileScript
{
    [SerializeField]
    private float m_upperForce = 5;

    private float ForceMultiplier { get; set; } = 1;
    private float UpperForce { get { return m_upperForce * ForceMultiplier; } }

    public override void SetAttack(ObjectScript _attacker, Vector3 _dir, float _damage, float _dist)
    {
        base.SetAttack(_attacker, _dir, _damage, _dist);
        ForceMultiplier = Mathf.Pow(_dist / m_attackRange, 1.2f);
        m_rigid.velocity = Vector3.up * UpperForce;
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        m_rigid.AddForce(Vector3.down * m_rigid.mass * ValueDefine.PARABOLA_GRAVITY);
    }
}
