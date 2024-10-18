using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttackScript : MonoBehaviour
{
    protected ObjectScript m_attacker;
    public ObjectScript Attacker { get { return m_attacker; } }

    public virtual bool IsAttacking { get; protected set; }     // 공격 중
    protected readonly List<IHittable> m_hitObjects = new();
    public bool CheckHit(IHittable _object) { return m_hitObjects.Contains(_object); }

    [SerializeField]
    protected ECCType[] m_ccList = new ECCType[0];
    [Range(0, 1)]
    [SerializeField]
    protected float m_impulseAmount = 0.1f;
    [SerializeField]
    protected CombinedEffect m_attackEffect;
    [SerializeField]
    protected AudioClip m_hitSound;


    public virtual float Damage { get; private set; } = 5;
    public float Impulse { get { return m_impulseAmount; } }
    public virtual ECCType[] CCList { get { return m_ccList; } }


    private void SetDamage(float _damage) { Damage = _damage * Attacker.DamageMultiplier * Attacker.AttackMultiplier; }
    public void SetAttack(ObjectScript _attacker, float _damage)
    {
        if (m_attacker == null) { m_attacker = _attacker; }
        SetDamage(_damage);
    }
    public void SetCCType(ECCType _cc) { SetCCType(new ECCType[1] { _cc }); }
    public void SetCCType(ECCType[] _ccs) { m_ccList = new ECCType[_ccs.Length]; for (int i = 0; i<_ccs.Length; i++) { m_ccList[i] = _ccs[i]; } }
    public void ResetCCType() { SetCCType(ECCType.NONE); }


    public virtual void AttackOn()                       // 공격 시작
    {
        IsAttacking = true;
    }
    public void AddHitObject(IHittable _object)         // 히트 판정 (중복 히트 방지)
    {
        m_hitObjects.Add(_object);
    }
    public virtual void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, Impulse, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
    }
    public virtual void AttackOff()                        // 공격 중단
    {
        IsAttacking = false;
        m_hitObjects.Clear();
    }


    public virtual void PlayEffect()
    {
        if (m_attackEffect == null) { return; }
        m_attackEffect.EffectOn();
    }
    public virtual void StopEffect()
    {
        if (m_attackEffect == null) { return; }
        m_attackEffect.EffectOff();
    }


    public virtual void Start()
    {
        m_attacker = GetComponentInParent<ObjectScript>();
        AttackOff();
    }
}
