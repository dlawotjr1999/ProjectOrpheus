using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloScript : MonsterScript
{
    public override void AddSkillState() { m_monsterStates[(int)EMonsterState.SKILL] = gameObject.AddComponent<BloRushState>(); }

    public override bool CanPurify => !AbsorbedSoul;
    public override bool CanSkill => base.CanSkill && !RushDone;

    protected bool AbsorbedSoul { get; set; }

    [Tooltip("돌진 속도")]
    [SerializeField]
    private float m_rushSpeed = 6;
    [Tooltip("영혼 흡수량")]
    [SerializeField]
    public int AbsorbAmount = 2;
    [SerializeField]
    private VisualEffect m_abosrbEffect;
    [SerializeField]
    private AudioClip m_absorbSound;


    private bool RushDone { get; set; }

    public bool IsRushing { get; private set; }

    public override void CreateAttack()
    {
        base.AttackTriggerOn(0);
    }
    public override void AttackDone()
    {
        m_anim.SetBool("IS_SKILLING", false);
        if (!RushDone) { RushDone = true; }
        base.AttackDone();
    }
    public void SetRush(bool _start)
    {
        m_anim.SetBool("IS_SKILLING", _start);
        if (_start) { PlayRushSound(); }
        else { AttackDone(); AttackTriggerOff(); }
        IsRushing = _start;
    }
    public virtual void PlayRushSound()
    {
        PlayAttackSound(1);
    }

    public void RushToTarget()
    {
        if(!IsRushing) { return; }
        Vector3 dir = transform.forward;
        Vector3 rot = (dir + (CurTarget.Position - Position).normalized * 0.1f).normalized;
        Vector2 rot2 = new(rot.x, rot.z);
        RotateToDir(rot2, ERotateSpeed.DEFAULT);
        m_rigid.velocity = m_rushSpeed * dir.normalized;
    }


    public override void SkillOn()
    {
        CurSkill = SkillList[0];
        ((MonsterSkillScript)CurSkill).SetAttack(this, SkillDamages[0], -1);
        CurSkill.AttackOn();
    }
    public override void SkillOff()
    {
        CurSkill.AttackOff();
        IsRushing = false;
    }


    public override void AttackedPlayer(HitData _hit)
    {
        if (!IsRushing) { return; }
        if (!AbsorbedSoul) { AbsorbSoul(); }
        SetRush(false);
        SkillOff();
        SkillDone();
        m_abosrbEffect.Play();
    }
    private void AbsorbSoul()
    {
        int soul = PlayManager.SoulNum;
        int absorb = FunctionDefine.Min(AbsorbAmount, soul);
        PlayManager.LooseSoul(absorb, true);
        GameManager.PlaySE(m_absorbSound, transform.position);
        AbsorbedSoul = true;
    }
    public override void SkillDone()
    {
        if (!IsSkilling) { return; }
        base.SkillOff();
        base.SkillDone();
    }


    public override void OnSpawned()
    {
        base.OnSpawned();
        RushDone = false;
        AbsorbedSoul = false;
    }
}
