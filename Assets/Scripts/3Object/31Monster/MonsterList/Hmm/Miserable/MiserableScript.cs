using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EMiserableAttack
{
    LEFT_HEAD,
    RIGHT_HEAD,
    HEAD_BUMP,
    TRIPLE_ATTACK,

    LAST
}

public class MiserableScript : HmmScript
{
    public override bool CanPurify => IsMelancholy;


    [SerializeField]
    private VisualEffect m_bumpEffect;
    [SerializeField]
    private AudioClip m_bumpSound;
    [Tooltip("3연격 시 전진 힘")]
    [SerializeField]
    private float m_tripleAttackForward = 8;
    [SerializeField]
    private AudioClip m_swingSound;

    private bool IsTripleAttack { get { return AttackIdx == (int)EMiserableAttack.TRIPLE_ATTACK; } }
    private int SkillIdx { get; set; }
    public override void StartAttack()
    {
        if(IsTripleAttack) { SkillIdx = 0; }
        AttackIdx =  Random.Range(0, (int)EMiserableAttack.LAST);
        m_anim.SetInteger("ATTACK_IDX", AttackIdx);
        base.StartAttack();
    }
    public override void CreateAttack()
    {
        if (AttackIdx == (int)EMiserableAttack.HEAD_BUMP)
        {
            m_bumpEffect.Play();
            GameManager.PlaySE(m_bumpSound, transform.position);
        }
        else if (AttackIdx == (int)EMiserableAttack.TRIPLE_ATTACK)
        {
            GameManager.PlaySE(m_swingSound, transform.position);
            m_rigid.velocity = m_tripleAttackForward * transform.forward;
        }
    }
    public override void AttackTriggerOn()
    {
        int idx;
        if (!IsTripleAttack) { idx = 0; }
        else { idx = SkillIdx % 2 + 1; SkillIdx++; }
        AttackTriggerOn(idx);
        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
    }
    public override void AttackTriggerOff()
    {
        AttackObject.AttackOff();
    }
    public override void AttackDone()
    {
        base.AttackDone();
        if(AttackIdx <= 1) { AttackTimeCount *= 0.5f; }
    }
}
