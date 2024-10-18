using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EMarmulakAttack
{
    THROW,
    LEFT_SWING,
    RIGHT_SWING,

    LAST
}

public enum EMarmulakSkill
{
    ROAR,

    LAST
}

public class MarmulakScript : RangedAttackMonster
{
    public override bool CanPurify => SkillTimeCount[0] >= (m_roarCooltime - m_purifyTime);

    public override float AttackRange => SkillTimeCheck ? m_roarRadius : (TargetDistance <= m_roarRadius) ? base.AttackRange : m_throwRange;


    [Tooltip("던지기 공격 범위")]
    [SerializeField]
    private float m_throwRange = 25;


    public override void StartAttack()
    {
        if (TargetDistance <= m_roarRadius)
        {
            SetAttackIdx(Random.Range((int)EMarmulakAttack.LEFT_SWING, (int)EMarmulakAttack.LAST));
        }
        else
        {
            SetAttackIdx((int)EMarmulakAttack.THROW);
        }
        base.StartAttack();
    }

    public override Vector3 AttackOffset => new(0.63f, 1.391f, 1.321f);
    public void ThrowBall()
    {
        base.CreateAttack();
    }
    public override void CreateAttack()
    {
        ThrowBall();
    }
    public override void AttackTriggerOn()
    {
        base.AttackTriggerOn(AttackIdx-1);
        AttackObject.SetAttack(this, NormalDamage(AttackIdx));
    }

    [Tooltip("포효 쿨타임")]
    [SerializeField]
    private float m_roarCooltime = 15;
    [Tooltip("포효 범위")]
    [SerializeField]
    public float m_roarRadius = 10;
    [Tooltip("포효 데미지")]
    [SerializeField]
    private float m_roarDamage = 10;

    [Tooltip("포효 후 성불 기간(초)")]
    [SerializeField]
    private float m_purifyTime = 8;
    [SerializeField]
    private AudioClip m_roarSound;


    private readonly List<ObjectScript> m_roarList = new();
    public override void StartSkill()
    {
        base.StartSkill();
    }
    public override void SkillOn()
    {
        MarmulakRoarEffect roar = SkillList[0] as MarmulakRoarEffect;
        roar.Play();
        GameManager.PlaySE(m_roarSound, SkillList[0].transform.position);
    }
    public override void CreateSkill()
    {
        CheckNRoar();
    }
    public override void SkillOff()
    {
        MarmulakRoarEffect roar = SkillList[0] as MarmulakRoarEffect;
        roar.Stop();
        base.SkillOff();
    }

    public void CheckNRoar()
    {
        m_roarList.Clear();
        Collider[] targets = Physics.OverlapSphere(SkillList[0].transform.position, m_roarRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        for (int i = 0; i<targets.Length; i++)
        {
            PlayerController player = targets[i].GetComponentInParent<PlayerController>();
            if (player == null || player == this || m_roarList.Contains(player)) { continue; }
            HitData hit = new(this, m_roarDamage, player.Position, new ECCType[2] { ECCType.FATIGUE, ECCType.KNOCKBACK });
            player.GetHit(hit);
            m_roarList.Add(player);
        }
    }
}
