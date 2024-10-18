using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECrystalGuardianAttack
{
    LEFT_SWING,
    RIGHT_SWING,
    LEFT_SPIKE,
    RIGHT_SPIKE,
    JUMP_ATTACK,

    LAST
}

public enum ECrystalGuardianSkill
{
    DOUBLE_SWING,
    JUMP_SKILL,
    IMPACT,

    LAST
}

public class CrystalGuardianScript : BossMonster
{
    // 공격
    public enum ENextAttack { SWING, SPIKE, NONE };
    private int NextAttack { get; set; }

    private int SequenceCount { get; set; } = 0;
    private bool IsSequencing { get; set; }

    private readonly float[] AttackAngle = new float[(int)ECrystalGuardianAttack.LAST] { 0, 0, 22, -22, 0 };
    private readonly float[] SkillAngle = new float[(int)ECrystalGuardianSkill.LAST] { 0, 0, 0 };


    public override bool CheckNormalCount() { return IsCorrectNormalnum((int)ECrystalGuardianAttack.LAST); }
    [SerializeField]
    private int m_maxSequence = 5;


    public override void LookTarget()
    {
        if (CurTarget == null) { return; }

        Vector2 dir = (CurTarget.Position2 - Position2).normalized;
        if (IsTracing)
        {
            float rot = FunctionDefine.VecToDeg(dir);
            if (IsSkilling) { rot += SkillAngle[CurSkillIdx]; }
            else { rot += AttackAngle[AttackIdx]; }
            if (rot < -180) { rot += 360; } else if(rot > 180) { rot -= 360; }
            dir = FunctionDefine.DegToVec(rot);
        }
        RotateToDir(dir, ERotateSpeed.SLOW);
    }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)ECrystalGuardianAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        SequenceCount = 0;
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        int objIdx = AttackIdx % 2;
        AttackObject = m_normalAttacks[objIdx].GetComponent<AnimateAttackScript>();

        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
        AttackObject.AttackOn();
        if(TargetDistance > AttackRange) { MoveForward(); }

        if (++SequenceCount >= m_maxSequence) { NextAttack = (int)ENextAttack.NONE; }
        else { NextAttack = Random.Range(0, (int)ENextAttack.NONE + 1); }

        m_anim.SetInteger("PROCEED_IDX", NextAttack);
        IsSequencing = NextAttack < (int)ENextAttack.NONE;
        PlayAttackSound(AttackIdx);
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
        AttackIdx = AttackIdx % 2 == 0 ? NextAttack * 2 + 1 : NextAttack * 2;
    }
    public override void CreateAttack()
    {
        if (AttackIdx == (int)ECrystalGuardianAttack.JUMP_ATTACK)
        {
            AttackObject = m_normalAttacks[2].GetComponent<NormalAttackScript>();

            AttackObject.SetAttack(this, NormalDamage(AttackIdx));
            AttackObject.AttackOn();
            AttackObject.PlayEffect();
            PlayAttackSound(AttackIdx);
            CreateImpulse(15);
        }
    }
    public override void AttackDone()
    {
        if (IsSequencing) { return; }
        base.AttackDone();
        IsTracing = false;
    }


    // 스킬
    public override float AttackRange => AnySkillTimeCheck ? base.AttackRange * 2 : base.AttackRange;

    private readonly int DoubleSwingIdx = (int)ECrystalGuardianSkill.DOUBLE_SWING;
    private readonly int JumpSkillIdx = (int)ECrystalGuardianSkill.JUMP_SKILL;
    private readonly int ImpactIdx = (int)ECrystalGuardianSkill.IMPACT;

    private int DoubleIdx { get; set; }

    [SerializeField]
    private float m_skillForwardForce = 7.5f;
    [SerializeField]
    private AudioClip[] m_skillSound;
    private readonly float MaxImpulseDistance = 20;


    public override void StartSkill()
    {
        base.StartSkill();
        if(CurSkillIdx == DoubleSwingIdx) { DoubleIdx = 0; }
    }
    public override void SkillOn()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            CurSkill = m_normalAttacks[DoubleIdx++].GetComponent<ObjectAttackScript>();
            CurSkill.SetAttack(this, SkillDamages[DoubleSwingIdx]);
            CurSkill.gameObject.SetActive(true);
            CurSkill.AttackOn();
            GameManager.PlaySE(m_skillSound[DoubleSwingIdx], transform.position);
        }
    }
    public override void CreateSkill()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            MoveForward();
        }
        else if (CurSkillIdx == JumpSkillIdx)
        {
            for (int i = 0; i<2; i++)
            {
                SkillList[i].SetAttack(this, SkillDamages[JumpSkillIdx]);
                SkillList[i].AttackOn();
                GameManager.PlaySE(m_skillSound[JumpSkillIdx], transform.position);
            }
            CreateImpulse(20);
            m_anim.SetBool("IS_SKILLING", false);
        }
        else if (CurSkillIdx == ImpactIdx)
        {
            SkillList[2].SetAttack(this, SkillDamages[ImpactIdx]);
            SkillList[2].AttackOn();
            GameManager.PlaySE(m_skillSound[ImpactIdx], transform.position);
            CreateImpulse(20);
            m_anim.SetBool("IS_SKILLING", false);
        }
    }
    private void CreateImpulse(float _max)
    {
        float distance = Vector2.Distance(PlayManager.PlayerPos2, Position2);
        float impulse = Mathf.Sqrt(1-distance / MaxImpulseDistance) * 0.9f + 0.1f;
        PlayManager.CreateImpulse(impulse);
    }
    public override void SkillOff()
    {
        if (CurSkillIdx == DoubleSwingIdx)
        {
            CurSkill.AttackOff();
            if(DoubleIdx == 2) { m_anim.SetBool("IS_SKILLING", false); }
        }
    }

    private void MoveForward()
    {
        m_rigid.AddForce(m_skillForwardForce * transform.forward, ForceMode.VelocityChange);
    }


    public override void SetStates()
    {
        base.SetStates();
        m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<CrystalGuardianSkillState>();
    }
}
