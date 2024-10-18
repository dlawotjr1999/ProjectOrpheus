using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWaterGuardianAttack
{
    RIGHT_DOWN_SLASH,
    RIGHT_SIDE_SLASH,
    POINTED,
    WIPE_UP,

    LAST
}

public enum EWaterGuardianSkill
{
    SLASH,
    DASH,
    ICE,

    LAST
}

public class WaterGuardianScript : BossMonster
{
    public override void SetDestination(Vector3 _destination)
    {
        base.SetDestination(_destination);
        m_anim.SetBool("IS_MOVING", true);
    }
    public override void StopMove()
    {
        base.StopMove();
        m_anim.SetBool("IS_MOVING", false);
    }

    private bool AttackProceed { get; set; }
    private readonly float[] AttackAngle = new float[4] { 15, -15, 0, 75 };
    private readonly float SkillAngle = 25;


    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            if (IsSkilling) { rot += SkillAngle; }
            else { rot += AttackAngle[AttackIdx]; }
            if (rot < 0) { rot += 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        StopMove();
        AttackAnimation();
        AttackIdx = Random.Range(0, (int)EWaterGuardianAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
    }
    public override void AttackTriggerOn()
    {
        foreach (GameObject attack in m_normalAttacks)
        {
            AnimateAttackScript script = attack.GetComponent<AnimateAttackScript>();
            script.AttackOn();

            script.SetAttack(this, NormalDamage(AttackIdx));
        }

        AttackProceed = AttackIdx < (int)EWaterGuardianAttack.POINTED && Random.Range(0, 2) == 0;
        m_anim.SetBool("PROCEED_ATTACK", AttackProceed);
        if (AttackIdx == (int)EWaterGuardianAttack.POINTED)
        {
            Vector3 dir = (CurTarget.Position-Position).normalized;
            m_rigid.AddForce(m_dashPower * dir);
        }
        PlayAttackSound(AttackIdx);
/*        else if (AttackIdx != (int)EWaterGuardianAttack.POINTED))
        {
            AttackObject.PlayEffect();
        }*/
    }
    public override void AttackTriggerOff()
    {
        foreach (GameObject attack in m_normalAttacks)
        {
            AnimateAttackScript script = attack.GetComponent<AnimateAttackScript>();
            script.AttackOff();
        }

/*        if (AttackIdx != (int)EWaterGuardianAttack.POINTED))
        {
            AttackObject.StopEffect();
        }*/
    }
    public override void AttackDone()
    {
        IsTracing = false;
        if (!AttackProceed)
        {
            base.AttackDone();
        }
        else { AttackIdx++; }
    }



    // 스킬
    public override float AttackRange => (NextSkillIdx == IceIdx && CanSkill) ? base.AttackRange * 4 : base.AttackRange;

    private readonly int SlashIdx = (int)EWaterGuardianSkill.SLASH;
    private readonly int RushIdx = (int)EWaterGuardianSkill.DASH;
    private readonly int IceIdx = (int)EWaterGuardianSkill.ICE;

    private Vector3 Skill3Offset { get { return Vector3.up * m_iceHeight; } }

    [Tooltip("돌진 공격 힘")]
    [SerializeField]
    private float m_dashPower = 15;
    [Tooltip("돌진 공격 시 상승")]
    [SerializeField]
    private float m_dashUp = 3;

    [SerializeField]
    private CombinedEffect m_iceCastEffect;
    [Tooltip("얼음 스킬 생성 위치")]
    [SerializeField]
    private float m_iceHeight = 12;
    [SerializeField]
    private AudioClip[] m_skillSounds;
    [SerializeField]
    private AudioClip m_iceReadySound;

    public override void SkillOn()
    {
        base.SkillOn();
        if (CurSkillIdx == SlashIdx)
        {
            CurSkill = SkillList[SlashIdx];
            CurSkill.SetAttack(this, SkillDamages[SlashIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            GameManager.PlaySE(m_skillSounds[SlashIdx], transform.position);
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill = SkillList[RushIdx];
            CurSkill.SetAttack(this, SkillDamages[RushIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            m_rigid.velocity = m_dashPower * transform.forward + Vector3.up * m_dashUp;
            GameManager.PlaySE(m_skillSounds[RushIdx], transform.position);
        }
        else if (CurSkillIdx == IceIdx)
        {
            m_iceCastEffect.EffectOn();
            GameManager.PlaySE(m_iceReadySound, transform.position);
        }
    }
    public override void CreateSkill()
    {
        base.CreateSkill();
        if (CurSkillIdx == IceIdx)
        {
            CurSkill = SkillList[IceIdx];
            CurSkill.AttackOn();
            CurSkill.SetAttack(this, SkillDamages[IceIdx]);

            Vector3 pos = CurTarget.Position + new Vector3(CurTarget.Velocity2.x,0,CurTarget.Velocity2.y);

            CurSkill.gameObject.transform.position = pos + Skill3Offset;
            CurSkill.gameObject.transform.SetParent(null);

            m_iceCastEffect.EffectOff();
            GameManager.PlaySE(m_skillSounds[IceIdx], transform.position);
        }
    }
    public override void SkillOff()
    {
        base.SkillOff();
        if (CurSkillIdx == SlashIdx)
        {
            CurSkill.AttackOff();
            CreatedSkill = true;
            IsTracing = false;
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill.AttackOff();
            m_rigid.velocity = Vector3.zero;
        }
    }

    public override void SetStates()
    {
        base.SetStates();
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<WaterGuardianSkillState>();
    }
}