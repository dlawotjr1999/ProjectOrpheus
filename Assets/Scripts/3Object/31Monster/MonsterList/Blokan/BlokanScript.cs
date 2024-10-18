using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBlokanAttack
{
    UP_SWING,
    FRONT,

    LAST
}

public class BlokanScript : BloScript
{
    public override bool CanPurify => AbsorbedSoul;


    private readonly Vector3[] NormalAttackOffsets = new Vector3[(int)EBlokanAttack.LAST]
        { new(0,1.5f,1.3f), new(0,1,1.3f) };

    public override void AttackAnimation()
    {
        AttackIdx = Random.Range(0, (int)EBlokanAttack.LAST);
        m_anim.SetInteger("RANDOM_IDX", AttackIdx);
        base.AttackAnimation();
    }

    public override void PlayRushSound()
    {
        PlayAttackSound(2);
    }
    public override void CreateAttack()
    {
        base.CreateAttack();
        m_normalAttacks[0].transform.localPosition = NormalAttackOffsets[AttackIdx];
    }
}
