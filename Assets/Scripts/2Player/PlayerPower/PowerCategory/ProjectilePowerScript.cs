using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectilePowerScript : PlayerPowerScript
{
    protected Rigidbody m_rigid;
    public float HitRadius { get { return m_scriptable.HitRadius; } }
    public float MoveSpeed { get { return m_scriptable.MoveSpeed; } }
    public Vector3 MoveDir;
    private Vector3 StartPosition;

    [SerializeField]
    protected ExplodeScript[] m_explodeAttacks;

    public virtual float DistanceMultiplier { get { return 1; } }


    public override void ReleaseToPool()
    {
        m_rigid.velocity = Vector3.zero;
        base.ReleaseToPool();
    }

    public virtual bool DistanceCheck(float _distanceMoved)
    {
        return _distanceMoved > m_scriptable.CastingRange * DistanceMultiplier;
    }

    public virtual void SetPower(PlayerController _player, float _attack, float _magic, Vector2 _dir)
    {
        base.SetPower(_player, _attack, _magic);
        MoveDir = new(_dir.x,0,_dir.y);
        transform.localEulerAngles = new(0, FunctionDefine.VecToDeg(_dir), 0);
    }

    public override void CollideTarget()
    {
        if (PowerEffect != null) { PowerEffect.EffectOn(transform, 1); }
        if(m_explodeAttacks.Length > 0) { ActiveExplodes(); }
        ReleaseToPool();
    }

    public virtual void ActiveExplodes()
    {
        foreach (ExplodeScript explode in m_explodeAttacks)
        {
            explode.gameObject.SetActive(true);
            explode.SetAttack(m_attacker, 10, 1);
            explode.SetReturnTransform(transform);
        }
    }



    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }
    public override void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider!= null)
        {
            sphereCollider.radius = HitRadius;
        }
        StartPosition = transform.position;
    }
    public virtual void FixedUpdate()
    {
        Vector3 vel = m_rigid.velocity;
        Vector3 dir = MoveSpeed * MoveDir;
        vel.x = dir.x; vel.z = dir.z;
        m_rigid.velocity = vel;
        float distanceMoved = Vector3.Distance(StartPosition, transform.position);
        if (DistanceCheck(distanceMoved))
        {
            CollideTarget();
        }
    }
}

