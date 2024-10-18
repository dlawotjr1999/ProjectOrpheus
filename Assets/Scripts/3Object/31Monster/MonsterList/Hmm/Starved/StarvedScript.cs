using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStarvedAttack
{
    SHORT,
    LONG,

    LAST
}

public enum EStarvedSkill
{
    BUMP,

    LAST
}

public class StarvedScript : HmmScript
{
    public override bool CanPurify => IsExtorted;

    public override bool CheckNormalCount() { return m_normalDamageMultiplier.Length == (int)EStarvedAttack.LAST; }

    public override void StartAttack()
    {
        AttackIdx = Random.Range(0, (int)EStarvedAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void AttackTriggerOn()
    {
        AttackTriggerOn(0);
        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }

    [SerializeField]
    private AudioClip m_skillSound;
    public override void StartSkill()
    {
        base.StartSkill();
        PlayAttackSound(0);
    }
    public override void CreateSkill()
    {
        base.CreateSkill();
        ObjectAttackScript skill = SkillList[CurSkillIdx];
        skill.SetAttack(this, SkillDamages[CurSkillIdx]);
        ((EffectedNormalAttack)skill).AttackOn(1);
        GameManager.PlaySE(m_skillSound, transform.position);
    }
}
