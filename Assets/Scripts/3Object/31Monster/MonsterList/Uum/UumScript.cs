using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.VFX;

public enum EUumAttack
{
    LEFT_SWING,
    RIGHT_SWING,
    SMASH,

    LAST
}
public enum EUumPart
{
    ARM_RD,
    ARM_RU,
    CHEST,
    ARM_LU,
    ARM_LD,
    AROUND,

    LAST
}

public enum EUumSkill
{
    LEFT_SPIKE,
    RIGHT_SPIKE,

    LAST
}

public class UumScript : AnimatedAttackMonster
{
    public override bool CanPurify => SkillTimeCheck;

    [SerializeField]
    private VisualEffect m_headFire;

/*    [SerializeField]
    private readonly float m_purifyTime = 5;*/


    private readonly float NarrowAttackMultiplier = 1.5f;

    private readonly List<ObjectAttackScript> AttackObjects = new();

    private readonly int[] RightAttackIdx = new int[] { 0, 1, 2 };
    private readonly int[] LeftAttackIdx = new int[] { 2, 3, 4 };

    [Tooltip("내려찍기 / 휩쓸기 공격 배율")]
    [SerializeField]
    private float[] m_skillDamageMultiplier = new float[2] { 1, 1 };
    [SerializeField]
    private AudioClip m_smashSound;
    [SerializeField]
    private AudioClip m_sweepSound;

    private readonly float MaxImpulseDistance = 15;

    public override void StartAttack()
    {
        SetAttackIdx(Random.Range(0, (int)EUumAttack.LAST));
        AttackObjects.Clear();
        if(AttackIdx == (int)EUumAttack.SMASH) { PlayAttackSound(AttackIdx); }
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        AttackObject = m_normalAttacks[(int)EUumPart.AROUND].GetComponent<ObjectAttackScript>();
        AttackObject.SetAttack(this, NormalDamage(AttackIdx) * NarrowAttackMultiplier);
        AttackObject.AttackOn();
        GameManager.PlaySE(m_smashSound, AttackObject.transform.position);
    }
    public override void AttackTriggerOn()
    {
        int[] attackList;
        if(AttackIdx == (int)EUumAttack.LEFT_SWING) { attackList = RightAttackIdx; }
        else { attackList = LeftAttackIdx; }
        foreach (int idx in attackList)
        {
            ObjectAttackScript attack = m_normalAttacks[idx].GetComponent<ObjectAttackScript>();
            attack.SetAttack(this, NormalDamage(AttackIdx));
            attack.AttackOn();
        }
        if(AttackIdx != (int)EUumAttack.SMASH) { PlayAttackSound(AttackIdx); }
    }
    public override void AttackTriggerOff()
    {
        int[] attackList;
        if (AttackIdx == (int)EUumAttack.LEFT_SWING) { attackList = RightAttackIdx; }
        else { attackList = LeftAttackIdx; }
        foreach (int idx in attackList)
        {
            ObjectAttackScript attack = m_normalAttacks[idx].GetComponent<ObjectAttackScript>();
            attack.SetAttack(this, NormalDamage(AttackIdx));
            attack.AttackOff();
        }
    }


    public override void StartSkill()
    {
        PlayAttackSound((int)EUumAttack.SMASH);
        base.StartSkill();
    }

    public override void SkillOn()
    {
        int[] partList;
        if (CurSkillIdx == (int)EUumSkill.LEFT_SPIKE) { partList = RightAttackIdx; }
        else { partList = LeftAttackIdx; }

        GameManager.PlaySE(m_sweepSound, transform.position);

        foreach (int idx in partList)
        {
            ObjectAttackScript attack = m_normalAttacks[idx].GetComponent<ObjectAttackScript>();
            float damage = SkillDamages[CurSkillIdx] * m_skillDamageMultiplier[1];
            attack.SetAttack(this, damage);
            attack.SetCCType(ECCType.KNOCKBACK);
            attack.AttackOn();
        }
    }
    public override void CreateSkill()
    {
        ObjectAttackScript skill;
        skill = SkillList[CurSkillIdx];
        float damage = SkillDamages[CurSkillIdx] * m_skillDamageMultiplier[0];
        skill.SetAttack(this, damage);
        skill.AttackOn();
        GameManager.PlaySE(m_smashSound, skill.transform.position);
        CreateSmashImpulse();

        base.CreateSkill();
    }
    private void CreateSmashImpulse()
    {
        float distance = Vector2.Distance(PlayManager.PlayerPos2, Position2);
        float impulse = Mathf.Sqrt(1-distance / MaxImpulseDistance) * 0.9f + 0.1f;
        PlayManager.CreateImpulse(impulse);
    }
    public override void SkillOff()
    {
        int[] partList;
        if (CurSkillIdx == (int)EUumSkill.LEFT_SPIKE) { partList = RightAttackIdx; }
        else { partList = LeftAttackIdx; }

        foreach (int idx in partList)
        {
            ObjectAttackScript attack = m_normalAttacks[idx].GetComponent<ObjectAttackScript>();
            attack.AttackOff();
            attack.ResetCCType();
        }
        base.SkillOff();
    }


    public override void StartDissolve()
    {
        base.StartDissolve();
        m_headFire.Stop();
    }
}
