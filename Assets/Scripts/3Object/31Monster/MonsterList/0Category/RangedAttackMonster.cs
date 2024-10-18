using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Timeline;

public class RangedAttackMonster : MonsterScript
{
    protected ObjectPool<GameObject> m_attackPool;

    [SerializeField]
    private int m_attackMaxNum = 5;
    [SerializeField]
    private int m_throwAttackIdx = 0;           // 던지기 공격이 몇 번째 프리펍인지

    public virtual Vector3 AttackOffset { get { return Vector3.zero; } }


    private bool AttackCreated { get; set; }

    public override void LookTarget()
    {
        if(IsAttacking && AttackCreated) { return; }
        base.LookTarget();
    }

    public override void StartAttack()
    {
        base.StartAttack();
        AttackCreated = false;
    }
    public override void CreateAttack()
    {
        GameObject attack = m_attackPool.Get();
        attack.transform.localPosition = AttackOffset;
        attack.transform.parent = null;

        Vector3 dir = (CurTarget.Position - attack.transform.position).normalized;
        Vector3 flatDir = (new Vector3(dir.x, 0, dir.z)).normalized;

        MonsterProjectileScript script = attack.GetComponent<MonsterProjectileScript>();
        script.SetAttack(this, flatDir, Attack, TargetDistance);
        script.AttackOn();

        PlayAttackSound(0);

        AttackCreated = true;
    }

    private void InitPool()
    {
        m_attackPool = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, m_attackMaxNum, m_attackMaxNum);
        for (int i = 0; i<m_attackMaxNum; i++) { GameObject obj = OnPoolCreate(); obj.GetComponent<MonsterProjectileScript>().ReleaseToPool(); }
    }
    private GameObject OnPoolCreate()
    {
        GameObject obj = Instantiate(m_normalAttacks[m_throwAttackIdx], transform);
        obj.GetComponent<MonsterProjectileScript>().SetPool(m_attackPool);
        return obj;
    }
    private void OnPoolGet(GameObject _obj) { _obj.SetActive(true); }
    private void OnPoolRelease(GameObject _obj) { _obj.transform.SetParent(transform); _obj.SetActive(false); }
    private void OnPoolDestroy(GameObject _obj) { Destroy(_obj); }


    public override void Awake()
    {
        base.Awake();
        InitPool();
    }
}
