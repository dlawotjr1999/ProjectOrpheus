using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EArrogantAttack
{
    RIGHT_SWING,
    LEFT_SWING,

    LAST
}

public enum EArrogantSkill
{
    SMASH,

    LAST
}

public class ArrogantScript : HmmScript
{
    public override bool CanPurify => IsFatigure;


    [Tooltip("내려찍기 반경")]
    [SerializeField]
    public float m_smashRadius = 4;

    public override float AttackRange => SkillTimeCheck ? m_smashRadius-2 : base.AttackRange;

    public override void StartAttack()
    {
        SetAttackIdx(Random.Range(0, (int)EArrogantAttack.LAST));
        base.StartAttack();
    }

    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn(AttackIdx);
        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
        AttackObject.AttackOn();
    }
    public override void AttackTriggerOff()
    {
        base.AttackTriggerOff();
        AttackObject.AttackOff();
    }

    public override void StartSkill()
    {
        base.StartSkill();
        m_smashList.Clear();
        PlayAttackSound(0);
    }
    public override void CreateSkill()
    {
        CheckNSmash();
        m_smash.Play();
        GameManager.PlaySE(m_smashSound, m_smash.transform.position);
        CreateSmashImpulse();
        base.CreateSkill();
    }
    private void CreateSmashImpulse()
    {
        float distance = Vector2.Distance(PlayManager.PlayerPos2, Position2);
        float impulse = Mathf.Sqrt(1-distance / MaxImpulseDistance) * 0.9f + 0.1f;
        PlayManager.CreateImpulse(impulse);
    }

    [SerializeField]
    private VisualEffect m_smash;
    [SerializeField]
    private AudioClip m_smashSound;
    private readonly float MaxImpulseDistance = 10;

    private readonly List<ObjectScript> m_smashList = new();
    public void CheckNSmash()
    {
        Collider[] targets = Physics.OverlapSphere(m_smash.transform.position, m_smashRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            ObjectScript obj = targets[i].GetComponentInParent<ObjectScript>();
            if (obj == null || obj == this || m_smashList.Contains(obj)) { continue; }
            HitData air = new(this, Attack, Position, ECCType.AIRBORNE);
            if (obj.GetHit(air))
            {
                m_smashList.Add(obj);
            }
        }
    }
}
