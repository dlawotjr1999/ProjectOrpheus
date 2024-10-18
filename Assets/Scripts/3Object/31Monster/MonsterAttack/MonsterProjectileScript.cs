using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class MonsterProjectileScript : ObjectAttackScript, IHittable, IPoolable
{
    protected Rigidbody m_rigid;

    [SerializeField]
    private float m_moveSpeed = 6;
    [SerializeField]
    protected float m_attackRange;
    [SerializeField]
    private float m_lastTime;

    private Vector3 MoveDir { get; set; }

    public bool IsPlayer => false;
    public bool IsMonster => false;

    public ObjectPool<GameObject> OriginalPool { get; private set; }

    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public void OnPoolGet() { }
    public void ReleaseToPool()
    {
        m_rigid.velocity = Vector3.zero;
        if(OriginalPool == null) { return; }
        OriginalPool.Release(gameObject);
    }


    public virtual void SetAttack(ObjectScript _attacker, Vector3 _dir, float _damage, float _dist)
    {
        SetAttack(_attacker, _damage);
        MoveDir = _dir;
    }



    public bool GetHit(HitData _hit)
    {
        Debug.Log("패링!");
        DestroyAttack();
        return true;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.GetComponentInParent<PlayerController>() != null)
        {
            DestroyAttack();
        }
        else if (_other.CompareTag(ValueDefine.TERRAIN_TAG))
        {
            DestroyAttack();
        }
    }

    public virtual void DestroyAttack()
    {
        ReleaseToPool();
    }



    private IEnumerator ReleaseDelay()
    {
        yield return new WaitForSeconds(m_lastTime);
        if (gameObject.activeSelf) { ReleaseToPool(); }
    }

    public virtual void OnEnable()
    {
        m_hitObjects.Clear();
        AttackOn();
        StartCoroutine(ReleaseDelay());
    }

    private void SetInfo()
    {

    }


    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }
    public override void Start() { }
    public virtual void FixedUpdate()
    {
        Vector3 dir = m_moveSpeed * MoveDir;
        m_rigid.velocity = dir;
    }
}
