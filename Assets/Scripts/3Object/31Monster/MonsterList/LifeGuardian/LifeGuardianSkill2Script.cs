using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGuardianSkill2Script : ObjectAttackScript
{
    private float SkillRadius { get { return ((LifeGuardianScript)m_attacker).DrainRadius; } }

    [SerializeField]
    private float m_damageGap = 1;
    [SerializeField]
    private AudioClip m_drainSound;
    private readonly float SoundGap = 1;

    private float GapCount { get; set; }
    private float SoundCount { get; set; }

    public override void AttackOn()
    {
        GapCount = m_damageGap;
        SoundCount = 0;
        m_attackEffect.EffectOn();
        base.AttackOn();
    }

    public override void AttackOff()
    {
        base.AttackOff();
        m_attackEffect.EffectOff();
    }

    private void CheckNAttackTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, SkillRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        if (SoundCount <= 0) { PlayDrainSound(); }
        foreach (Collider col in hits)
        {
            ObjectScript script = col.GetComponentInParent<ObjectScript>();
            if(script == null ||  script.IsDead || !script.IsPlayer) { continue; }
            GiveDamage(script, script.Position);
        }
    }
    private void PlayDrainSound()
    {
        GameManager.PlaySE(m_drainSound, transform.position);
        SoundCount += SoundGap;
    }

    public override void GiveDamage(IHittable _hittable, Vector3 _point)
    {
        if (CheckHit(_hittable)) { return; }
        HitData hit = new(Attacker, Damage, _point, m_impulseAmount, CCList);
        if (_hittable.GetHit(hit))
        {
            AddHitObject(_hittable);
        }
    }


    public override void Start() { }

    private void Update()
    {
        if (!IsAttacking) { return; }
        if (GapCount > 0)
        {
            GapCount -= Time.deltaTime;
            if (GapCount <= 0)
            {
                GapCount = m_damageGap;
                m_hitObjects.Clear();
            }
        }
        if(SoundCount > 0) { SoundCount -= Time.deltaTime; }
    }

    private void FixedUpdate()
    {
        if (!IsAttacking) { return; }
        CheckNAttackTarget();
    }
}
