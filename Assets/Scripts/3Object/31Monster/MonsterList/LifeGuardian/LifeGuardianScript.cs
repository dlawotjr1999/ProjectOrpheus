using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELifeGuardianAttack
{
    RIGHT_DOWN_SWING,
    LEFT_UP_SWING,
    RIGHT_UP_SWING,
    SPIKE_ATTACK,

    LAST
}

public enum ELifeGuardianSkill
{
    SPIKE,
    DRAIN,
    RUSH,

    LAST
}

public class LifeGuardianScript : BossMonster
{
    public override void AddSkillState() { m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<LifeGuardianSkillState>(); }

    private bool AttackProceed { get; set; }
    private readonly float[] AttackAngle = new float[4] { 0, 30, -30, -15 };

    [SerializeField]
    private AudioClip m_spikeSound;


    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            rot += AttackAngle[AttackIdx];
            if(rot < 0) { rot += 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)ELifeGuardianAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn();

        AttackObject.SetAttack(this, NormalDamage(AttackIdx));

        AttackProceed = AttackIdx < (int)ELifeGuardianAttack.RIGHT_UP_SWING && Random.Range(0, 2) == 0;
        m_anim.SetBool("PROCEED_ATTACK", AttackProceed);
    }
    public override void AttackDone()
    {
        IsTracing = false;
        if (AttackIdx >= (int)ELifeGuardianAttack.RIGHT_UP_SWING || !AttackProceed)
        {
            base.AttackDone();
        }
        else { AttackIdx++; }
    }


    // 스킬
    private readonly int SpikeIdx = (int)ELifeGuardianSkill.SPIKE;
    private readonly int DrainIdx = (int)ELifeGuardianSkill.DRAIN;
    private readonly int RushIdx = (int)ELifeGuardianSkill.RUSH;


    public bool RushStarted { get; private set; }

    [SerializeField]
    private float m_rushSpeed = 8;
    [SerializeField]
    private float m_drainRadius = 15;
    [SerializeField]
    private AudioClip[] m_skillSounds;
    [SerializeField]
    private AudioClip m_rushReadySound;

    public float DrainRadius { get { return m_drainRadius; } }

    public override void StartSkill()
    {
        base.StartSkill();
        RushStarted = false;
        if(CurSkillIdx == (int)ELifeGuardianSkill.RUSH) { GameManager.PlaySE(m_rushReadySound, transform.position); }
    }
    public override void SkillOn()
    {
        base.SkillOn();
        if (CurSkillIdx == SpikeIdx)
        {
            CurSkill = SkillList[SpikeIdx];
            CurSkill.SetAttack(this, SkillDamages[SpikeIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            GameManager.PlaySE(m_skillSounds[0], CurSkill.transform.position);
        }
        else if (CurSkillIdx == DrainIdx)
        {
            CurSkill = SkillList[DrainIdx];
            CurSkill.SetAttack(this, SkillDamages[DrainIdx]);
            CurSkill.AttackOn();
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill = SkillList[RushIdx];
            CurSkill.SetAttack(this, SkillDamages[RushIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            RushStarted = true;
            GameManager.PlaySE(m_skillSounds[2], CurSkill.transform.position);
        }
    }
    public override void SkillOff()
    {
        base.SkillOff();
        if (CurSkillIdx == SpikeIdx)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
        }
        else if (CurSkillIdx == DrainIdx)
        {
            CurSkill.AttackOff();
        }
        else if (CurSkillIdx == RushIdx)
        {
            CurSkill.AttackOff();
            CurSkill.gameObject.SetActive(false);
            RushStarted = false;
        }
    }
    public void RushForward()
    {
        if (!CreatedSkill)
        {
            Vector2 dir = (CurTarget.Position2-Position2).normalized;
            RotateToDir(dir, ERotateSpeed.DEFAULT);
        }

        m_rigid.velocity = m_rushSpeed * transform.forward;
    }
    public override void SkillDone()
    {
        if (CurSkillIdx == DrainIdx)
        {
            m_skillManager.SkillUsed(CurSkillIdx);
            ChangeState(EMonsterState.ATTACK);
        }
        else
        {
            base.SkillDone();
        }
    }
}